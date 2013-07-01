using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Barebone.Router
{
	using OwinEnv = IDictionary<string, object>;
	using AppFunc = Func<IDictionary<string, object>, Task>;
	using HttpMethod = String;

	public class Route{
		public HttpMethod Method { get; set; }
		public AppFunc OwinAction { get; set; }
		string _path;
		Path _pathElements;

		/// <summary>
		/// Dictionary of conditions per parameter which all have to be true for the route to be picked
		/// </summary>
		/// <value>The parameter conditions.</value>
		public Dictionary<string, List<Func<string, bool>>> ParameterConditions { get; set;}

		/// <summary>
		/// List of conditions which all have to be true for the route entry to be picked
		/// </summary>
		/// <value>The conditions.</value>
		public List<RouteCondition> Conditions { get; set; }

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

		/// <summary>
		/// Adds a condition to the route.
		/// </summary>
		public void AddCondition(RouteCondition condition){
			if (Conditions == null) {
				lock (this) {
					if (Conditions == null) {
						Conditions = new List<RouteCondition>();
					}
				}
			}

			Conditions.Add(condition);
		}

		public void AddCondition(string parameterName, Func<string, bool> condition){
			if (ParameterConditions == null) {
				lock (this) {
					if (ParameterConditions == null) {
						ParameterConditions = new Dictionary<string, List<Func<string, bool>>>();
					}
				}
			}

			if (!ParameterConditions.ContainsKey(parameterName)) {
				lock (ParameterConditions) {
					if(!ParameterConditions.ContainsKey(parameterName))
						ParameterConditions.Add(parameterName, new List<Func<string, bool>>());
				}
			}

			ParameterConditions[parameterName].Add(condition);
		}
	}
}
