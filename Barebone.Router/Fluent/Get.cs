using System;

namespace Barebone.Routing
{
	public static class RoutesExtensions{
		public static RouteRegistration Get(this Routes routes, string path){
			var registration = new RouteRegistration("GET", path);
			routes.Add(registration);
			return registration;
		}
	}
}

