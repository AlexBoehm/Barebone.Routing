using System;
using Xunit;

namespace Barebone.Routing.Tests
{
	public class Router_Ids : RouterTestBase{
		[Fact]
		public void Route_can_be_defined_without_id(){
			var router = new Router();
			router.AddRoute(new Route("GET", "/foo", App));
			var result = router.Resolve(Utils.BuildGetRequest("/foo"));
			Assert.True(result.Success);
			Assert.Equal(App, result.Route.OwinAction);
		}

		[Fact]
		public void Two_routes_without_an_id_can_be_defined(){
			var router = new Router();
			router.AddRoute(new Route("GET", "/foo"));

			var secondRouteWithoutId = new Route("GET", "/bar");

			Assert.DoesNotThrow(() => router.AddRoute(secondRouteWithoutId));
		}

		[Fact]
		public void If_a_route_is_added_with_an_id_that_already_exists_the_router_throws_an_exception(){
			var router = new Router();
			router.AddRoute(new Route("my-id", "GET", "/foo", App));

			var otherRouteWithSameId = new Route("my-id", "GET", "/bar", App);

			Assert.Throws<RouteAlreadyExistsException>(() => router.AddRoute(otherRouteWithSameId));
		}
	}
}