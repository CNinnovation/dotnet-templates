namespace MyGenerator.SnapshotTests;

/// <summary>
/// Snapshot tests for <see cref="MyGeneratorImpl"/>.
///
/// Each test calls <see cref="TestHelper.Verify"/> which:
///  1. Compiles the provided source string with the generator.
///  2. On the first run, writes a <c>.verified.txt</c> file in the
///     <c>Snapshots/</c> directory and fails (so you can review the output).
///  3. On subsequent runs, compares the output to the stored snapshot and
///     fails only when the generated code has changed unexpectedly.
///
/// To accept new/changed snapshots run:
///   dotnet test -- Verify.AutoVerify=true
/// or set the environment variable VERIFY_AUTO_APPROVE=true.
/// </summary>
public class MyGeneratorSnapshotTests
{
    [Fact]
    public Task GeneratesInfoForSimpleClass()
    {
        var source = """
            using MyGenerator.Attributes;

            namespace TestNamespace;

            [GenerateInfo]
            public class Person
            {
                public string Name { get; set; } = string.Empty;
                public int Age { get; set; }
            }
            """;

        return TestHelper.Verify(source);
    }

    [Fact]
    public Task GeneratesInfoForClassWithMethods()
    {
        var source = """
            using MyGenerator.Attributes;

            namespace TestNamespace;

            [GenerateInfo]
            public class Calculator
            {
                public int Add(int a, int b) => a + b;
                public int Subtract(int a, int b) => a - b;
                private int Multiply(int a, int b) => a * b;
            }
            """;

        return TestHelper.Verify(source);
    }
}
