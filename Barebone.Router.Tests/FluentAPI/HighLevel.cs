using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace Barebone.Router.Tests
{
	using OwinEnv = IDictionary<string, object>;
	using AppFunc = Func<IDictionary<string, object>, Task>;
	//using Assert = Barebone.Router.Tests.Assert;

	public class HighLevel{
		Func<OwinEnv, Task> _action = env => Task.Factory.StartNew(() => {});

		Routes _routes = new Routes();

		[Fact]
		public void Get_to_static_path_is_working(){
			_routes.Get("/test").Action(_action);
			Assert.RoutesTo("/test", _action, _routes);
		}
	}
}