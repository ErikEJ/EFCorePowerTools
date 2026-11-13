# EF Core Power Tools

EF Core Power Tools is a comprehensive toolkit for Entity Framework Core development consisting of a Visual Studio extension (VSIX), command-line tools (CLI), and supporting libraries. It enables reverse engineering of databases into EF Core models, provides data visualization tools, and includes advanced scaffolding capabilities.

Always reference these instructions first and fallback to search or bash commands only when you encounter unexpected information that does not match the info here.

## Working Effectively

### Prerequisites and Setup
- **CRITICAL**: The CLI and EF Core 10 projects require the .NET 10 SDK; EF Core 8 core components can still be built with .NET 8.
- For development with .NET 8.0 SDK only: `{"sdk": {"version": "8.0.119", "allowPrerelease": false, "rollForward": "latestFeature"}}`
- Projects requiring .NET 10: `RevEng.Core.100`, `efreveng100`, `efcpt.10`, `efpt100.core` (will fail with .NET 8 SDK)
- The CLI and reverse-engineering tool now target EF Core 10.

### Build and Test the Repository
- Navigate to repository root: `cd /home/runner/work/EFCorePowerTools/EFCorePowerTools`
- **Building CLI and reverse-engineering tools (EF Core 10)**:
  - `dotnet build src/Core/efcpt.10/efcpt.10.csproj` -- takes 15-20 seconds from clean. NEVER CANCEL. Set timeout to 60+ seconds.
  - `dotnet build src/Core/efreveng100/efreveng100.csproj` -- takes 10-15 seconds from clean. NEVER CANCEL. Set timeout to 60+ seconds.
- **Running tests**:
  - `dotnet test src/Core/NUnitTestCore/NUnitTestCore.csproj` -- EF Core 10 core and CLI tests.
  - NEVER CANCEL; set timeout to 120+ seconds.
- **Building dacpac projects**:
  - `cd test/ScaffoldingTester/Chinook && dotnet build` -- takes 7-10 seconds. NEVER CANCEL. Set timeout to 60+ seconds.

### CLI Tool Installation and Usage
- **Install globally**: `dotnet tool install ErikEJ.EFCorePowerTools.Cli -g --version 10.*`
- **Update globally**: `dotnet tool update ErikEJ.EFCorePowerTools.Cli -g --version 10.*`
- **Basic usage**: `efcpt --help` (takes 1-2 seconds to run)
- **Test with dacpac**: `efcpt path/to/file.dacpac mssql` (takes 3-5 seconds for typical database)

### Visual Studio Extension (VSIX)
- The main solution `src/EFCorePowerTools.slnx` builds the VSIX but requires Windows and Visual Studio build tools
- **CRITICAL**: VSIX building requires Windows environment with MSBuild and Visual Studio SDK
- On Linux: Can build core components but not the full VSIX package
- Build workflow uses specific CMD scripts in `src/Core/efreveng*/BuildCmdlineTool.cmd`

## Validation

### Essential Validation Scenarios
- **ALWAYS test CLI functionality** after making changes:
  1. Install/update CLI tool globally: `dotnet tool update ErikEJ.EFCorePowerTools.Cli -g --version 10.*`
  2. Create test directory: `mkdir /tmp/efcpt_test && cd /tmp/efcpt_test`
  3. Test with sample dacpac: Copy from `test/ScaffoldingTester/Chinook/obj/Debug/netstandard2.1/ErikEJ.Dacpac.Chinook.dacpac`
  4. Run: `efcpt ErikEJ.Dacpac.Chinook.dacpac mssql`
  5. Verify: Generated models in `Models/` directory and config file `efcpt-config.json`
  6. Test config modification: Edit `efcpt-config.json`, change `"use-data-annotations": true`, re-run
  7. Verify: Data annotations added to generated classes

- **ALWAYS run tests** to ensure no regressions:
  - `dotnet test src/Core/NUnitTestCore/NUnitTestCore.csproj` -- NEVER CANCEL, wait 2+ minutes

- **Build validation timing expectations**:
  - Clean CLI build: 15-20 seconds
  - Incremental CLI build: 3-5 seconds  
  - Test suite: 20-25 seconds
  - Dacpac build: 7-10 seconds
  - CLI tool execution: 3-5 seconds

### Manual Testing Workflows
1. **Reverse Engineering Workflow**:
   - Build dacpac: `cd test/ScaffoldingTester/Chinook && dotnet build`
   - Test CLI: `efcpt path/to/dacpac.dacpac mssql`
   - Verify output: Check Models directory, configuration file, readme

2. **Configuration Testing**:
   - Modify `efcpt-config.json` settings (data annotations, namespaces, etc.)
   - Re-run CLI command
   - Verify changes reflected in generated code

3. **Build System Validation**:
   - Clean build test: `dotnet clean && dotnet build`
   - Dependency verification: Check all referenced packages restore correctly

## Common Tasks

### Frequently Used Commands
```bash
# Core development workflow
cd /home/runner/work/EFCorePowerTools/EFCorePowerTools

# Build CLI 10 tool
dotnet build src/Core/efcpt.10/efcpt.10.csproj

# Run tests  
cd src/Core/NUnitTestCore && dotnet test

# Build test dacpac
cd test/ScaffoldingTester/Chinook && dotnet build

# Install/update CLI globally
dotnet tool update ErikEJ.EFCorePowerTools.Cli -g --version 10.*

# Test CLI with dacpac
cd /tmp && mkdir test_efcpt && cd test_efcpt
cp /home/runner/work/EFCorePowerTools/EFCorePowerTools/test/ScaffoldingTester/Chinook/obj/Debug/netstandard2.1/ErikEJ.Dacpac.Chinook.dacpac .
efcpt ErikEJ.Dacpac.Chinook.dacpac mssql
```

### Repository Structure Overview
```
src/
├── Core/                    # Core reverse engineering libraries and CLI tools
│   ├── efcpt.10/           # CLI tool for EF Core 10 (requires .NET 10 SDK)
│   ├── efreveng100/        # Reverse-engineering tool for EF Core 10
│   ├── RevEng.Core.80/     # Shared core reverse engineering source
│   └── NUnitTestCore/      # Core test suite (74 tests)
├── GUI/                     # Visual Studio extension components
│   ├── EFCorePowerTools/   # Main VSIX project (Windows/VS only)
│   └── Shared/             # Shared GUI components
└── Nupkg/                  # NuGet packages

test/
├── ScaffoldingTester/      # Test applications and sample databases
│   └── Chinook/            # Sample dacpac for testing

samples/                    # Configuration examples and schemas
├── efcpt-config.json       # Complete sample configuration
└── efcpt-config.schema.json # JSON schema
```

### Common Configuration Files
- `global.json`: SDK version requirements
- `src/Directory.Build.props`: MSBuild common properties, analyzers, code style
- `src/EFCorePowerTools.slnx`: Main solution (includes VSIX projects)
- `samples/efcpt-config.json`: Complete CLI configuration example
- `.editorconfig`: Code style and formatting rules

### Development Workflow Timing
| Operation | Expected Time | Timeout Setting |
|-----------|---------------|-----------------|
| CLI build (clean) | 15-20s | 60s |
| CLI build (incremental) | 3-5s | 30s |
| Test suite | 20-25s | 120s |
| Dacpac build | 7-10s | 60s |
| CLI execution | 3-5s | 30s |
| Tool install/update | 10-15s | 60s |

**CRITICAL REMINDERS**:
- **NEVER CANCEL** builds or tests - they may appear to hang but are working
- Always use **NEVER CANCEL** warnings and appropriate timeout values
- EF Core 10 projects require .NET 10 SDK - modify global.json if needed
- CLI tools are the primary way to test functionality outside Visual Studio
- Always validate end-to-end scenarios: build → install → test → verify output