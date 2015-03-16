using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Barebone.Routing.Tests
{
    public class Router_Resolve_with_Predicate
    {
        private Func<IDictionary<string, object>, Task> App = (env) => Task.Factory.StartNew(() => { });
        Router router = new Router();

        [Fact]
        public void Only_Routes_are_in_result_of_resolve_when_predicate_matches()
        {
            var route_1 = new Route("GET", "/foo", App);
            route_1.Data.Add("has-to-be-in-data", new object());
            router.AddRoute(route_1);

            var route_2 = new Route("GET", "/foo", App);
            router.AddRoute(route_2);

            var result = router.Resolve(
                env: Utils.BuildGetRequest("/foo"), 
                predicate: route => route.Data.ContainsKey("has-to-be-in-data")
            );

            Assert.True(result.Success);
            Assert.Equal(route_1, result.Route);
        }

        [Fact]
        public void Route_gets_resolved_when_no_predicate_is_in_parameters()
        {
            var route_1 = new Route("GET", "/foo", App);
            route_1.Data.Add("has-to-be-in_data", new object());
            router.AddRoute(route_1);

            var result = router.Resolve(
                env: Utils.BuildGetRequest("/foo")
            );

            Assert.True(result.Success);
            Assert.Equal(route_1, result.Route);
        }

        [Fact]
        public void When_predicate_is_given_the_first_matching_route_ordered_by_priority_is_resolved()
        {
            var route_1 = new Route("GET", "/foo", App);
            route_1.Data.Add("has-to-be-in_data", new object());
            route_1.Priority = 1;
            router.AddRoute(route_1);                        

            var route_3 = new Route("GET", "/foo", App);
            route_3.Data.Add("has-to-be-in_data", new object());
            route_3.Priority = 3;
            router.AddRoute(route_3);

            var route_2 = new Route("GET", "/foo", App);
            route_2.Data.Add("has-to-be-in_data", new object());            
            route_2.Priority = 2;
            router.AddRoute(route_2);

            var result = router.Resolve(Utils.BuildGetRequest("/foo"));
            Assert.True(result.Success);
            Assert.Equal(route_3, result.Route);
        }
    }
}
