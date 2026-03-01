# MyGenerator — .NET Source Generator Template

This template scaffolds a complete .NET source generator solution that includes:

- **`MyGenerator/`** – The source generator project (`netstandard2.0`)
- **`MyGenerator.Tests/`** – Unit tests using **xUnit v3**
- **`MyGenerator.SnapshotTests/`** – Snapshot tests using **xUnit v3** + **Verify**

---

## Using the Template

Install the template (from the repository root):

```bash
dotnet new install templates/SourceGeneratorTemplate
```

Scaffold a new generator solution:

```bash
dotnet new sourcegen -n AwesomeGenerator
cd AwesomeGenerator
```

Remove the template installation when no longer needed:

```bash
dotnet new uninstall templates/SourceGeneratorTemplate
```

---

## Project Structure

```
AwesomeGenerator/
├── AwesomeGenerator/                   # Source generator (netstandard2.0)
│   ├── AwesomeGenerator.csproj
│   └── AwesomeGeneratorImpl.cs         # IIncrementalGenerator implementation
│
├── AwesomeGenerator.Tests/             # xUnit v3 unit tests
│   ├── AwesomeGenerator.Tests.csproj
│   ├── GlobalUsings.cs
│   ├── TestHelper.cs                   # Compiles code and runs the generator
│   └── AwesomeGeneratorTests.cs
│
├── AwesomeGenerator.SnapshotTests/     # xUnit v3 snapshot tests via Verify
│   ├── AwesomeGenerator.SnapshotTests.csproj
│   ├── GlobalUsings.cs
│   ├── TestHelper.cs                   # Runs generator and calls Verifier.Verify
│   ├── AwesomeGeneratorSnapshotTests.cs
│   └── Snapshots/                      # Verified snapshot files live here
│
└── AwesomeGenerator.slnx               # Solution file
```

---

## Getting Started

### Build

```bash
dotnet build AwesomeGenerator.slnx
```

### Run Unit Tests

```bash
dotnet test AwesomeGenerator.Tests/AwesomeGenerator.Tests.csproj
```

### Run Snapshot Tests

```bash
dotnet test AwesomeGenerator.SnapshotTests/AwesomeGenerator.SnapshotTests.csproj
```

### Run All Tests

```bash
dotnet test AwesomeGenerator.slnx
```

---

## How the Generator Works

The template implements a simple but realistic pattern:

1. **Inject an attribute** – `GenerateInfoAttribute` is added to every compilation via
   `RegisterPostInitializationOutput`.

2. **Detect marked classes** – `CreateSyntaxProvider` finds classes decorated with
   `[GenerateInfo]`.

3. **Generate a companion class** – For each marked class a `<ClassName>Info` static class
   is generated containing compile-time constants (type name, property count, method count)
   and a `GetSummary()` method.

Replace `GenerateInfoAttribute` and the corresponding generation logic in
`MyGeneratorImpl.cs` with your own attribute and code generation.

---

## Snapshot Testing with Verify

Snapshot tests use the [Verify](https://github.com/VerifyTests/Verify) library together
with `Verify.SourceGenerators` to capture and compare the exact text of every generated
source file.

### Running the Bundled Snapshot Tests

The template ships with two pre-accepted `.verified.txt` files in `MyGenerator.SnapshotTests/Snapshots/`, so the included snapshot tests **pass immediately** without any extra setup.

### First Run for New Tests

When you add a new snapshot test (or modify the generator), no `.verified.txt` file exists yet for that test. Verify will:

1. Write a `.received.txt` file in `Snapshots/` next to the test class.
2. **Fail the test** so you can inspect the output.
3. Accept it once the output looks correct (see below).

### Accepting / Updating Snapshots

**Via environment variable** (useful in CI pipelines):

```bash
VERIFY_AUTO_APPROVE=true dotnet test AwesomeGenerator.SnapshotTests
```

**Via test runner argument**:

```bash
dotnet test AwesomeGenerator.SnapshotTests -- Verify.AutoVerify=true
```

**Manually**: Rename the `.received.txt` file to `.verified.txt` (remove the `received`
segment) and commit it.

### Snapshot File Location

Verified snapshots are stored in `MyGenerator.SnapshotTests/Snapshots/` and should be
committed to source control so that future test runs can detect regressions.

---

## xUnit v3

This template targets **xUnit v3** (`xunit.v3` NuGet package), which provides:

- Improved parallelism and performance
- Cleaner async test support
- Better extensibility model

All standard xUnit attributes (`[Fact]`, `[Theory]`, `[InlineData]`, etc.) work the
same as in xUnit v2.

---

## Customizing the Generator

1. Open `MyGeneratorImpl.cs` and replace the `GenerateInfo` attribute name and logic
   with your own.
2. Update `MyGeneratorTests.cs` and `MyGeneratorSnapshotTests.cs` with tests for your
   new behaviour.
3. Run `dotnet test` to verify everything works.
4. Accept new snapshots as described above.

---

## Requirements

- **.NET 9.0** or later
- **C# 13** (LangVersion `latest` is set automatically)
