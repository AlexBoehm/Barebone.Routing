using System;
using System.Collections.Generic;
using System.Linq;

namespace Barebone.Routing
{
	public class Routes{
		List<RouteRegistration> _routes = new List<RouteRegistration>();

		public void Add(RouteRegistration registration){
			_routes.Add(registration);
		}

		public List<Route> GetRouteEntries(){
			return _routes.Select(x => x.RouteEntry).ToList();
		}
	}
}