using System;
using System.Collections.Generic;

namespace RouteTest
{
	public class RouteMatcher{
		public static bool Matches(Route item, string[] pathSegments, out IDictionary<string, string> parameters){
			parameters = new Dictionary<string, string>();

			if (item.Segments.Segments.Length != pathSegments.Length)
				return false;

			for (int i = 0; i < pathSegments.Length ; i++) {
				var pathSegment = pathSegments[i];
				var routeSegment = item.Segments.Segments[i];

				if (routeSegment is StaticSegment) {
					if ((routeSegment as StaticSegment).Value.Equals(routeSegment))
						return false;
				} else {
					if (!CheckDynamicSegment(routeSegment as DynamicSegment, pathSegment, parameters))
						return false;
				}
			}

			return true;
		}

		static bool CheckDynamicSegment(
			DynamicSegment dynamicSegment,
			string pathSegment,
			IDictionary<string, string> parameters
		){
			var parts = dynamicSegment.Parts;
			var lastPart = parts[parts.Length - 1];

			if (!lastPart.IsParameter) {
				if (!pathSegment.EndsWith(lastPart.Value))
					return false;

				pathSegment = pathSegment.Substring(0, pathSegment.Length - lastPart.Value.Length);
			}

			for (int j = 0; j < parts.Length; j++) {
				var isLastPart = j == parts.Length -1;
				if (!lastPart.IsParameter && isLastPart) {
					continue;
				}

				var part = parts[j];
				if (!part.IsParameter) {
					pathSegment = pathSegment.Substring(part.Value.Length, pathSegment.Length - part.Value.Length);
				}
				else {
					var isLastParameterFollowedByStaticPart = part.IsParameter && j == parts.Length - 2 && !lastPart.IsParameter;

					if (isLastParameterFollowedByStaticPart) {
						var parameterValue = pathSegment;
						parameters.Add(part.ParameterName, parameterValue);
					}
					else if (!isLastPart) {
						var nextPart = parts[j + 1];
						if (nextPart.IsParameter)
							throw new Exception("Two dynamic parts within the same segment have to be seperated by a static part!");

						var indexOfNextStaticPart = pathSegment.IndexOf(nextPart.Value);
						if (indexOfNextStaticPart < 0)
							return false;

						var parameterValue = pathSegment.Substring(0, indexOfNextStaticPart);
						parameters.Add(part.ParameterName, parameterValue);

						pathSegment = pathSegment.Substring(indexOfNextStaticPart, pathSegment.Length - indexOfNextStaticPart);
					}
					else {
						var parameterValue = pathSegment;
						parameters.Add(lastPart.ParameterName, parameterValue);
					}
				}
			}

			return true;
		}
	}
}