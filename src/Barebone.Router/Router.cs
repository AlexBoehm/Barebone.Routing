using System;
using System.Collections.Generic;
using System.Linq;

namespace Barebone.Routing
{
	using OwinEnv = IDictionary<string, object>;

	/// <summary>
	/// Router for owin Requests.
	/// 
	/// You can register routes within the router. If you pass an owin request (owin environment)
	/// it tries to find a route which can handle the request. You have to call the resolved AppFunc
	/// by yourself. The router does NOT dispath the request.
	/// </summary>
	public class Router{
		RouteTree _routes = new RouteTree();

		/// <summary>
		/// Registers routes in the router.
		/// </summary>
		public void AddRoutes(IEnumerable<Route> routes){
			foreach (var route in routes) {
				AddRoute(route);
			}
		}

		/// <summary>
		/// Registers a route in the router.
		/// </summary>
		public void AddRoute(Route route){
			_routes.Add(route);
		}

        public ResolveResult Resolve(OwinEnv env)
        {
            return Internal_Resolve(env, null);
        }

        public ResolveResult Resolve(OwinEnv env, Func<Route, bool> predicate)
        {
            return Internal_Resolve(env, predicate);
        }

		/// <summary>
		/// Finds a route for the request.
		/// </summary>
		public ResolveResult Internal_Resolve(OwinEnv env, Func<Route, bool> predicate = null){
			var path = env ["owin.RequestPath"] as string;
            var method = env["owin.RequestMethod"] as string;
			var candidates = _routes.GetCandidates(path, method);
			var segments = path.Substring(1, path.Length - 1).Split('/');
			var routesWithMatchingPath = new List<MatchingRoute>();

			foreach (var route in candidates) {
				IDictionary<string, RouteValue> parameters;

				if (!PathMatcher.Matches(route, segments, out parameters))
					continue;

				routesWithMatchingPath.Add(new MatchingRoute(route, parameters));
			}

			foreach (var item in routesWithMatchingPath.OrderByDescending(x => x.Route.Priority)) {
				if (!CheckConditions(item.Route, env, item.Parameters))
					continue;

				if (!CheckParameterConditions(item.Route, item.Parameters))
					continue;

                if (predicate != null && !predicate.Invoke(item.Route))
                    continue;

				return ResolveResult.RouteFound(item.Route, item.Parameters);
			}

			return ResolveResult.NoResult();
		}

		/// <summary>
		/// Removes a route by id.
		/// </summary>
		public void RemoveRoute(string routeId){
			_routes.RemoveRoute(routeId);
		}

		/// <summary>
		/// Removes the given route.
		/// </summary>
		/// <param name="route">Route.</param>
		public void RemoveRoute(Route route){
			_routes.RemoveRoute(route);
		}

		/// <summary>or sets
		/// Returns all reigstered routes.
		/// </summary>
		/// <returns>The all routes.</returns>
		public Route[] GetAllRoutes(){
			return _routes.GetAllRoutes();
		}

		private bool CheckConditions(Route route, OwinEnv env, IDictionary<string, RouteValue> parameters){
			if (route.Conditions == null)
				return true;

			foreach (var condition in route.Conditions) {
				if(!condition.Condition(new RouteConditionData(env, parameters, route, condition.Data)))
				   return false;
			}

			return true;
		}

		private bool CheckParameterConditions(Route route, IDictionary<string, RouteValue> parameters){
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
			public IDictionary<string, RouteValue> Parameters {get; private set;}

			public MatchingRoute(Route route, IDictionary<string, RouteValue> parameters){
				Route = route;
				Parameters = parameters;
			}
		}
	}
}
