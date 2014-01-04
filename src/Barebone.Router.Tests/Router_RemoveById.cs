using System;
using Xunit;

namespace Barebone.Routing.Tests
{
	public class Router_RemoveById : RouterTestBase{
		[Fact]
		public void Route_can_be_removed_by_id(){
			var table = new Router();
			table.AddRoute(new Route("my-id", "GET", "/foo", App));
			table.RemoveRoute("my-id");

			var result = table.Resolve(Utils.BuildGetRequest("/foo"));
       		Assert.False(result.Success);
		}

		[Fact]
		public void RemoveById_throws_ArgumentException_if_id_is_not_available(){
			var table = new Router();
			Assert.Throws<ArgumentException>(() => table.RemoveRoute("route-id"));
		}
	}
}
