using System;

namespace Barebone.Routing
{
	public static class RouterExtensions{
		public static void AddRoutes(this Router router, Routes routes){
			router.AddRoutes(routes.GetRouteEntries());
		}
	}
}
