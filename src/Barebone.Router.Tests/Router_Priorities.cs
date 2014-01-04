using System;
using Xunit;
using System.Collections.Generic;

namespace Barebone.Routing.Tests
{


	/// <summary>
	/// Router_ priorities.
	/// </summary>

	public class Router_Priorities : RouterTestBase {
		[Fact]
		public void If_a_url_matches_multiple_routes_the_routes_are_checked_ordered_by_priority(){
			var router = new Router();

			var listOfPriorities = new List<int>();

			var condition = new RouteCondition(x => {
				//Adds the priorities of the checked route to the list of priorities
				listOfPriorities.Add(x.Route.Priority);
				return false;
			});

			var route = new Route("GET", "/foo/{action}", App);
			route.Priority = 6;
			route.AddCondition(condition);
			router.AddRoute(route);

			route = new Route("GET", "/foo/{action}", App);
			route.Priority = 10;
			route.AddCondition(condition);
			router.AddRoute(route);

			route = new Route("GET", "/foo/{action}", App);
			route.Priority = 8;
			route.AddCondition(condition);
			router.AddRoute(route);

			route = new Route("GET", "/foo/{action}", App);
			route.Priority = 7;
			route.AddCondition(condition);
			router.AddRoute(route);

			router.Resolve(Utils.BuildGetRequest("/foo/test"));
			Assert.Equal(new[] { 10, 8,7, 6 }, listOfPriorities.ToArray());
		}

		[Fact]
		public void If_multiple_routes_match_a_request_the_route_with_the_highest_priority_is_selected(){
			var router = new Router();
			var listOfPriorities = new List<int>();

			var falseCondition = new RouteCondition(x => {
				//Adds the priorities of the checked route to the list of priorities
				listOfPriorities.Add(x.Route.Priority);
				return false;
			});

			var trueCondition = new RouteCondition(x => {
				//Adds the priorities of the checked route to the list of priorities
				listOfPriorities.Add(x.Route.Priority);
				return true;
			});

			var route = new Route("GET", "/foo/{action}", App);
			route.Priority = 6;
			route.AddCondition(falseCondition);
			router.AddRoute(route);

			route = new Route("GET", "/foo/{action}", App);
			route.Priority = 7;
			route.AddCondition(trueCondition);
			router.AddRoute(route);

			route = new Route("GET", "/foo/{action}", App);
			route.Priority = 8;
			route.AddCondition(trueCondition);
			router.AddRoute(route);

			var result = router.Resolve(Utils.BuildGetRequest("/foo/test"));
			Assert.Equal(8, result.Route.Priority);
		}

		[Fact]
		public void When_a_route_is_found_routes_with_a_lower_priority_are_not_checked(){
			var router = new Router();
			var listOfPriorities = new List<int>();

			var falseCondition = new RouteCondition(x => {
				//Adds the priorities of the checked route to the list of priorities
				listOfPriorities.Add(x.Route.Priority);
				return false;
			});

			var trueCondition = new RouteCondition(x => {
				//Adds the priorities of the checked route to the list of priorities
				listOfPriorities.Add(x.Route.Priority);
				return true;
			});

			var route = new Route("GET", "/foo/{action}", App);
			route.Priority = 8;
			route.AddCondition(falseCondition);
			router.AddRoute(route);

			route = new Route("GET", "/foo/{action}", App);
			route.Priority = 6;
			route.AddCondition(trueCondition);
			router.AddRoute(route);

			route = new Route("GET", "/foo/{action}", App);
			route.Priority = 7;
			route.AddCondition(trueCondition);
			router.AddRoute(route);

			router.Resolve(Utils.BuildGetRequest("/foo/test"));

			// 8 should not be included!
			Assert.Equal(new[] { 8,7 }, listOfPriorities.ToArray()); 
		}
	}
}