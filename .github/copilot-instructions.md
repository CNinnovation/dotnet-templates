# Copilot Instructions

This repository is a collection of `dotnet new` templates (currently one: `SourceGeneratorTemplate`, `dotnet new` short name `cni-sourcegen`). There is no single app to build/run — work centers on editing template content and validating it by generating a project from the template and building/testing that generated output.

## Repository layout

- `templates/SourceGeneratorTemplate/SourceGenerator.Template.csproj` — the NuGet template package project (`PackageId: CNinnovation.Templates.SourceGenerator`). `PackageVersion` here is bumped by the `deploy-template.yml` workflow, not manually in normal PRs.
- `templates/SourceGeneratorTemplate/content/` — the actual template source that gets scaffolded via `dotnet new`. Contains `MyGenerator` (the generator itself, `netstandard2.0` by default), `MyGenerator.Tests` (xUnit v3), `MyGenerator.SnapshotTests` (Verify snapshot tests), plus `README.md`/`THIRD-PARTY-NOTICES.md` that ship with generated projects.
- `templates/SourceGeneratorTemplate/content/.template.config/template.json` — defines template symbols/parameters (`Framework`, `GeneratorFramework`, `IncludeTests`, `IncludeSnapshotTests`, `SyntaxProviderMode`) and conditional file inclusion. Any new parameter must be wired here, referenced via template conditionals (e.g. `<!--#if (!IsNetStandard20)-->`) in the content files, and reflected in the README usage docs.

## Build / test / validate

There's no template source itself to "build" — validate changes by installing the template locally and generating a project, then building/testing the generated output:

```powershell
dotnet new install .\templates\SourceGeneratorTemplate\
dotnet new cni-sourcegen -n TestGenerator --Framework net9.0 --GeneratorFramework netstandard2.0 --IncludeTests true --IncludeSnapshotTests true
cd TestGenerator
dotnet build TestGenerator.slnx --configuration Release
dotnet test TestGenerator.Tests/ --configuration Release --no-build
dotnet test TestGenerator.SnapshotTests/ --configuration Release --no-build
```

Run a single test in the generated project the normal `dotnet test` way, e.g. `dotnet test TestGenerator.Tests/ --filter "FullyQualifiedName~SomeTestName"`.

Uninstall between iterations with `dotnet new uninstall CNinnovation.Templates.SourceGenerator` (or `dotnet new uninstall .\templates\SourceGeneratorTemplate\` while still installed from that path) if you need a clean reinstall.

## CI workflows (`.github/workflows/`)

- `integration-tests-source-generator.yml` — runs on push/PR touching `templates/SourceGeneratorTemplate/**`. Matrix-generates the template across .NET 9/10, generator frameworks (`netstandard2.0`/`net8.0`/`net9.0`/`net10.0`), `IncludeTests`/`IncludeSnapshotTests` on/off, and `SyntaxProviderMode` variants, then verifies expected files exist/are absent, builds, and tests. **When adding a new template parameter or file, add/update a matrix entry and the file-existence checks here.**
- `update-template-packages.yml` — scheduled job that generates a project from the template, runs `dotnet package list --outdated`, and patches package versions directly in the template `content/**/*.csproj` files and `content/README.md` (including a special-case display-name mapping for `xunit.v3` → "xUnit v3"). Opens a PR with the changes.
- `deploy-template.yml` — manual (`workflow_dispatch`) release: bumps `PackageVersion` in `SourceGenerator.Template.csproj` via `sed`, packs, pushes to NuGet, and tags the release.

## Key conventions

- Generated scaffold output (e.g. package versions in `MyGenerator.csproj`/`MyGenerator.Tests.csproj`, README version callouts) is a **user-visible contract**. Package version bumps in `content/**` must stay consistent between the `.csproj` files and the README's version-display text, since `update-template-packages.yml` edits both in lockstep with matching literal strings.
- Template conditionals use dotnet template engine syntax embedded in comments, e.g. `<!--#if (!IsNetStandard20)-->...<!--#endif-->` in `.csproj`/`.cs` files — computed symbols like `IsNetStandard20` and `UseForAttributeWithMetadataName` drive these branches based on parameter choices.
- The template targets multiple frameworks by design (`netstandard2.0` default for broad compatibility with older Visual Studio/analyzer hosts, up through `net10.0`); don't drop older framework support without checking `template.json` choices and the CI matrix.