using System;
using System.Collections.Generic;
using System.IO;

namespace Barebone.Routing
{
	using OwinEnv = IDictionary<string, object>;

	public static class FakeRequest {
		public static OwinEnv Get(string path){
			var env = Utils.FakeRequest(Methods.GET);
			env["owin.RequestPath"] = path;
			return env;
		}

		public static OwinEnv Post(string path){
			var env = Utils.FakeRequest(Methods.POST);
			env["owin.RequestPath"] = path;
			return env;
		}
	}

	public static class Utils
	{
		public static OwinEnv BuildGetRequest(string path) {
			var env = FakeRequest(Methods.GET);
			env["owin.RequestPath"] = path;
			return env;
		}

		public static OwinEnv FakeRequest(string method) {
			return new Dictionary<string, object>(){
				{ "owin.RequestPath", "/"},
				{ "owin.RequestPathBase", string.Empty},
				{ "owin.RequestMethod", method},
				{ "owin.ResponseBody", new MemoryStream()},
				{ "owin.RequestHeaders", new Dictionary<string, string[]>(){{"HOST", new []{"localhost"}}}},
				{ "owin.RequestProtocol", "HTTP/1.1"},
				{ "owin.RequestQueryString", string.Empty},
				{ "owin.RequestScheme", "http"}
			};
		}
	}
}