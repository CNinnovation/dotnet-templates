# .NET Templates

A collection of `dotnet new` templates for .NET development, designed to streamline the creation of common project types and accelerate development workflows.

## Source Generator Template

A comprehensive template for creating C# source generators with built-in testing infrastructure, including xUnit v3 and snapshot testing support.

### Features

- **Source Generator Project** - Pre-configured Roslyn incremental source generator with configurable target framework (`netstandard2.0` by default)
- **xUnit v3 Test Project** - Modern testing infrastructure with the latest xUnit version
- **Snapshot Testing** - Integrated Verify framework for snapshot-based testing
- **Flexible Configuration** - Choose your target framework (.NET Standard 2.0, .NET 8, .NET 9, or .NET 10)
- **Optional Components** - Include/exclude unit tests and snapshot tests as needed
- **Sample Code** - Working example to get started immediately

### Installation

Install the template from NuGet:

```bash
dotnet new install CNinnovation.SourceGenerator.Template
```

Or install locally from the repository:

```bash
dotnet new install ./templates/SourceGeneratorTemplate
```

### Usage

Create a new source generator project with all features:

```bash
dotnet new sourcegen -n MyGenerator
```

**Options:**

- `-n|--name` - Name of the source generator (default: `MyGenerator`)
- `--GeneratorFramework` - Target framework for the source generator project: `netstandard2.0`, `net8.0`, `net9.0`, or `net10.0` (default: `netstandard2.0`)
- `--Framework` - Target framework for the test projects: `net9.0` or `net10.0` (default: `net10.0`)
- `--IncludeTests` - Include unit test project (default: `true`)
- `--IncludeSnapshotTests` - Include snapshot test project (default: `true`)

**Examples:**

```bash
# Create with .NET Standard 2.0 generator (supports .NET Framework consumers)
dotnet new sourcegen -n MyGenerator --GeneratorFramework netstandard2.0

# Create with .NET 9 generator and .NET 9 test framework
dotnet new sourcegen -n MyGenerator --GeneratorFramework net9.0 --Framework net9.0

# Create without snapshot tests
dotnet new sourcegen -n MyGenerator --IncludeSnapshotTests false

# Create only the generator (no tests)
dotnet new sourcegen -n MyGenerator --IncludeTests false --IncludeSnapshotTests false
```

### Generated Structure

```
MyGenerator/
├── MyGenerator/                    # Source generator library (netstandard2.0)
├── MyGenerator.Tests/              # xUnit v3 unit tests
└── MyGenerator.SnapshotTests/      # Verify snapshot tests
```

### Uninstall

```bash
dotnet new uninstall CNinnovation.SourceGenerator.Template
```

## CI / Integration Tests

Automated integration tests run via GitHub Actions on every push or pull request that changes files under `templates/SourceGeneratorTemplate/`. They can also be triggered manually from the **Actions** tab.

### What is tested

The workflow (`integration-tests-source-generator.yml`) exercises the following combinations:

| Job | .NET SDK | `--Framework` | `--GeneratorFramework` | `--IncludeTests` | `--IncludeSnapshotTests` |
|-----|----------|--------------|------------------------|-----------------|-------------------------|
| net9 \| all options \| netstandard2.0 generator | 9.x | net9.0 | netstandard2.0 | true | true |
| net9 \| all options \| net8.0 generator | 9.x | net9.0 | net8.0 | true | true |
| net9 \| all options \| net9.0 generator | 9.x | net9.0 | net9.0 | true | true |
| net10 \| all options \| netstandard2.0 generator | 10.x | net10.0 | netstandard2.0 | true | true |
| net10 \| all options \| net10.0 generator | 10.x | net10.0 | net10.0 | true | true |
| net9 \| generator only | 9.x | net9.0 | netstandard2.0 | false | false |
| net9 \| unit tests only | 9.x | net9.0 | netstandard2.0 | true | false |
| net9 \| snapshot tests only | 9.x | net9.0 | netstandard2.0 | false | true |

### Each job performs

1. **Install** – installs the template from the local repository source.
2. **Generate** – runs `dotnet new sourcegen` with the matrix parameters.
3. **Verify** – checks that all expected files and directories are present (and absent ones do not exist).
4. **Build** – compiles the generated solution with `dotnet build --configuration Release`.
5. **Test** – runs any included test projects with `dotnet test --configuration Release --no-build`.

Build or test failures are reported with full output so failures can be diagnosed quickly.

## Requirements

- .NET 9 SDK or later (or .NET 10 preview for `net10.0` targets)
- Visual Studio 2022 17.7 or later (recommended for out-of-process source generator development experience)
- C# 12.0 or later

## Contributing

Contributions are welcome! Before you submit a Pull Request, create an issue to discuss it.

## License

This project is licensed under the MIT License.

## Author

**CN innovation**
- GitHub: [https://github.com/CNinnovation](https://github.com/CNinnovation)
- Project: [https://github.com/CNinnovation/dotnet-templates](https://github.com/CNinnovation/dotnet-templates)

## Resources

- [Source Generators Documentation](https://learn.microsoft.com/en-us/dotnet/csharp/roslyn-sdk/source-generators-overview)
- [Incremental Generators](https://github.com/dotnet/roslyn/blob/main/docs/features/incremental-generators.md)
- [xUnit Documentation](https://xunit.net/)
- [Verify Snapshot Testing](https://github.com/VerifyTests/Verify)
- [dotnet new Templates](https://learn.microsoft.com/en-us/dotnet/core/tools/custom-templates)
