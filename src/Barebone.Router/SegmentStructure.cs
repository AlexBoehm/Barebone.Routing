using System;

namespace Barebone.Routing
{
	public class SegmentStructure {
		public const string StaticSegment = "StaticSegment";
		public const string DynamicSegment = "DynamicSegment";
		public const string StaticPart = "StaticPart";
		public const string ParameterName = "ParameterName";
		public const string Parameter = "Parameter";
		public const string SegmentEnd = "SegmentEnd";

		public static readonly string staticSegment = @"(?<"+StaticSegment+">[^{]*)";
		public static readonly string parameter = @"(?<"+Parameter+@">\{(?<ParameterName>\w+?)\})";
		public static readonly string staticPart = @"(?<"+StaticPart+">.+?)";
		public static readonly string segmentEnd = @"(?<"+SegmentEnd+">.+){0,1}";
		public static readonly string dynamicSegment = "(?<"+DynamicSegment+">" + parameter + "(?:" + staticPart + parameter + ")*" + segmentEnd + ")";
		public static readonly string segment = "^(?<Segment>" + staticSegment + "|" + dynamicSegment + ")$";
	}
}