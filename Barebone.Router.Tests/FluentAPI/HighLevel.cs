using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace Barebone.Router.Tests
{	

	public class HighLevel : TestBase{
		[Fact]
		public void Get_to_static_path_is_working(){
			_routes.Get("/test").Action(_action);
			Assert.RoutesTo("/test", _action, _routes);
		}
	
	}
}