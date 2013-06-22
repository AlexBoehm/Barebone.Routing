using System;
using System.Collections.Generic;

namespace RouteTest
{
	public class Router{
		List<Route> _routes = new List<Route>();

		public void AddRoute(Route route){
			_routes.Add(route);
		}

//		public ResolveResult Resolve(string method, string path){
//			foreach (var route in _routes) {
//				route.PathElements
//			}
//		}
	}
}

