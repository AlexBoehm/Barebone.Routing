using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace Barebone.Routing.Fluent.Tests
{
	public class HighLevel : TestBase{
		[Fact]
		public void Get_to_static_path_is_working(){
			_routes.Get("/test").Action(_action);
			Assert.RoutesTo(FakeRequest.Get("/test"), _action, _routes);
		}

		[Fact]
		public void Post_to_static_path_is_working(){
			_routes.Post("/test").Action(_action);
			Assert.RoutesTo(FakeRequest.Post("/test"), _action, _routes);
		}	
	}
}