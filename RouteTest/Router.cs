using System;
using System.Collections.Generic;

namespace RouteTest
{
	public class Router{
		RouteTree _routes = new RouteTree();

		public void AddRoute(Route route){
			_routes.Add(route);
		}

		public ResolveResult Resolve(string method, string path){
			var candidates = _routes.GetCandidates(path);
			var segments = path.Substring(1, path.Length - 1).Split('/');

			foreach (var route in candidates) {
				// Method prüfen
				IDictionary<string, string> parameters;

				if (RouteMatcher.Matches(route, segments, out parameters)) {
					return ResolveResult.RouteFound(route, parameters);
				}
			}

			return ResolveResult.NoResult();
		}
	}
}

