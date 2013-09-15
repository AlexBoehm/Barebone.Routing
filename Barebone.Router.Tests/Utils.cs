using System;
using System.Collections.Generic;
using System.IO;

namespace Barebone.Routing
{
	using OwinEnv = IDictionary<string, object>;

	public static class Utils
	{
		public static OwinEnv BuildGetRequest(string path) {
			var env = FakeRequest();
			env["owin.RequestPath"] = path;
			return env;
		}

		public static OwinEnv FakeRequest() {
			return new Dictionary<string, object>(){
				{ "owin.RequestPath", "/"},
				{ "owin.RequestPathBase", string.Empty},
				{ "owin.RequestMethod", "GET"},
				{ "owin.ResponseBody", new MemoryStream()},
				{ "owin.RequestHeaders", new Dictionary<string, string[]>(){{"HOST", new []{"localhost"}}}},
				{ "owin.RequestProtocol", "HTTP/1.1"},
				{ "owin.RequestQueryString", string.Empty},
				{ "owin.RequestScheme", "http"}
			};
		}
	}
}