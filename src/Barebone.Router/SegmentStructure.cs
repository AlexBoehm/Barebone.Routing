using System;

namespace Barebone.Routing
{
	public class SegmentStructure {
		public static readonly string staticSegmentRegex = @"(?<"+GroupNames.StaticSegment+">[^{]*)";
		public static readonly string parameterRegex = @"(?<"+GroupNames.Parameter+@">\{(?<ParameterName>\w+?)\})"; //Parameter wie {productId}
        public static readonly string optionalParameterRegex = parameterRegex + "?"; //Parameter wie {productId}
        public static readonly string staticPartRegex = @"(?<" + GroupNames.StaticPart + ">.+?)";
        public static readonly string segmentEndRegex = @"(?<" + GroupNames.SegmentEnd + ">.+){0,1}";
        public static readonly string dynamicSegmentRegex = "(?<" + GroupNames.DynamicSegment + ">" + optionalParameterRegex + "(?:" + staticPartRegex + parameterRegex + ")*" + segmentEndRegex + ")";
		public static readonly string segmentRegex = "^(?<Segment>" + staticSegmentRegex + "|" + dynamicSegmentRegex + ")$";

        public static class GroupNames
        {
            public const string StaticSegment = "StaticSegment";
            public const string DynamicSegment = "DynamicSegment";
            public const string StaticPart = "StaticPart";
            public const string ParameterName = "ParameterName";
            public const string Parameter = "Parameter";
            public const string SegmentEnd = "SegmentEnd";
        }
	}
}