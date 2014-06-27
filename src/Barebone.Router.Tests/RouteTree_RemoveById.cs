using System;
using Xunit;

namespace Barebone.Routing.Tests
{
	public class RouteTree_RemoveById : RouteTreeTestBase{
		[Fact]
		public void Route_can_be_removed_by_id(){
			var table = new RouteTree();
			table.Add(new Route("my-id", "GET", "/foo", App));
			table.RemoveRoute("my-id");

			var candiates = table.GetCandidates("/foo", "GET");
			Assert.Equal(0, candiates.Count);
		}

		[Fact]
		public void RemoveById_throws_ArgumentException_if_id_is_not_available(){
			var table = new RouteTree();
			Assert.Throws<ArgumentException>(() => table.RemoveRoute("route-id"));
		}
	}
}
