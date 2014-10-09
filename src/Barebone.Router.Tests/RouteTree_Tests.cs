using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Barebone.Routing
{
	public class RouteTree_Tests : RouteTreeTestBase{
		[Fact]
		public void Returns_Candidates(){
			var table = new RouteTree();
			var route = new Route("GET", "/foo/bar", App);
			table.Add(route);
			var candidates = table.GetCandidates("/foo/bar", "GET");
			Assert.Equal(new List<Route>{route}, candidates);
		}

		[Fact]
		public void Returns_dynamic_route_as_candiate(){
			var table = new RouteTree();
			var route1 = new Route("GET", "/foo/bar", App);
			var route2 = new Route("GET", "/foo/{action}", App);
			table.Add(route1, route2);
			var candidates = table.GetCandidates("/foo/bar", "GET");
			Assert.Equal(new List<Route>{route1, route2}, candidates);
		}

		[Fact]
		public void Returns_dynamic_route_with_multiple_parameters_in_one_segment_as_candiate(){
			var table = new RouteTree();
			var route1 = new Route("GET", "/foo/bar", App);
			var route2 = new Route("GET", "/foo/{id}-{name}", App);
			table.Add(route1, route2);
			var candidates = table.GetCandidates("/foo/bar", "GET");
			Assert.Equal(new List<Route>{route1, route2}, candidates);
		}

        [Fact]
        public void Dynamic_segement_can_contain_constant_at_start()
        {
            var table = new RouteTree();
            var route1 = new Route("GET", "/foo/bar", App);
            var route2 = new Route("GET", "/foo/any-text-{id}-{name}", App);
            table.Add(route1, route2);
            var candidates = table.GetCandidates("/foo/any-text-232-test", "GET");
            Assert.Equal(new List<Route> { route2 }, candidates);
        }

		[Fact]
		public void Does_not_return_route_with_wrong_segement_count(){
			var table = new RouteTree();
			var route1 = new Route("GET", "/foo/bar", App);
			var route2 = new Route("GET", "/foo/{action}", App);
			var route3 = new Route("GET", "/foo/bar/{action}", App);
			table.Add(route1, route2, route3);
			var candidates = table.GetCandidates("/foo/bar", "GET");
			Assert.Equal(new List<Route>{route1, route2}, candidates);
		}

		[Fact]
		public void Does_not_return_route_with_with_wrong_static_segment(){
			var table = new RouteTree();
			var route1 = new Route("GET", "/foo/bar", App);
			var route2 = new Route("GET", "/foo/{action}", App);
			var route3 = new Route("GET", "/wrong/bar", App);
			table.Add(route1, route2, route3);
			var candidates = table.GetCandidates("/foo/bar", "GET");
			Assert.Equal(new List<Route>{route1, route2}, candidates);
		}

		[Fact]
		public void Returns_Routes_ordered_by_priority(){
			var table = new RouteTree();

			var route = new Route("GET", "/foo/{action}", App);
			route.Priority = 8;
			table.Add(route);

			route = new Route("GET", "/foo/{action}", App);
			route.Priority = 6;
			table.Add(route);

			route = new Route("GET", "/foo/{action}", App);
			route.Priority = 7;
			table.Add(route);

			var candidates = table.GetCandidates("/foo/test", "GET");
			var priorities = (from x in candidates select x.Priority).ToArray();

			Assert.Equal(new[] { 8, 7, 6 }, priorities);
		}

	}
}