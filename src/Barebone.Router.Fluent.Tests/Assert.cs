using System;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Barebone.Routing.Fluent.Tests
{
	using AppFunc = Func<IDictionary<string, object>, Task>;
	using OwinEnv = IDictionary<string, object>;

	public class Assert : Xunit.Assert {
		public static void RoutesTo(OwinEnv owinEnv, AppFunc action, Routes routes){
			var router = new Router();
			router.AddRoutes(routes);
			var result = router.Resolve(owinEnv);
			Assert.True(result.Success);
			Assert.True(action.Equals(result.Route.OwinAction));
		}

		public static void DoesNotRouteTo(OwinEnv owinEnv, AppFunc action, Routes routes){
			var router = new Router();
			router.AddRoutes(routes);
			var result = router.Resolve(owinEnv);
			Assert.False(result.Success);
		}
	}
}
