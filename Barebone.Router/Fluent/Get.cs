using System;

namespace Barebone.Routing
{
	public static class RoutesExtensions{
		public static RouteRegistrationChain Get(this Routes routes, string path){
			var registration = new RouteRegistrationChain("GET", path);
			routes.Add(registration);
			return registration;
		}

		public static RouteRegistrationChain Post(this Routes routes, string path){
			var registration = new RouteRegistrationChain("POST", path);
			routes.Add(registration);
			return registration;
		}
	}
}

