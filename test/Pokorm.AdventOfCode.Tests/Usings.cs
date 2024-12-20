global using Xunit;
global using Xunit.Abstractions;
global using Microsoft.Extensions.Logging;
global using Pokorm.AdventOfCode.Tests.Logging;
using Pokorm.AdventOfCode.Tests;

[assembly: TestCaseOrderer(AlphabeticalOrderer.TypeName, AlphabeticalOrderer.AssemblyName)]
