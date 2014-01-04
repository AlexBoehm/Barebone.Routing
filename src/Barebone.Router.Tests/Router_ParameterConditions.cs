using System;
using Xunit;
using System.Collections.Generic;

namespace Barebone.Routing
{
	using OwinEnv = IDictionary<string, object>;

	public class Router_ParameterConditions : RouterTestBase {
		[Fact]
		public void If_no_general_route_condition_is_defined_this_equals_true(){
			var route = new Route("GET", "/test", App);
			Assert.True(RoutesTo(route, _other, "/test"));
		}

		[Fact]
		public void Does__otherturn_Route_if_condition_is_false(){
			var url = "/test";

			var Route = new Route("GET", url, App);
			Route.AddCondition(new RouteCondition(data => data.OwinEnv.ContainsKey ("KeyNeedsToExist")));

			Assert.True(DoesNotRouteTo(Route, url));
		}

		[Fact]
		public void Route_parameters_passed_to_condition_function_are_not_null_if_route_contains_no_parameters(){
			var route = new Route("GET", "/test", App);
			IDictionary<string, string> receivedParams = null;

			route.AddCondition(new RouteCondition(data => {
				receivedParams = data.RouteParameters;
				return true;
			}));

			var router = new Router();
			router.AddRoute(route);
			router.Resolve(Utils.BuildGetRequest("/test"));

			Assert.NotNull(receivedParams);
		}

		[Fact]
		public void ConditionData_is_passed_to_condition_function(){
			var route = new Route("GET", "/test", App);
			object conditionData = "test data to pass to check method";
			object receivedDataFromCheckMethod = null;

			route.AddCondition(new RouteCondition(
				x => { 
					receivedDataFromCheckMethod = x.ConditionData;
					return true;
				}, 
				conditionData)
           	);

			var router = new Router();
			router.AddRoute(route);
			router.Resolve(FakeRequest.Get("/test"));

			Assert.Same(conditionData, receivedDataFromCheckMethod);
		}

		[Fact]
		public void Route_parameters_are_passed_to_condition_function(){
			var route = new Route("GET", @"/products/{ProductId}-{Name}", App);

			IDictionary<string, string> receivedParams = null;

			route.AddCondition(new RouteCondition(data => {
				receivedParams = data.RouteParameters;
				return true;
			}));

			var router = new Router();
			router.AddRoute(route);
			router.Resolve(Utils.BuildGetRequest("/products/1234-myproduct"));

			Assert.Equal(new Dictionary<string, string>(){
				{ "ProductId" , "1234"},
				{ "Name" , "myproduct"}
			}, receivedParams);
		}

		[Fact]
		public void Route_instance_is_passed_to_condition_function(){
			var route = new Route("GET", "/test", App);
			Route receivedRoute = null;

			route.AddCondition(new RouteCondition(data => {
				receivedRoute = data.Route;
				return true;
			}));

			var router = new Router();
			router.AddRoute(route);
			router.Resolve(Utils.BuildGetRequest("/test"));

			Assert.Equal(route, receivedRoute);
		}

		[Fact]
		public void Owin_Environment_is_passed_to_condition_function(){
			var route = new Route("GET", "/test", App);
			OwinEnv receivedOwinEnv = null;

			route.AddCondition(new RouteCondition(data => {
				receivedOwinEnv = data.OwinEnv;
				return true;
			}));

			var router = new Router();
			router.AddRoute(route);

			var owinEnv = Utils.BuildGetRequest("/test");
			owinEnv["test"] = true;

			router.Resolve(owinEnv);
			Assert.Equal(true, receivedOwinEnv["test"]);
		}
	}
}

