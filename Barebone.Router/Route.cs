using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Barebone.Router
{
	using OwinEnv = IDictionary<string, object>;
	using AppFunc = Func<IDictionary<string, object>, Task>;
	using HttpMethod = String;

	public class Route{
		/// <summary>
		/// HTTP Method
		/// </summary>
		public HttpMethod Method { get; set; }

		/// <summary>
		/// Owin-Action to Execute
		/// </summary>
		/// <value>The owin action.</value>
		public AppFunc OwinAction { get; set; }

		/// <summary>
		/// Gets or sets the data stored with the route.
		/// </summary>
		/// <value>The data.</value>
		public Dictionary<string, object> Data { get; set; }

		/// <summary>
		/// Priority of this route Entry. The priority is used determine which route has to be executed if the request matches more than one route.
		/// </summary>
		/// <value>The priority.</value>
		public int Priority { get; set; }

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
			Data = new Dictionary<string, object>();
		}

		string _path;
		Path _pathElements;

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


		//ToDo: Should this method throw an exception, if the parameter is not defined in the route?
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
