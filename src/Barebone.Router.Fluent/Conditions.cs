using System;

namespace Barebone.Routing
{
	public static class RouteRegistrationConditionExtensions {
		public static RouteRegistrationChain Condition(this RouteRegistrationChain route, Func<RouteConditionData, bool> condition){
			route.RouteEntry.AddCondition(new RouteCondition(condition));
			return route;
		}

		public static RouteRegistrationChain Condition(this RouteRegistrationChain route, object data, Func<RouteConditionData, bool> condition){
			route.RouteEntry.AddCondition(new RouteCondition(condition, data));
			return route;
		}
	}
}