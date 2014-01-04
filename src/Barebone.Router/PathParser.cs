using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Barebone.Routing
{
	public static class PathParser{
		public static Path Parse(string path){
			var regex = GetRegex();
			var groupNames = regex.GetGroupNames();

			var parts = path.Substring(1, path.Length-1).Split('/');

			var segments = new List<Segment>();

			foreach (var item in parts) {
				var match = regex.Match(item);
				var groups = match.Groups;

				if (match.Groups[SegmentStructure.StaticSegment].Success) {
					segments.Add(new StaticSegment(){
						Value = groups[SegmentStructure.StaticSegment].Value
					});
				} else if (match.Groups[SegmentStructure.DynamicSegment].Success) {
					var segment = new DynamicSegment();
					segment.Value = groups[SegmentStructure.DynamicSegment].Value;

					var dynamicSegmentGroup = match.Groups[SegmentStructure.DynamicSegment];

					var elements = new List<Element>();

					for (int i = 1; i < match.Groups.Count; i++) {

						//Console.WriteLine(groupNames[i]);

						var group = match.Groups[i];
						foreach (Capture capture in group.Captures) {
							elements.Add(new Element(){
								GroupName = groupNames[i],
								Index = capture.Index,
								Value = capture.Value
							});

							//Console.WriteLine(capture.Index +  "\t" + capture.Value);
						}
					}

					var segmentParts = new List<Part>();

					foreach (var element in elements.OrderBy(x => x.Index)) {
						switch (element.GroupName) {
						case SegmentStructure.SegmentEnd:
						case SegmentStructure.StaticPart:
								segmentParts.Add(
									new Part{
										Value = element.Value,
										IsParameter = false,
										ParameterName = null
									});
								break;
						case SegmentStructure.Parameter:
								segmentParts.Add(
									new Part{
									Value = element.Value,
									IsParameter = true,
									ParameterName = element.Value.Substring(1, element.Value.Length-2)
								});
								break;
							default:
								break;
						}
					}

					segment.Parts = segmentParts.ToArray();
					segments.Add(segment);
				} else {
					throw new Exception("Unkown segment type");
				}
			}

			return new Path { Segments = segments.ToArray() };
		}

		private static Regex GetRegex(){
			return new Regex(SegmentStructure.segment);
		}

		private class Element {
			public string Value { get; set; }
			public string GroupName { get; set; }
			public int Index { get; set; }
		}
	}
}