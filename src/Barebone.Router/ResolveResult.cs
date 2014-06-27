using System;
using System.Collections.Generic;

namespace Barebone.Routing
{
	public class ResolveResult{
		public Route Route { get; private set; }
		public bool Success { get; private set; }
		public IDictionary<string,RouteValue> Parameters { get; private set; }

		private ResolveResult(){}

		public static ResolveResult RouteFound(Route route, IDictionary<string, RouteValue> parameters){
			return new ResolveResult(){
				Route = route,
				Success = true,
				Parameters = parameters
			};
		}

		public static ResolveResult NoResult(){
			return new ResolveResult() {
				Success = false
			};
		}
	}
}
