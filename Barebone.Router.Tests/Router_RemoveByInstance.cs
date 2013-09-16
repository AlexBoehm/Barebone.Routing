using System;
using Xunit;

namespace Barebone.Routing.Tests
{
	public class Router_RemoveByInstance : RouterTestBase{
		[Fact]
		public void Route_can_be_removed(){
			var table = new Router();
			var route = new Route("GET", "/foo", App);
			table.AddRoute(route);
			table.RemoveRoute(route);

			var result = table.Resolve(Utils.BuildGetRequest("/foo"));
			Assert.False(result.Success);
		}

		[Fact]
		public void Route_with_two_static_segments_can_be_removed(){
			var table = new Router();
			var route = new Route("GET", "/foo/bar", App);
			table.AddRoute(route);
			table.RemoveRoute(route);

			var result = table.Resolve(Utils.BuildGetRequest("/foo"));
			Assert.False(result.Success);
		}

		[Fact]
		public void Route_with_static_and_dynamic_segment_can_be_removed(){
			var table = new Router();
			var route = new Route("GET", "/foo/{id}", App);
			table.AddRoute(route);

			table.RemoveRoute(route);

			var result = table.Resolve(Utils.BuildGetRequest("/foo/12"));
			Assert.False(result.Success);
		}

		[Fact]
		public void Route_dynamic_segment_can_be_removed(){
			var table = new Router();
			var route = new Route("GET", "/{id}", App);
			table.AddRoute(route);

			table.RemoveRoute(route);

			var result = table.Resolve(Utils.BuildGetRequest("/12"));
			Assert.False(result.Success);
		}

		[Fact]
		public void Throws_argument_exception_if_RemoveRoute_is_call_with_null_parameter(){
			var table = new Router();
			Assert.Throws<ArgumentNullException>(() => table.RemoveRoute((string)null));
		}

		[Fact]
		public void Throws_argument_exception_if_RemoveRoute_is_called_with_empty_string(){
			var table = new Router();
			Assert.Throws<ArgumentException>(() => table.RemoveRoute(string.Empty));
		}
	}
}
