using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Barebone.Routing
{
	public class RouteTreeTestBase{
		protected Func<IDictionary<string, object>, Task> App = (env) => Task.Factory.StartNew(() => {});
	}
}