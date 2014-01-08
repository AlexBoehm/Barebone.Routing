Barebone.Router - OWIN Router for .NET/Mono
===========================================

```csharp
using OwinEnv = IDictionary<string, object>;

public class SimpleExample{
	public Task ProcessRequest(OwinEnv env){
		var router = new Router();
		var route = new Route("GET", "/info", InfoAppFunc);
		router.AddRoute(route);

		var result = router.Resolve(env);

		if (result.Success) {
			return result.Route.OwinAction(env);
		} else {
			// Do something else
		}
	}

	private Task InfoAppFunc(OwinEnv env){
		return Task.Factory.StartNew(() => {
			// show info page
		});
	}
}
```

Some Info
------------------

- Barebone.Router is an OWIN Router for .NET and Mono.
- It is designed for high performance.
- It does not use regular expressions to check the given path against route entries. It uses a tree based algorithm instead
- It does not have any dependencies on other projects (except xunit.net)
- It is designed to be a standalone component which can easily be used in applications using owin

Features
------------------

- Supports route conditions
- Supports parameter conditions
- Allows you to store arbitray data with each route entry
- Supports route parameters
- Supports route priorities in case that a path matches more than one route
- Provides a fluent API on top

Samples
------------------

### Parameters in routes

```csharp
var route = new Route("GET", "/foo/{action}", App);
var router = new Router();
router.AddRoute(route);
var result = router.Resolve(FakeRequest.Get("/foo/test"));
Assert.Equal(route, result.Route);
```

### Multiple parameters in routes

```csharp
var route = new Route("GET", "/foo/{action}-{subaction}", App);
var router = new Router();
router.AddRoute(route);
var result = router.Resolve(FakeRequest.Get("/foo/do-this"));
Assert.Equal(
	new Dictionary<string, string> {
		{"action", "do"},
		{"subaction", "this"},
	},
	result.Parameters
);
```

### Dynamic segment with static end

```csharp
var route = new Route("GET", "/foo/{action}-{subaction}.html", App);
var router = new Router();
router.AddRoute(route);
var result = router.Resolve(FakeRequest.Get("/foo/do-this.html"));
Assert.Equal(
	new Dictionary<string, string> {
		{"action", "do"},
		{"subaction", "this"},
	},
	result.Parameters
);
```

### Parameter conditions

```csharp
var route = new Route("GET", "/test/{ProductId}/{title}", App);
route.AddCondition("ProductId", value => true);
route.AddCondition("ProductId", value => true);
route.AddCondition("Title", value => true);
Assert.True(RoutesTo(route, _other, "/test/foo/nice-product"));
```

### Parameter conditions with additional data

```csharp
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
```

Fluent API
------------------

```csharp
using OwinEnv = IDictionary<string, object>;

public class ReadmeExample{
	public static void Sample() {
		Routes routes = new Routes();

		routes
			.Get("/foo")
			.Condition("en", x => x.ConditionData.Equals("en"))
			.Action(InfoAppFunc);
		
		var router = new Router();
		router.AddRoutes(routes);
	}

	private static Task InfoAppFunc(OwinEnv env){
		return Task.Factory.StartNew(() => {
			// show info page
		});
	}
}
```

Contributions
-------------

Contributions are always welcome. There are a lot of things to do, for example:

- The fluent API has to be extended. Just a few features are implemented yet.
- Performance tests are in need to make sure that the router is working effectively
- Integrate with nuget
