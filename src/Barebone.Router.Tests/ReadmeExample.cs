using System;
using Xunit;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;

namespace Barebone.Routing.Tests
{
	using OwinEnv = IDictionary<string, object>;

	public class SimpleExample{
		public Task ProcessRequest(OwinEnv env){
			var router = new Router();
			var route = new Route("GET", "/info", InfoAppFunc);
			router.AddRoute(route);

			var result = router.Resolve(env);

			if (result.Success) {
				return result.Route.OwinAction(env);
			} else {
				// Do something else
				throw new Exception();
			}
		}

		private Task InfoAppFunc(OwinEnv env){
			return Task.Factory.StartNew(() => {
				// show info page
			});
		}
	}
}