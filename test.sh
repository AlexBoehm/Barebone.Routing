#/bin/sh
mono tools/xunit/xunit.console.clr4.exe src/Barebone.Router.Tests/bin/Debug/Barebone.Router.Tests.dll /nunit test_results_router.xml
mono tools/xunit/xunit.console.clr4.exe src/Barebone.Router.Fluent.Tests/bin/Debug/Barebone.Router.Fluent.Tests.dll /nunit test_results_fluent_api.xml
