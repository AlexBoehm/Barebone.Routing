using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Barebone.Routing.Tests.FluentAPI
{
	using OwinEnv = IDictionary<string, object>;
	using AppFunc = Func<IDictionary<string, object>, Task>;

	public class TestBase{
		protected Func<OwinEnv, Task> _action = env => Task.Factory.StartNew(() => {});
		protected Routes _routes = new Routes();
	}
}