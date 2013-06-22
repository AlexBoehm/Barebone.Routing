using System;

namespace RouteTest
{
	public class RouteMatcher{
		public static bool Matches(Route item, string[] pathSegments){
			if (item.Segments.Segments.Length != pathSegments.Length)
				return false;

			for (int i = 0; i < pathSegments.Length ; i++) {
				var pathSegment = pathSegments[i];
				var routeSegment = item.Segments.Segments[i];

				if (routeSegment is StaticSegment) {
					if ((routeSegment as StaticSegment).Value.Equals(routeSegment))
						return false;
				} else {
					if (!CheckDynamicSegment(routeSegment as DynamicSegment, pathSegment))
						return false;
				}
			}

			return true;
		}

		static bool CheckDynamicSegment(DynamicSegment dynamicSegment, string pathSegment){
			var parts = dynamicSegment.Parts;
			var lastPart = parts[parts.Length - 1];

			if (!lastPart.IsParameter) {
				pathSegment = pathSegment.Substring(0, pathSegment.Length - lastPart.Value.Length);
				if (!pathSegment.EndsWith(lastPart.Value))
					return false;
			}

			for (int j = 0; j < parts.Length; j++) {
				if (lastPart.IsParameter && j == parts.Length - 1)
					continue;

				var part = parts[j];
				if (part.IsParameter) {
					pathSegment = pathSegment.Substring(part.Value.Length, pathSegment.Length - part.Value.Length);
				}
				else {
					var isLastPart = j + 1 == parts.Length;
					if (!isLastPart) {
						var nextPart = parts[j + 1];
						if (nextPart.IsParameter)
							throw new Exception("Two dynamic parts of to be seperated by a static part!");
						var indexOfNextStaticPart = pathSegment.IndexOf(nextPart.Value);
						if (indexOfNextStaticPart < 0)
							return false;

						var parameterValue = pathSegment.Substring(0, indexOfNextStaticPart);
						pathSegment = pathSegment.Substring(indexOfNextStaticPart, pathSegment.Length - indexOfNextStaticPart);
					}
					else {
						var parameterValue = pathSegment;
					}
				}
			}

			return true;
		}
	}
}