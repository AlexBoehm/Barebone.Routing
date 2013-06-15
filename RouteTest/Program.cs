using System;
using System.Text;
using System.Text.RegularExpressions;

namespace RouteTest
{
	class MainClass{
		public static void Main(string[] args){
			Parse("/test/{foo}/{assetId}-hghg{caption}.html");
		}

		private static void Parse(string path){
			var staticSegment = @"(?<StaticSegment>\w+)";
			var parameter = @"\{(?<ParameterName>\w+?)\}";
			var staticPart = @"(?<StaticPart>.+?)";
			var segmentEnd = @"(?<SegmentEnd>.+)";
			var dynamicSegment = "(?<DynamicSegment>" + parameter + "(?:" + staticPart + parameter + ")*" + segmentEnd + ")";
			var segment = "/(?<Segment>" + staticSegment + "|" + dynamicSegment + ")";
			var url = "^(" + segment + ")*$";

			var regex = new Regex(url);
			var match = regex.Match(path);

			for (int i = 0; i < match.Groups.Count; i++) {
				Console.WriteLine(regex.GroupNameFromNumber(i));

				var group = match.Groups[i];
				foreach (Capture item in group.Captures) {
					Console.WriteLine(item.Index +  "\t" + item.Value);
				}				
			}
		}
	}
}
