using System;
using Xunit;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RouteTest
{
	public class RouteTableTests{
		private Func<IDictionary<string, object>, Task> App = (env) => Task.Factory.StartNew(() => {});

		[Fact]
		public void Returns_Candidates(){
			var table = new RouteTree();
			var route = new Route("GET", "/foo/bar", App);
			table.Add(route);
			var candidates = table.GetCandidates("/foo/bar");
			Assert.Equal(new List<Route>{route}, candidates);
		}
	}
}