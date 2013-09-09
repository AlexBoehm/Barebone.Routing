using System;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Barebone.Router.Tests
{
	using AppFunc = Func<IDictionary<string, object>, Task>;

	public class Assert : Xunit.Assert {
		public static void RoutesTo(string path, AppFunc action, Routes routes){
			var router = new Router();
			router.AddRoutes(routes);
			var received = router.Resolve(Utils.BuildGetRequest(path));
			Assert.True(action.Equals(received.Route.OwinAction));
		}
	}
}
