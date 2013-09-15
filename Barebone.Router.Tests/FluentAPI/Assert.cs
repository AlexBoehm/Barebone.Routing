using System;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Barebone.Routing.Tests
{
	using AppFunc = Func<IDictionary<string, object>, Task>;

	public class Assert : Xunit.Assert {
		public static void RoutesTo(string path, AppFunc action, Routes routes){
			var router = new Router();
			router.AddRoutes(routes);
			var result = router.Resolve(Utils.BuildGetRequest(path));
			Assert.True(result.Success);
			Assert.True(action.Equals(result.Route.OwinAction));
		}

		public static void DoesNotRouteTo(string path, AppFunc action, Routes routes){
			var router = new Router();
			router.AddRoutes(routes);
			var result = router.Resolve(Utils.BuildGetRequest(path));
			Assert.False(result.Success);
		}
	}
}
