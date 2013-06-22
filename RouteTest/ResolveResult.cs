using System;

namespace RouteTest
{
	public class ResolveResult{
		public Route Route { get; private set; }
		public bool Success { get; private set; }
		//public IDictionary<string,string> Parameters { get; set; }

		private ResolveResult(){}

		public static ResolveResult RouteFound(Route route){
			return new ResolveResult(){
				Route = route,
				Success = true
			};
		}

//		public static ResolveResult NoResult(){
//			return new ResolveResult() {
//				Success = false
//			};
//		}
	}
}

