using System;

namespace Barebone.Router
{
	public static class RouteRegistrationConditionExtensions {
		public static RouteRegistration Condition(this RouteRegistration route, Func<RouteConditionData, bool> condition){
			route.RouteEntry.AddCondition(new RouteCondition(condition));
			return route;
		}

		public static RouteRegistration Condition(this RouteRegistration route, object data, Func<RouteConditionData, bool> condition){
			route.RouteEntry.AddCondition(new RouteCondition(condition, data));
			return route;
		}
	}	   
}