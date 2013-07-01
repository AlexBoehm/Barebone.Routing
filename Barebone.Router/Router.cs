using System;
using System.Collections.Generic;

namespace Barebone.Router
{
	using OwinEnv = IDictionary<string, object>;

	public class Router{
		RouteTree _routes = new RouteTree();

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

			foreach (var route in candidates) {
				// Method prüfen
				IDictionary<string, string> parameters;

				if (!RouteMatcher.Matches(route, segments, out parameters))
					continue;

				if (!CheckConditions(route, env, parameters))
					continue;

				if (!CheckParameterConditions(route, parameters))
					continue;

				return ResolveResult.RouteFound(route, parameters);
			}

			return ResolveResult.NoResult();
		}

		bool CheckConditions(Route route, OwinEnv env, IDictionary<string, string> parameters){
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
	}
}

