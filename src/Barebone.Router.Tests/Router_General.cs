using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace Barebone.Routing
{
	using OwinEnv = IDictionary<string, object>;
	//using AppFunc = Func<IDictionary<string, object>, Task>;

	public partial class Router_General : RouterTestBase {
		[Fact]
		public void Get_Static_Urls_with_one_segment_is_matched_correctly(){
			var route = new Route("GET", "/foo", App);
			var router = new Router();
			router.AddRoute(route);	
			var result = router.Resolve(FakeRequest.Get("/foo"));
			Assert.Equal(route, result.Route);
		}

		[Fact]
		public void Get_Static_Urls_with_dynamic_segment_is_matched_correctly(){
			var route = new Route("GET", "/foo/{action}", App);
			var router = new Router();
			router.AddRoute(route);
			var result = router.Resolve(FakeRequest.Get("/foo/test"));
			Assert.Equal(route, result.Route);
		}

		[Fact]
		public void Get_Static_Urls_with_dynamic_segment_and_multiple_parameters_is_matched_correctly(){
			var route = new Route("GET", "/foo/{action}-{subaction}", App);
			var router = new Router();
			router.AddRoute(route);
			var result = router.Resolve(FakeRequest.Get("/foo/test-bar"));
			Assert.Equal(route, result.Route);
		}

		[Fact]
		public void Puts_values_of_parameters_into_parameters_dictionary(){
			var route = new Route("GET", "/foo/{action}-{subaction}", App);
			var router = new Router();
			router.AddRoute(route);
			var result = router.Resolve(FakeRequest.Get("/foo/do-this"));
			Assert.Equal(
				new Dictionary<string, RouteValue> {
					{"action", new RouteValue("do")},
					{"subaction", new RouteValue("this")},
				},
				result.Parameters
			);
		}

		[Fact]
		public void Puts_values_of_parameters_into_parameters_dictionary_if_url_ends_with_static_segment(){
			var route = new Route("GET", "/foo/{action}-{subaction}.html", App);
			var router = new Router();
			router.AddRoute(route);
			var result = router.Resolve(FakeRequest.Get("/foo/do-this.html"));
			Assert.Equal(
				new Dictionary<string, RouteValue> {
					{"action", new RouteValue("do")},
					{"subaction", new RouteValue("this")},
				},
				result.Parameters
			);
		}

		[Fact]
		public void Route_is_not_picted_if_one_parameter_condition_does_return_false(){
			var route = new Route("GET", "/test/{ProductId}/{Title}", App);
			route.AddCondition("ProductId", value => true);
			route.AddCondition("ProductId", value => false);
			Assert.True(DoesNotRouteTo(route, Utils.BuildGetRequest("/test/foo/nice-product")));
		}

		[Fact]
		public void If_no_route_condition_is_defined_for_a_parameter_this_equals_true(){
			var route = new Route("GET", "/test/{ProductId}/{Title}", App);
			Assert.True(RoutesTo(route, _other, "/test/12345/nice-product"));
		}

		[Fact]
		public void Route_is_picted_if_all_parameter_conditions_return_true(){
			var route = new Route("GET", "/test/{ProductId}/{Title}", App);
			route.AddCondition("ProductId", value => true);
			route.AddCondition("ProductId", value => true);
			route.AddCondition("Title", value => true);
			Assert.True(RoutesTo(route, _other, "/test/foo/nice-product"));
		}

		[Fact]
		public void Passes_parameter_values_to_parameter_condition_functions(){
			var route = new Route("GET", "/test/{ProductId}/{Title}", App);

			string receivedProductId = null;
			string receivedTitle = null;

			route.AddCondition("ProductId", value => { receivedProductId = value.Value; return true;});
			route.AddCondition("Title", value => { receivedTitle = value.Value; return true;});
			var router = new Router();
			router.AddRoute(route);
			router.Resolve(Utils.BuildGetRequest("/test/123/foo"));

			Assert.Equal("123", receivedProductId);
			Assert.Equal("foo", receivedTitle);
		}

		[Fact]
		public void All_Parameter_condition_for_same_paramter_get_checked_until_one_returns_false(){
			var route = new Route("GET", "/test/{ProductId}/{Title}", App);

			var c1 = false;
			var c2 = false;
			var c3 = false;

			route.AddCondition("ProductId", value => { c1 = true; return true;});
			route.AddCondition("ProductId", value => { c2 = true; return false;});
			route.AddCondition("ProductId", value => { c3 = true; return true;});

			var router = new Router();
			router.AddRoute(route);
			router.Resolve(Utils.BuildGetRequest("/test/1234/title"));

			Assert.True(c1);
			Assert.True(c2);
			Assert.False(c3);
		}

		[Fact]
		public void Does_not_route_to_action_if_path_is_to_long(){
			var route = new Route("GET", "/test/{ProductId}/{Title}", App);
			Assert.True(DoesNotRouteTo(route, "/test/1234/foo/this is to much"));}

		[Fact]
		public void Does_not_route_if_last_segement_does_not_match(){
			var route = new Route("GET", "/test/{ProductId}/{Title}/", App);
			Assert.True(DoesNotRouteTo(route, "/test/1234/foo/this is to much"));
		}

		[Fact]
		public void Static_segments_have_to_be_equal(){
			var route1 = new Route("GET", "/test/foo", App);
			var route2 = new Route("GET", "/test/bar", App);
			var router = new Router();
			router.AddRoute(route1);
			router.AddRoute(route2);
			var result = router.Resolve(Utils.BuildGetRequest("/test/bar"));
			Assert.Same(route2, result.Route);
		}

		[Fact]
		public void Router_stops_checking_conditions_if_it_found_condition_that_returns_false(){
			var route = new Route("GET", "/test", App);
			var c1 = false;
			var c2 = false;
			var c3 = false;
			var c4 = false;

			route.AddCondition(new RouteCondition(data => { c1 = true; return true;}));
			route.AddCondition(new RouteCondition(data => { c2 = true; return true;}));
			route.AddCondition(new RouteCondition(data => { c3 = true; return false;}));
			route.AddCondition(new RouteCondition(data => { c4 = true; return true;}));

			var router = new Router();
			router.AddRoute(route);
			router.Resolve(Utils.BuildGetRequest("/test"));

			Assert.True(c1 && c2 && c3 && !c4);
		}

		[Fact]
		public void Route_data_is_not_null_if_there_no_parameters_have_been_defined(){
			var route = new Route("GET", @"/test", App);
			var router = new Router();
			router.AddRoute(route);
			var received = router.Resolve(Utils.BuildGetRequest("/test"));
			Assert.NotNull(received.Parameters);
		}

        [Fact]
        public void Does_not_pick_route_when_method_is_not_matching()
        {
            var route = new Route("POST", "/test", App);
            var router = new Router();
            router.AddRoute(route);

            var received = router.Resolve(Utils.BuildGetRequest("/test"));

            Assert.False(received.Success);            
        }

        [Fact]
        public void Picks_route_when_method_is_not_matching()
        {
            var route = new Route("POST", "/test", App);
            var router = new Router();
            router.AddRoute(route);

            var received = router.Resolve(Utils.BuildPostRequest("/test"));

            Assert.True(received.Success);
            Assert.Same(route, received.Route);
        }

        [Fact]
        public void Routes_with_dash_are_working()
        {
            var route = new Route("GET", "/api/foo-bar/get-details");
            var router = new Router();
            router.AddRoute(route);

            var received = router.Resolve(Utils.BuildGetRequest("/api/foo-bar/get-details"));
            Assert.True(received.Success);
            Assert.Same(route, received.Route);
        }

        [Fact]
        public void Routes_with_similar_segments_are_working_01()
        {
            var router = new Router();

            var route1 = new Route("GET", "/SiteMaps/PreviewPages.xml");
            var route2 = new Route("GET", "/SiteMaps/PreviewPages_{page}.xml");            
            route2.AddCondition(new RouteCondition(data => {
                int isInteger;
                return int.TryParse(data.RouteParameters["page"].Value, out isInteger);
            }));

            router.AddRoute(route1);
            router.AddRoute(route2);

            var received = router.Resolve(Utils.BuildGetRequest("/SiteMaps/PreviewPages.xml"));
            Assert.Same(route1, received.Route);
        }

        [Fact]
        public void Routes_with_similar_segments_are_working_02()
        {
            var router = new Router();

            var route1 = new Route("GET", "/SiteMaps/PreviewPages.xml");
            var route2 = new Route("GET", "/SiteMaps/PreviewPages_{page}.xml");
            route2.AddCondition(new RouteCondition(data =>
            {
                int isInteger;
                return int.TryParse(data.RouteParameters["page"].Value, out isInteger);
            }));

            router.AddRoute(route1);
            router.AddRoute(route2);

            var received = router.Resolve(Utils.BuildGetRequest("/SiteMaps/PreviewPages_2.xml"));
            Assert.Same(route2, received.Route);
        }
	}
}