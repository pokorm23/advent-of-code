global using Xunit;
global using Xunit.Abstractions;
global using Microsoft.Extensions.Logging;
using Xunit.Extensions.TestDependency;

[assembly: TestCaseOrderer(DependencyOrderer.TypeName, DependencyOrderer.AssemblyName)]