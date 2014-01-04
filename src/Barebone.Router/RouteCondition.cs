using System;

namespace Barebone.Routing
{
	using CheckFunc = Func<RouteConditionData, bool>;

	/// <summary>
	/// Represents a condition which has to be true, for the route to be picked
	/// </summary>
	public class RouteCondition{
		public CheckFunc Condition { get; set;}

		/// <summary>
		/// Data to pass to the check Method
		/// </summary>
		/// <value>The data.</value>
		public object Data { get; set; }

		public RouteCondition(CheckFunc condition) : this(condition, null){
		}

		public RouteCondition(CheckFunc condition, object data){
			if (condition == null) {
				throw new ArgumentException("condition may not be null");
			}

			Condition = condition;
			Data = data;
		}
	}
}
