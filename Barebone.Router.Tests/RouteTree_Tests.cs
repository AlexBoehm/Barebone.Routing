using System;
using Xunit;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace Barebone.Routing
{
	public class RouteTree_Tests{
		private Func<IDictionary<string, object>, Task> App = (env) => Task.Factory.StartNew(() => {});

		[Fact]
		public void Returns_Candidates(){
			var table = new RouteTree();
			var route = new Route("GET", "/foo/bar", App);
			table.Add(route);
			var candidates = table.GetCandidates("/foo/bar");
			Assert.Equal(new List<Route>{route}, candidates);
		}

		[Fact]
		public void Returns_dynamic_route_as_candiate(){
			var table = new RouteTree();
			var route1 = new Route("GET", "/foo/bar", App);
			var route2 = new Route("GET", "/foo/{action}", App);
			table.Add(route1, route2);
			var candidates = table.GetCandidates("/foo/bar");
			Assert.Equal(new List<Route>{route1, route2}, candidates);
		}

		[Fact]
		public void Returns_dynamic_route_with_multiple_parameters_in_one_segment_as_candiate(){
			var table = new RouteTree();
			var route1 = new Route("GET", "/foo/bar", App);
			var route2 = new Route("GET", "/foo/{id}-{name}", App);
			table.Add(route1, route2);
			var candidates = table.GetCandidates("/foo/bar");
			Assert.Equal(new List<Route>{route1, route2}, candidates);
		}

		[Fact]
		public void Does_not_return_route_with_wrong_segement_count(){
			var table = new RouteTree();
			var route1 = new Route("GET", "/foo/bar", App);
			var route2 = new Route("GET", "/foo/{action}", App);
			var route3 = new Route("GET", "/foo/bar/{action}", App);
			table.Add(route1, route2, route3);
			var candidates = table.GetCandidates("/foo/bar");
			Assert.Equal(new List<Route>{route1, route2}, candidates);
		}

		[Fact]
		public void Does_not_return_route_with_with_wrong_static_segment(){
			var table = new RouteTree();
			var route1 = new Route("GET", "/foo/bar", App);
			var route2 = new Route("GET", "/foo/{action}", App);
			var route3 = new Route("GET", "/wrong/bar", App);
			table.Add(route1, route2, route3);
			var candidates = table.GetCandidates("/foo/bar");
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

			var candidates = table.GetCandidates("/foo/test");
			var priorities = (from x in candidates select x.Priority).ToArray();

			Assert.Equal(new[] { 8, 7, 6 }, priorities);
		}

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

		[Fact]
		public void Route_can_be_removed_by_id(){
			var router = new RouteTree();
			router.Add(new Route("my-id", "GET", "/foo", App));
			router.RemoveRoute("my-id");

			var candiates = router.GetCandidates("/foo");
			Assert.Equal(0, candiates.Count);
		}

		[Fact]
		public void Route_can_be_removed(){
			var router = new RouteTree();
			var route = new Route("GET", "/foo", App);
			router.Add(route);
			router.RemoveRoute(route);

			var candidates = router.GetCandidates("/foo");
			Assert.Equal(0, candidates.Count);
		}

		[Fact]
		public void Route_with_two_static_segments_can_be_removed(){
			var router = new RouteTree();
			var route = new Route("GET", "/foo/bar", App);
			router.Add(route);
			router.RemoveRoute(route);

			var candidates = router.GetCandidates("/foo/bar");
			Assert.Equal(0, candidates.Count);
		}

		[Fact]
		public void Route_with_static_and_dynamic_segment_can_be_removed(){
			var router = new RouteTree();
			var route = new Route("GET", "/foo/{id}", App);
			router.Add(route);
			Assert.Equal(new[] { route }, router.GetCandidates("/foo/12"));

			router.RemoveRoute(route);

			var candidates = router.GetCandidates("/foo/12");
			Assert.Equal(0, candidates.Count);
		}

		[Fact]
		public void Route_dynamic_segment_can_be_removed(){
			var router = new RouteTree();
			var route = new Route("GET", "/{id}", App);
			router.Add(route);

			Assert.Equal(new[] { route }, router.GetCandidates("/12"));
			router.RemoveRoute(route);

			var candidates = router.GetCandidates("/13");
			Assert.Equal(0, candidates.Count);
		}

		[Fact]
		public void Throws_argument_exception_if_RemoveRoute_is_call_with_null_parameter(){
			var router = new RouteTree();
			Assert.Throws<ArgumentNullException>(() => router.RemoveRoute((string)null));
		}

		[Fact]
		public void Throws_argument_exception_if_RemoveRoute_is_called_with_empty_string(){
			var router = new RouteTree();
			Assert.Throws<ArgumentException>(() => router.RemoveRoute(string.Empty));
		}

		[Fact]
		public void RemoveById_throws_ArgumentException_if_id_is_not_available(){
			var router = new RouteTree();
			Assert.Throws<ArgumentException>(() => router.RemoveRoute("route-id"));
		}
	}
}