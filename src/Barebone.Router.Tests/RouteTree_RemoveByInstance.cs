using System;
using Xunit;

namespace Barebone.Routing.Tests
{
	public class RouteTree_RemoveByInstance : RouteTreeTestBase{
		[Fact]
		public void Route_can_be_removed(){
			var table = new RouteTree();
			var route = new Route("GET", "/foo", App);
			table.Add(route);
			table.RemoveRoute(route);

			var candidates = table.GetCandidates("/foo");
			Assert.Equal(0, candidates.Count);
		}

		[Fact]
		public void Route_with_two_static_segments_can_be_removed(){
			var table = new RouteTree();
			var route = new Route("GET", "/foo/bar", App);
			table.Add(route);
			table.RemoveRoute(route);

			var candidates = table.GetCandidates("/foo/bar");
			Assert.Equal(0, candidates.Count);
		}

		[Fact]
		public void Route_with_static_and_dynamic_segment_can_be_removed(){
			var table = new RouteTree();
			var route = new Route("GET", "/foo/{id}", App);
			table.Add(route);
			Assert.Equal(new[] { route }, table.GetCandidates("/foo/12"));

			table.RemoveRoute(route);

			var candidates = table.GetCandidates("/foo/12");
			Assert.Equal(0, candidates.Count);
		}

		[Fact]
		public void Route_dynamic_segment_can_be_removed(){
			var table = new RouteTree();
			var route = new Route("GET", "/{id}", App);
			table.Add(route);

			Assert.Equal(new[] { route }, table.GetCandidates("/12"));
			table.RemoveRoute(route);

			var candidates = table.GetCandidates("/13");
			Assert.Equal(0, candidates.Count);
		}

		[Fact]
		public void Throws_argument_exception_if_RemoveRoute_is_call_with_null_parameter(){
			var table = new RouteTree();
			Assert.Throws<ArgumentNullException>(() => table.RemoveRoute((string)null));
		}

		[Fact]
		public void Throws_argument_exception_if_RemoveRoute_is_called_with_empty_string(){
			var table = new RouteTree();
			Assert.Throws<ArgumentException>(() => table.RemoveRoute(string.Empty));
		}
	}
}
