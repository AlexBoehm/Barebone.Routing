using System;
using System.Collections.Generic;
using System.Linq;

namespace Barebone.Routing
{
	public class Routes{
		List<RouteRegistrationChain> _routes = new List<RouteRegistrationChain>();

		public void Add(RouteRegistrationChain registration){
			_routes.Add(registration);
		}

		public List<Route> GetRouteEntries(){
			return _routes.Select(x => x.RouteEntry).ToList();
		}
	}
}