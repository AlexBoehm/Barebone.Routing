using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Barebone.Routing
{
	public class PathParser{
		const string StaticSegment = "StaticSegment";
		const string DynamicSegment = "DynamicSegment";
		const string StaticPart = "StaticPart";
		const string ParameterName = "ParameterName";
		const string Parameter = "Parameter";
		const string SegmentEnd = "SegmentEnd";
		

		public static Path Parse(string path){
			var regex = GetRegex();
			var groupNames = regex.GetGroupNames();

			var parts = path.Substring(1, path.Length-1).Split('/');

			var segments = new List<Segment>();

			foreach (var item in parts) {
				var match = regex.Match(item);
				var groups = match.Groups;

				if (match.Groups[StaticSegment].Success) {
					segments.Add(new StaticSegment(){
						Value = groups[StaticSegment].Value
					});
				} else if (match.Groups[DynamicSegment].Success) {
					var segment = new DynamicSegment();
					segment.Value = groups[DynamicSegment].Value;

					var dynamicSegmentGroup = match.Groups[DynamicSegment];

					var elements = new List<Element>();

					for (int i = 1; i < match.Groups.Count; i++) {
//						if(
//
						Console.WriteLine(groupNames[i]);

						var group = match.Groups[i];
						foreach (Capture capture in group.Captures) {
							elements.Add(new Element(){
								GroupName = groupNames[i],
								Index = capture.Index,
								Value = capture.Value
							});

							Console.WriteLine(capture.Index +  "\t" + capture.Value);
						}
					}

					//segments.OrderBy(x => x.Position);

					var segmentParts = new List<Part>();

					foreach (var element in elements.OrderBy(x => x.Index)) {
						switch (element.GroupName) {
							case SegmentEnd:
							case StaticPart:
								segmentParts.Add(
									new Part{
										Value = element.Value,
										IsParameter = false,
										ParameterName = null
									});
								break;
						case Parameter:
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
			var staticSegment = @"(?<"+StaticSegment+">[^{]*)";
			var parameter = @"(?<"+Parameter+@">\{(?<ParameterName>\w+?)\})";
			var staticPart = @"(?<"+StaticPart+">.+?)";
			var segmentEnd = @"(?<"+SegmentEnd+">.+){0,1}";
			var dynamicSegment = "(?<"+DynamicSegment+">" + parameter + "(?:" + staticPart + parameter + ")*" + segmentEnd + ")";
			var segment = "^(?<Segment>" + staticSegment + "|" + dynamicSegment + ")$";

			return new Regex(segment);
		}

		private class Element {
			public string Value { get; set; }
			public string GroupName { get; set; }
			public int Index { get; set; }
		}
	}

	public class Path{
		public Segment[] Segments {get; set;}
	}

	public abstract class Segment {
		public string Value { get; set; }
	}

	public class StaticSegment : Segment {
		public override string ToString(){
			return "static: " + Value;
		}
	}

	public class DynamicSegment : Segment {
		public Part[] Parts {get; set;}

		public override string ToString(){
			return "dynamic: " + Value;
		}
	}

	public class Part {
		/// <summary>
		/// string from the route which was parsed to this part
		/// </summary>
		/// <value>The value.</value>
		public string Value { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether this instance is a parameter.
		/// </summary>
		/// <value><c>true</c> if this instance is parameter; otherwise, <c>false</c>.</value>
		public bool IsParameter { get; set; }

		/// <summary>
		/// Name of the parameter if this part of the segment is a parameter
		/// </summary>
		/// <value>The name of the parameter.</value>
		public string ParameterName { get; set; }

		public override string ToString(){
			return Value;
		}
	}
}