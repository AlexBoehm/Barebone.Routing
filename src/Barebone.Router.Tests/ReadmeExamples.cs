using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Barebone.Routing.Tests
{
	using OwinEnv = IDictionary<string, object>;

	public class SimpleExample{
		public Task ProcessRequest(OwinEnv env){
			var router = new Router();
			var route = new Route("GET", "/products/{id}-{name}.html", InfoAppFunc);
			router.AddRoute(route);

			var result = router.Resolve(env);

			if (result.Success) {
				var id = result.Parameters["id"];
				var name = result.Parameters["name"];
				return result.Route.OwinAction(env); //Call the AppFunc
			} else {
				// Do something else
				throw new Exception();
			}
		}

		public void ParametersInRoutes(){
			var route = new Route("GET", "/foo/{action}", App);
			var router = new Router();
			router.AddRoute(route);
			var result = router.Resolve(FakeRequest.Get("/foo/test"));
		}

		public void StoringArbitraryData(OwinEnv owinEnironment){
			var route = new Route("GET", "/customers/show/{id}", App);
			route.Data["CacheOptions"] = new CacheOptions(/* options */);

			var router = new Router();
			router.AddRoute(route);

			var result = router.Resolve(FakeRequest.Get("/customers/show/{id}"));
			var cacheOptions = (CacheOptions)route.Data["CacheOptions"];
			// Add Caching Options to HTTP-Header

			result.Route.OwinAction.Invoke(owinEnironment);
		}

		private Task App (OwinEnv env){
			// Don't do this in real code!
			return Task.Factory.StartNew(() => {
				// show info page
			});
		}

		private Task InfoAppFunc(OwinEnv env){
			// Don't do this in real code!
			return Task.Factory.StartNew(() => {
				// show info page
			});
		}

		private class CacheOptions{
		}
	}
}