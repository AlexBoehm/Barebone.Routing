using System;
using Xunit;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RouteTest
{
	public class RouteTests{
		private Func<IDictionary<string, object>, Task> App = (env) => Task.Factory.StartNew(() => {});

		[Fact]
		public void Sets_Route_Path(){
			var route = new Route("GET", "/foo", App);
			Assert.Equal(
				new[]{"foo"},
				route.Segments.Segments.Select(x => x.Value)
			);
		}
	}
}