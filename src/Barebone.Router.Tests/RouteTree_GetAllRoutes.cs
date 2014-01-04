using System;
using Xunit;
using System.Linq;

namespace Barebone.Routing.Tests
{
	public class RouteTree_GetAllRoutes{
		[Fact]
		public void Returns_all_routes(){
			var routeTree = new RouteTree();
			var route1 = new Route("GET", "/test");
			routeTree.Add(route1);

			var route2 = new Route("GET", "/products/{id}");
			routeTree.Add(route2);

			var route3 = new Route("GET", "/products/new/{id}");
			routeTree.Add(route3);

			var route4 = new Route("GET", "/{id}");
			routeTree.Add(route4);

			var allRoutes = new[] { route1, route2, route3, route4 }.OrderBy(x => x.Path);
			var resolvedRoutes = routeTree.GetAllRoutes().OrderBy(x => x.Path);
			Assert.Equal(allRoutes, resolvedRoutes);
		}
	}
}