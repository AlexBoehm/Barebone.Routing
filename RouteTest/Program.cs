using System;
using System.Text;
using System.Text.RegularExpressions;

namespace RouteTest
{
	class MainClass{
		public static void Main(string[] args){
			Parse("/test/{foo}/{assetId}-{caption}.foo.html");
		}

		private static void Parse(string path){
			var staticSegment = @"(?<StaticSegment>\w+)";
			var parameter = @"\{(?<ParameterName>\w+?)\}";
			var staticPart = @"(?<StaticPart>.+?)";
			var segmentEnd = @"(?<SegmentEnd>.+)";
			var dynamicSegment = "(?<DynamicSegment>" + parameter + "(?:" + staticPart + parameter + ")*" + segmentEnd + ")";
			var segment = "(?<Segment>" + staticSegment + "|" + dynamicSegment + ")";

			var regex = new Regex(segment);
			var groupNames = regex.GetGroupNames();

			var segments = path.Split('/');

			foreach (var item in segments) {
				var match = regex.Match(item);

				for (int i = 0; i < match.Groups.Count; i++) {
					Console.WriteLine(groupNames[i]);

					var group = match.Groups[i];
					foreach (Capture capture in group.Captures) {
						Console.WriteLine(capture.Index +  "\t" + capture.Value);
					}				
				}
			}
		}
	}
}
