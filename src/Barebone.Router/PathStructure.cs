using System;

namespace Barebone.Routing
{
	public class Path{
		public Segment[] Segments {get; set;}
	}

	/// <summary>
	/// A path is divided into segments by /
	/// </summary>
	public abstract class Segment {
		public string Value { get; set; }
	}

	/// <summary>
	/// A static segment is a segment without parameters
	/// </summary>
	public class StaticSegment : Segment {
		public override string ToString(){
			return "static: " + Value;
		}
	}

	/// <summary>
	/// A dynamic segment is a segment with parameters
	/// </summary>
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

