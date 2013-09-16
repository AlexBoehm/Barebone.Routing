using System;
using Xunit;

namespace Barebone.Routing.Tests
{
	public class RouteTree_Ids : RouteTreeTestBase{
		[Fact]
		public void Two_routes_without_an_id_can_be_defined(){
			var table = new RouteTree();
			table.Add(new Route("GET", "/foo"));

			var secondRouteWithoutId = new Route("GET", "/bar");

			Assert.DoesNotThrow(() => table.Add(secondRouteWithoutId));
		}

		[Fact]
		public void If_a_route_is_added_with_an_id_that_already_exists_the_router_throws_an_exception(){
			var table = new RouteTree();
			table.Add(new Route("my-id", "GET", "/foo", App));

			var otherRouteWithSameId = new Route("my-id", "GET", "/bar", App);

			Assert.Throws<RouteAlreadyExistsException>(() => table.Add(otherRouteWithSameId));
		}
	}
}
