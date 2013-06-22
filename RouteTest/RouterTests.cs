using System;
using Xunit;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RouteTest
{
	public class RouterTests{
		private Func<IDictionary<string, object>, Task> App = (env) => Task.Factory.StartNew(() => {});

		[Fact]
		public void Get_Static_Urls_with_one_segment_is_matched_correctly(){
			var route = new Route("GET", "/foo", App);
			var router = new Router();
			router.AddRoute(route);	
			var result = router.Resolve("GET", "/foo");
			Assert.Equal(route, result.Route);
		}

		[Fact]
		public void Get_Static_Urls_with_dynamic_segment_is_matched_correctly(){
			var route = new Route("GET", "/foo/{action}", App);
			var router = new Router();
			router.AddRoute(route);
			var result = router.Resolve("GET", "/foo/test");
			Assert.Equal(route, result.Route);
		}

		[Fact]
		public void Get_Static_Urls_with_dynamic_segment_and_multiple_parameters_is_matched_correctly(){
			var route = new Route("GET", "/foo/{action}-{subaction}", App);
			var router = new Router();
			router.AddRoute(route);
			var result = router.Resolve("GET", "/foo/test-bar");
			Assert.Equal(route, result.Route);
		}

		[Fact]
		public void Puts_values_of_parameters_into_parameters_dictionary(){
			var route = new Route("GET", "/foo/{action}-{subaction}", App);
			var router = new Router();
			router.AddRoute(route);
			var result = router.Resolve("GET", "/foo/do-this");
			Assert.Equal(
				new Dictionary<string, string> {
					{"action", "do"},
					{"subaction", "this"},
				},
				result.Parameters
			);
		}

		[Fact]
		public void Puts_values_of_parameters_into_parameters_dictionary_if_url_ends_with_static_segment(){
			var route = new Route("GET", "/foo/{action}-{subaction}.html", App);
			var router = new Router();
			router.AddRoute(route);
			var result = router.Resolve("GET", "/foo/do-this.html");
			Assert.Equal(
				new Dictionary<string, string> {
					{"action", "do"},
					{"subaction", "this"},
				},
				result.Parameters
			);
		}
	}
}