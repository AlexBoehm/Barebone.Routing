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
	}
}