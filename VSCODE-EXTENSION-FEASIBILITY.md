# VS Code extension feasibility for reverse engineering

## Executive summary

A cross-platform VS Code extension is feasible as a thin UI over the EF Core Power Tools CLI. This proposal assumes that two CLI improvements are delivered and tested before extension development starts: connection-string input through a named environment variable, and side-effect-free database-object discovery with structured JSON output. These are planned prerequisites, not claims about functionality already implemented.

With those foundations in place, the first extension release can provide a guided workflow: select a connection reference or `.dacpac`, discover objects, manage exclusions and generation settings, save `efcpt-config.json`, and explicitly generate code. Object selection is part of the MVP rather than a later workaround. The CLI remains responsible for database access and scaffolding; the extension does not duplicate the generation engine.

**Recommendation:** deliver the CLI foundations first, then build a TypeScript workspace extension using native VS Code controls for object selection and common configuration settings. Retain schema-assisted JSON editing for advanced options. A full generation preview, credential manager and Visual Studio feature parity are outside the first release.

## Reusable repository foundations

The extension can build on these existing components:

- **Cross-platform generation:** the `ErikEJ.EFCorePowerTools.Cli` .NET tool exposes the `efcpt` command, with documented EF Core 8, 9 and 10 installation paths. See the [CLI guide](src/Core/efcpt.8/readme.md) and [tool project](src/Core/efcpt.8/efcpt.8.csproj).
- **Inputs and output control:** the CLI accepts database connection strings or SQL Server `.dacpac` paths, a provider, a root output directory, a config path and a renaming file. See [ScaffoldOptions.cs](src/Core/efcpt.8/Options/ScaffoldOptions.cs).
- **A canonical configuration format:** [efcpt-config.json](samples/efcpt-config.json) and its [JSON schema](samples/efcpt-config.schema.json) cover object lists, exclusions, generation settings, names, layout, replacements and type mappings. The extension should edit this format rather than introduce a competing configuration model.
- **Shared semantics:** [CliConfigMapper.cs](src/GUI/RevEng.Shared/Cli/CliConfigMapper.cs) maps configuration to generation options and handles object-list refresh. Reuse its behavior as the reference for selection semantics, without porting .NET scaffolding code into TypeScript.

These components reduce the extension's scope to UI, document editing and CLI orchestration. The extension is a separate VS Code package, not a port of the existing Visual Studio VSIX.

## Phase 1: CLI prerequisites

Complete both improvements before the extension MVP. Exact command names and flags should be defined by the CLI implementation, not assumed by this document.

### 1. Connection string from a named environment variable

Provide an explicit option that identifies the variable containing the connection string. Support it for both discovery and generation. Only the variable name travels in command-line arguments; the CLI reads the value from its inherited environment.

The contract should:

- Reject missing, empty or whitespace-only values with a clear error and nonzero exit code, without printing the value.
- Define conflicts between environment-variable input and positional connection input; reject ambiguous combinations rather than silently choosing one.
- Preserve existing positional connection-string and `.dacpac` usage for CLI callers.
- Avoid exposing connection values in normal output, diagnostics or generated artifacts for the extension workflow. Keep `enable-on-configuring` disabled for that workflow and warn if users enable connection-string generation.

This avoids repeated connection-string entry without introducing a credential manager. Environment variables are not a secret store and may be inspectable by processes running with sufficient privileges.

### 2. Side-effect-free object discovery

Provide a discovery operation for supported database providers and `.dacpac` input. It should return a versioned JSON response identifying object types and qualified names, with enough identity information to map objects to the canonical config without parsing display labels. Define optional metadata explicitly; do not assume that every provider exposes the same routines or column details.

The contract should:

- Write machine-readable results to stdout and diagnostics to stderr, without banners or progress messages corrupting the JSON response.
- Define schema version, exit codes, unsupported-provider behavior and cancellation behavior.
- Avoid generating code, creating templates, deleting files or creating/updating configuration files. Database access can still require credentials, permissions and provider-specific metadata queries.
- Be tested for its exact output shape and absence of local file/config side effects.

A generation dry-run is not required for this phase. Discovery answers which objects are available, not which files generation will write or delete.

## Phase 2: VS Code extension MVP

### User workflow

1. **Choose the project and tool:** select the workspace folder/project, compatible CLI major version and existing or new config file. Make the choice explicit in multi-root workspaces.
2. **Choose the source:** select a `.dacpac` file or enter/select an environment-variable name and provider. Remember the name and non-secret source metadata, never the connection value.
3. **Discover objects:** invoke CLI discovery with progress and cancellation. Show tables, views and routines grouped by type/schema, with search and include/exclude actions. A failed refresh must preserve the existing selection and show the error.
4. **Configure generation:** offer controls for common settings such as DbContext name, namespaces, entity/context output folders, data annotations and nullable reference types. Provide an Open JSON action for advanced settings, naming rules, templates and type mappings.
5. **Save the config:** apply explicit edits to `efcpt-config.json`, preserving existing exclusions, wildcard rules and unrelated properties. Discovery alone must not save or generate anything.
6. **Generate:** show the source reference, target paths and cleanup setting; ask the user to save pending edits and confirm the write operation. Run the CLI, show output, and report success, failure or cancellation. Allow the same configuration to be rerun without re-entering the connection string.

### UI choice

| Approach | Advantages | Trade-offs | Recommendation |
|---|---|---|---|
| Native TreeView with QuickPick/InputBox commands | Fits VS Code, supports grouped objects and focused setting edits, relatively small implementation | Less convenient than a single form for many interdependent options | Use for the MVP's object browser and common settings |
| Webview-based configuration editor | Supports a richer form and integrated object/settings layout | Requires document synchronization, accessibility, theming and webview security work | Consider after validating the native workflow |
| Schema-assisted JSON only | Existing format, broad option coverage, easy review in source control | Does not by itself provide guided object selection or configuration controls | Keep as an escape hatch, not the entire UI |

A webview is technically feasible but not a prerequisite. VS Code recommends native APIs when they are sufficient; see the [Webview guide](https://code.visualstudio.com/api/extension-guides/webview).

### Configuration and selection semantics

Keep `efcpt-config.json` authoritative. Use document edits integrated with VS Code save/undo behavior, preserve unknown properties and formatting where possible, and detect external changes rather than overwriting them. Do not write unsupported fields or materialize every schema default: runtime defaults and schema annotations need to be checked against the supported CLI version.

Object inclusion is more than a checkbox: wildcard exclusions and explicit overrides must retain their CLI meaning. Show whether an object is included through defaults/rules or an explicit override. Refreshing discovery should merge the displayed object list without silently resetting user choices or deleting config entries for temporarily unavailable objects.

Explain `refresh-object-lists` separately from UI refresh. The generation command may update the config when that setting is enabled; coordinate document saves and reloads, and show resulting changes instead of assuming generation only writes C# files.

## Architecture and operating constraints

Use a TypeScript/Node.js workspace extension that launches the selected CLI as a child process with an argument array, no shell interpolation, explicit working directory and explicit config/output paths. Keep database drivers and provider logic in the CLI. No language server or persistent .NET service is needed for the initial workflow.

- **Tool compatibility:** identify the installed package version and supported discovery contract before running. Reject incompatible versions with actionable setup instructions. Support a documented local-tool or global-tool invocation strategy; do not silently install/update a global tool or change a project manifest. EF Core 8/9/10 share the package ID and command name, so the executable name alone is insufficient to select a version.
- **Execution environment:** run the CLI where the workspace extension host runs. For WSL, containers or SSH, .NET, the tool, input files, database connectivity and environment variables must be available there, not merely on the desktop. Browser-only VS Code without a process-capable remote host is outside the MVP.
- **Environment inheritance:** a variable set in an integrated terminal does not automatically update the already-running extension host's environment. Document launching/restarting VS Code or its remote host with the variable available. Use exact variable names for portability; do not display variable values or enumerate all environment variables as connection profiles.
- **Workspace trust:** gate CLI execution and workspace-specified executable/template paths on Workspace Trust. Do not automatically execute tools when opening a folder or config.
- **Errors and cancellation:** distinguish missing tools/runtimes, missing variables, connection failures, incompatible JSON responses and generation errors. Parse only the discovery JSON contract; generation can use human-readable output and exit status. Cancellation of generation may leave partial output and must not be reported as success or rollback.
- **Output safety:** keep credentials out of logs, telemetry and settings, including provider exception messages. Show useful diagnostics without exposing secrets. Warn about overwrite/cleanup behavior and recommend source control; without a dry-run contract, do not claim to know the exact file change set. The [schema](samples/efcpt-config.schema.json) notes that cleanup can permanently delete files on Linux/macOS.
- **Packaging:** publish a distinct VS Code extension with its own manifest, dependencies and release workflow. Use `vsce` for packaging and validate on Windows, Linux and macOS. See the [publishing guide](https://code.visualstudio.com/api/working-with-extensions/publishing-extension).

## Acceptance criteria and delivery order

### Phase 1 completion gate

- Environment-variable input works for discovery and generation, preserves existing CLI usage, and handles missing/invalid/conflicting input explicitly without disclosing secrets.
- Discovery returns the agreed versioned JSON with clean stdout, useful diagnostics and defined exit/cancellation behavior.
- Tests verify exact discovery results and that discovery leaves generated files and configuration unchanged. Include a reproducible SQLite or `.dacpac` case and provider-specific checks for advertised routine discovery.

### Phase 2 completion gate

- A user can select a variable name, discover objects, edit selections/settings through the UI, save the canonical config, generate and rerun without entering the connection value.
- Existing wildcard rules, explicit overrides and unknown properties survive UI edits and discovery refresh; dirty documents and external changes are handled without data loss.
- Missing prerequisites, failed discovery, invalid config, cancellation and nonzero generation exits produce visible, actionable outcomes.
- End-to-end validation covers paths with spaces, multi-root workspaces and the supported operating systems. Remote-host environment setup is documented and verified before advertising remote support.

### Later enhancements

Consider a side-effect-free generation plan/dry-run, structured generation results, a richer webview editor and additional connection sources only after the MVP is validated. Saved credentials, automatic appsettings/user-secrets discovery, database-server browsing and full Visual Studio feature parity are not required for the first iteration.

The key sequencing decision is to finish and stabilize the two CLI contracts first. The extension can then deliver object selection and configuration management together, rather than launching as a JSON-and-run wrapper and adding its core UI later.

## References

Repository sources establish the reusable baseline; the Phase 1 CLI contracts above are proposed prerequisites.

- [CLI guide](src/Core/efcpt.8/readme.md), [arguments](src/Core/efcpt.8/Options/ScaffoldOptions.cs) and [generation orchestration](src/Core/efcpt.8/HostedServices/ScaffoldHostedService.cs)
- [Configuration schema](samples/efcpt-config.schema.json), [sample config](samples/efcpt-config.json) and [shared config mapper](src/GUI/RevEng.Shared/Cli/CliConfigMapper.cs)
- VS Code: [Tree View API](https://code.visualstudio.com/api/extension-guides/tree-view), [Webview API](https://code.visualstudio.com/api/extension-guides/webview), [Remote extensions](https://code.visualstudio.com/api/advanced-topics/remote-extensions), [Workspace Trust](https://code.visualstudio.com/api/extension-guides/workspace-trust), [Publishing](https://code.visualstudio.com/api/working-with-extensions/publishing-extension)
