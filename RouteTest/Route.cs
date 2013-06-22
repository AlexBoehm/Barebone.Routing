using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RouteTest
{
	using OwinEnv = IDictionary<string, object>;
	using AppFunc = Func<IDictionary<string, object>, Task>;
	using HttpMethod = String;

	public class Route{
		public HttpMethod Method { get; set; }
		public AppFunc OwinAction { get; set; }
		string _path;
		Path _pathElements;

		public Route(HttpMethod method, string path, AppFunc owinAction){
			Method = method;
			Path = path;
			OwinAction = owinAction;
		}

		public string Path { 
			get { return _path;} 
			set {
				_path = value;
				_pathElements = PathParser.Parse(value);
			}
		}

		public Path Segments { 
			get { return _pathElements; }
		}
	}
}
