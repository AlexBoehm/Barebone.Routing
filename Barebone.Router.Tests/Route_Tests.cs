using System;
using Xunit;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Barebone.Routing
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

		[Fact]
		public void Data_Dictionary_is_not_null(){
			var route = new Route("GET", "/foo", App);
			Assert.NotNull(route);
		}

		[Fact]
		public void Route_can_be_created_with_an_id(){
			var route = new Route("the-route-id", "GET", "/foo", App);
			Assert.Equal("the-route-id", route.Id);
		}

		[Fact]
		public void Id_is_null_if_route_id_is_not_defined(){
			var route = new Route("GET", "/foo", App);
			Assert.Null(route.Id);
		}
	}
}