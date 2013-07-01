using System;
using System.Collections.Generic;

namespace RouteTest
{
	public class ResolveResult{
		public Route Route { get; private set; }
		public bool Success { get; private set; }
		public IDictionary<string,string> Parameters { get; private set; }

		private ResolveResult(){}

		public static ResolveResult RouteFound(Route route, IDictionary<string, string> parameters){
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

