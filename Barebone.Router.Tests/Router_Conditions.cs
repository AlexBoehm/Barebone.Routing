using System;
using Xunit;

namespace Barebone.Routing
{
	public class Router_Conditions{
		Func<RouteConditionData, bool> checkFunc = data => true;

		[Fact]
		public void Condition_is_set(){
			var condition = new RouteCondition(checkFunc);
			Assert.Equal(checkFunc, condition.Condition);
		}

		[Fact]
		public void Data_is_null_if_constuctor_without_data_is_used(){
			var condition = new RouteCondition(checkFunc);
			Assert.Null(condition.Data);
		}

		[Fact]
		public void Throws_exception_if_condition_is_null(){
			Assert.Throws<ArgumentException>(() => { new RouteCondition(null);});
		}

		[Fact]
		public void Condition_is_set_when_constructing_with_data(){
			var condition = new RouteCondition(checkFunc, "data");
			Assert.Equal(checkFunc, condition.Condition);
		}

		[Fact]
		public void Data_is_set_when_constructing_with_data(){
			var data = "data";
			var condition = new RouteCondition(checkFunc, data);
			Assert.Same(data, condition.Data);
		}
	}
}
