using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Barebone.Routing;

namespace Barebone.Routing.Fluent.Tests
{
	using OwinEnv = IDictionary<string, object>;

	public class ReadmeExample{
		public static void Sample() {
			Routes routes = new Routes();

			routes
				.Get("/foo")
				.Condition("en", x => x.ConditionData.Equals("en"))
				.Action(InfoAppFunc);

			var router = new Router();
			router.AddRoutes(routes);
		}

		private static Task InfoAppFunc(OwinEnv env){
			return Task.Factory.StartNew(() => {
				// show info page
			});
		}
	}
}