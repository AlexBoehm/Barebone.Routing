using System;
using Xunit;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Barebone.Router
{
	public class Route_Tests{
		private Func<IDictionary<string, object>, Task> App = (env) => Task.Factory.StartNew(() => {});

		[Fact]
		public void Sets_Route_Path(){
			var route = new Route("GET", "/foo", App);
			Assert.Equal(
				new[]{"foo"},
				route.Segments.Segments.Select(x => x.Value)
			);
		}

		[Fact(Skip="Not implemented yet")]
		public void Exception_is_thrown_when_paramter_is_defined_multiple_times(){
			throw new NotImplementedException();
		}
	}
}