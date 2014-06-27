using System;
using System.Collections.Generic;

namespace Barebone.Routing
{
	public static class PathMatcher{
		/// <summary>
		/// Checks if the given path (as pathSegments) matches with the given route
		/// </summary>
		/// <param name="item">route to check agains</param>
		/// <param name="pathSegments">segements of the path to check.</param>
		/// <param name="parameters">parameters and their values found in the path</param>
		public static bool Matches(Route item, string[] pathSegments, out IDictionary<string, RouteValue> parameters){
			parameters = new Dictionary<string, RouteValue>();

			if (item.Segments.Segments.Length != pathSegments.Length)
				return false;

			for (int i = 0; i < pathSegments.Length ; i++) {
				var pathSegment = pathSegments[i];
				var routeSegment = item.Segments.Segments[i];

				if (routeSegment is StaticSegment) {
					if (!(routeSegment as StaticSegment).Value.Equals(pathSegment))
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
			IDictionary<string, RouteValue> parameters
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
						var parameterValue = new RouteValue(pathSegment);
						parameters.Add(part.ParameterName, parameterValue);
					}
					else if (!isLastPart) {
						var nextPart = parts[j + 1];
						if (nextPart.IsParameter)
							throw new Exception("Two dynamic parts within the same segment have to be seperated by a static part!");

						var indexOfNextStaticPart = pathSegment.IndexOf(nextPart.Value);
						if (indexOfNextStaticPart < 0)
							return false;

						var parameterValue = new RouteValue(pathSegment.Substring(0, indexOfNextStaticPart));
						parameters.Add(part.ParameterName, parameterValue);

						pathSegment = pathSegment.Substring(indexOfNextStaticPart, pathSegment.Length - indexOfNextStaticPart);
					}
					else {
						var parameterValue = pathSegment;
						parameters.Add(lastPart.ParameterName, new RouteValue(parameterValue));
					}
				}
			}

			return true;
		}
	}
}