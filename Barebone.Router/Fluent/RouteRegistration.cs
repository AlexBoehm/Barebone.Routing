using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Barebone.Router
{
	using OwinEnv = IDictionary<string, object>;

	public class RouteRegistration{
		public Route RouteEntry { get; private set;}

		public RouteRegistration(string method, string path){
			RouteEntry = new Route(method, path);
		}

		public void Action(Func<OwinEnv, Task> action){
			RouteEntry.OwinAction = action;
		}
	}
}