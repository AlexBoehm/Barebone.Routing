using System;
using System.Collections.Generic;
using System.Linq;

namespace Barebone.Routing
{
	using OwinEnv = IDictionary<string, object>;

	public class Router{
		RouteTree _routes = new RouteTree();

		public void AddRoutes(IEnumerable<Route> routes){
			foreach (var route in routes) {
				AddRoute(route);
			}
		}

		public void AddRoute(Route route){
			_routes.Add(route);
		}

		public ResolveResult Resolve (OwinEnv env){
			var path = env ["owin.RequestPath"] as string;
			var method = env ["owin.RequestMethod"] as string;
			return Resolve(method, path, env);
		}

		public ResolveResult Resolve(string method, string path, OwinEnv env){
			var candidates = _routes.GetCandidates(path);
			var segments = path.Substring(1, path.Length - 1).Split('/');
			var routesWithMatchingPath = new List<MatchingRoute>();

			foreach (var route in candidates) {
				// Method prüfen
				IDictionary<string, string> parameters;

				if (!PathMatcher.Matches(route, segments, out parameters))
					continue;

				routesWithMatchingPath.Add(new MatchingRoute(route, parameters));

				//return ResolveResult.RouteFound(route, parameters);
			}

			foreach (var item in routesWithMatchingPath.OrderByDescending(x => x.Route.Priority)) {
				if (!CheckConditions(item.Route, env, item.Parameters))
					continue;

				if (!CheckParameterConditions(item.Route, item.Parameters))
					continue;

				return ResolveResult.RouteFound(item.Route, item.Parameters);
			}

			return ResolveResult.NoResult();
		}

		public void RemoveRoute(string routeId){
			_routes.RemoveRoute(routeId);
		}

		public void RemoveRoute(Route route){
			_routes.RemoveRoute(route);
		}

		public Route[] GetAllRoutes(){
			return _routes.GetAllRoutes();
		}

		private bool CheckConditions(Route route, OwinEnv env, IDictionary<string, string> parameters){
			if (route.Conditions == null)
				return true;

			foreach (var condition in route.Conditions) {
				if(!condition.Condition(new RouteConditionData(env, parameters, route, condition.Data)))
				   return false;
			}

			return true;
		}

		private bool CheckParameterConditions(Route route, IDictionary<string, string> parameters){
			if (route.ParameterConditions == null)
				return true;

			foreach (var conditionsEntry in route.ParameterConditions) {
				var parameterName = conditionsEntry.Key;
				var conditions = conditionsEntry.Value;

				foreach (var condition in conditions) {
					if (!condition.Invoke(parameters[parameterName]))
						return false;
				}
			}

			return true;
		}

		private class MatchingRoute {
			public Route Route { get; private set; }
			public IDictionary<string, string> Parameters {get; private set;}

			public MatchingRoute(Route route, IDictionary<string, string> parameters){
				Route = route;
				Parameters = parameters;
			}
		}
	}
}
