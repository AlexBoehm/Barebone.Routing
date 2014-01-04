using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Barebone.Routing
{
	using OwinEnv = IDictionary<string, object>;

	public class RouteRegistrationChain{
		public Route RouteEntry { get; private set;}

		public RouteRegistrationChain(string method, string path){
			RouteEntry = new Route(method, path);
		}

		public void Action(Func<OwinEnv, Task> action){
			RouteEntry.OwinAction = action;
		}
	}

	public class RouteRegistrationChain<THandler> : RouteRegistrationChain{
		public RouteRegistrationChain(string method, string path):base(method, path){ }
	}
}