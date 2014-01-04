using System;
using System.Collections.Generic;

namespace Barebone.Routing
{
	using OwinEnv = IDictionary<string, object>;
	using RouteParameters = IDictionary<string,string>;

	/// <summary>
	/// Data to pass to route dondition functions
	/// 
	/// The data gets passed to the RouteCondition as an Object instead of multiple parameters
	/// ,such that the signature of the route Condition function does not have to change (which would be a braking change)
	/// if we decide to pass additional data in the future.
	/// </summary>
	public class RouteConditionData{
		/// <summary>
		/// Owin Environment
		/// </summary>
		/// <value>The owin env.</value>
		public OwinEnv OwinEnv {get; private set;}

		/// <summary>
		/// Data captured from the path
		/// </summary>
		/// <value>The route parameters.</value>
		public RouteParameters RouteParameters { get; private set; }

		/// <summary>
		/// Data which is assigned to the route condition, when the route condition is created.
		/// </summary>
		/// <value>The condition data.</value>
		public object ConditionData { get; set; }

		/// <summary>
		/// The Route Entry which matches the path
		/// </summary>
		/// <value>The route.</value>
		public Route Route { get; private set; }

		public RouteConditionData(OwinEnv env, RouteParameters routeParameters, Route route, object conditionData){
			OwinEnv = env;
			RouteParameters = routeParameters;
			Route = route;
			ConditionData = conditionData;
		}
	}
}
