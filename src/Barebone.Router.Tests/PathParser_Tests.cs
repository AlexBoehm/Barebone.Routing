using System;
using Xunit;
using System.Linq;

namespace Barebone.Routing
{
	public class PathParser_Tests{
		[Fact]
		public void Recognizes_one_static(){
			var result = Parse("/info");
			Assert.True(result.Segments[0] is StaticSegment);
		}

		[Fact]
		public void Recognizes_static_segments(){
			var result = Parse("/info/about-us");
			Assert.True(result.Segments[0] is StaticSegment);
			Assert.True(result.Segments[1] is StaticSegment);
		}

		[Fact]
		public void Sets_values_of_static_segments_correctly(){
			var result = Parse("/info/about-us");
			Assert.Equal("info", result.Segments[0].Value);
			Assert.Equal("about-us", result.Segments[1].Value);
		}

		[Fact]
		public void Can_use_minus_in_static_segments(){
			var result = Parse("/info/about-us");
			Assert.Equal("about-us", result.Segments[1].Value);
		}

		[Fact]
		public void Sets_value_of_dynamic_segments(){
			var result = Parse("/products/{id}");
			Assert.Equal("{id}", ((DynamicSegment)result.Segments[1]).Value);
		}

		[Fact]
		public void Recognizes_parameter_in_segment_only_consists_of_one_parameter(){
			var result = Parse("/products/{id}");
			Assert.True(((DynamicSegment)result.Segments[1]).Parts[0].IsParameter);
		}

		[Fact]
		public void Elements_are_in_correct_order(){
			var result = Parse("/products/{title}-{id}-{foo}");
			var parts = ((DynamicSegment)result.Segments[1]).Parts;
			Assert.Equal(new[]{"{title}", "-", "{id}", "-", "{foo}"}, parts.Select(x => x.Value).ToArray());
		}

		[Fact]
		public void Sets_parameter_names_correctly(){
			var result = Parse("/products/{title}-{id}-{foo}");
			var parts = ((DynamicSegment)result.Segments[1]).Parts;
			Assert.Equal(new[]{"title", "id", "foo"}, from p in parts where p.IsParameter select p.ParameterName);
		}

		[Fact]
		public void End_is_added_as_static_element(){
			var result = Parse("/products/{title}-{id}-{foo}.test.html");
			var parts = ((DynamicSegment)result.Segments[1]).Parts;
			Assert.False(parts[5].IsParameter);
		}

		[Fact]
		public void End_is_added_as_static_element_if_end_is_html(){
			var result = Parse("/foo/{action}-{subaction}.html");
			var parts = ((DynamicSegment)result.Segments[1]).Parts;
			Assert.False(parts[3].IsParameter);
		}

		[Fact]
		public void Value_of_ending_is_set_correctly(){
			var result = Parse("/products/{title}-{id}-{foo}.test.html");
			var parts = ((DynamicSegment)result.Segments[1]).Parts;
			Assert.Equal(".test.html", parts[5].Value);
		}

		[Fact]
		public void Parameter_name_of_static_parts_is_null(){
			var result = Parse("/products/{title}-{id}-{foo}.test.html");
			var parts = ((DynamicSegment)result.Segments[1]).Parts;
			Assert.Null(parts[5].ParameterName);
		}

        [Fact]
        public void A_dynamic_segment_can_start_with_a_static_part()
        {
            var result = Parse("/products/one-two-{title}-{id}-{foo}.test.html");
            var parts = ((DynamicSegment)result.Segments[1]).Parts;
            Assert.False(parts[0].IsParameter);
        }

		[Fact]
		public void Slash_at_end_generates_a_new_segment(){
			var result = Parse("/products/");
			Assert.True(result.Segments[1] is StaticSegment);
			Assert.Equal(string.Empty, ((StaticSegment)result.Segments[1]).Value);
		}

		private Path Parse(string path){
			return PathParser.Parse(path);
		}
	}
}
