using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Barebone.Routing
{
	using OwinEnv = IDictionary<string, object>;

	public abstract class RouterTestBase {
		protected static readonly Func<IDictionary<string, object>, Task> App = (env) => Task.Factory.StartNew(() => {});

		protected Route[] _other = new Route[]{
			new Route("GET", "/bar", App),
			new Route("GET", "/boo", App)
		};

		protected bool RoutesTo(
			Route correctEntry,
			IEnumerable<Route> otherEntries,
			string url
			){
			return RoutesTo (correctEntry, otherEntries, Utils.BuildGetRequest (url));
		}

		protected bool RoutesTo(
			Route correctEntry,
			IEnumerable<Route> otherEntries,
			OwinEnv request
			){
			Router router = new Router ();
			router.AddRoute (correctEntry);
			var entry = router.Resolve (request);
			return entry.Route == correctEntry;
		}

		protected bool DoesNotRouteTo(
			Route Route,
			string url
			){
			return DoesNotRouteTo(Route, Utils.BuildGetRequest(url));
		}

		protected bool DoesNotRouteTo(
			Route correctEntry,
			OwinEnv request
			){
			Router router = new Router ();
			router.AddRoute (correctEntry);
			var entry = router.Resolve (request);

			if (entry == null)
				return true;

			return entry.Route != correctEntry;
		}
	}
}

