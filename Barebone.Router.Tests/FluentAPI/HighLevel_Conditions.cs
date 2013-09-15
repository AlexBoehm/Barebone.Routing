using System;
using Xunit;

namespace Barebone.Routing.Tests
{
	public class HighLevel_Conditions : TestBase{
		[Fact]
		public void Route_with_condition_data_is_working(){
			_routes
				.Get(@"/test")
					.Condition("en", x => x.ConditionData.Equals("en"))
					.Action(_action);

			Assert.RoutesTo("/test", _action, _routes);
		}

		[Fact]
		public void Does_not_route_to_action_if_condition_is_false(){
			_routes.Get("/test")
				.Condition("en", x => x.ConditionData.Equals("de"))
				.Action(_action);

			Assert.DoesNotRouteTo("/test", _action, _routes);
		}
	}
}
