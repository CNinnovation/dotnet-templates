# .NET Templates

A collection of `dotnet new` templates for .NET development, designed to streamline the creation of common project types and accelerate development workflows.

## Source Generator Template

A comprehensive template for creating C# source generators with built-in testing infrastructure, including xUnit v3 and snapshot testing support.

### Features

- **Source Generator Project** - Pre-configured Roslyn incremental source generator with `netstandard2.0` targeting
- **xUnit v3 Test Project** - Modern testing infrastructure with the latest xUnit version
- **Snapshot Testing** - Integrated Verify framework for snapshot-based testing
- **Flexible Configuration** - Choose your target framework (.NET 9 or .NET 10)
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
- `--Framework` - Target framework: `net9.0` or `net10.0` (default: `net10.0`)
- `--IncludeTests` - Include unit test project (default: `true`)
- `--IncludeSnapshotTests` - Include snapshot test project (default: `true`)

**Examples:**

```bash
# Create with .NET 9 target framework
dotnet new sourcegen -n MyGenerator --Framework net9.0

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

## Requirements

- .NET 8 SDK or later
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
