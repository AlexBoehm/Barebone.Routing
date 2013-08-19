using System;
using Xunit;

namespace Barebone.Router.Tests
{
	public class Router_Data : RouterTestBase{ 
		[Fact]
		public void Route_data_can_be_stored_and_retrieved(){
			var router = new Router();
			var route = new Route("GET", "/foo/test", App);
			route.Data["Test1"] = "data1";
			route.Data["Test2"] = "data2";
			router.AddRoute(route);

			var result = router.Resolve(Utils.BuildGetRequest("/foo/test"));

			Assert.Equal("data1", (string) result.Route.Data["Test1"]);
			Assert.Equal("data2", (string) result.Route.Data["Test2"]);
		}
	}
}

