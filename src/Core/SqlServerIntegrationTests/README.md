# SQL Server integration tests

Run from the repository root with Docker running in Linux container mode and the .NET 8 and 10 SDKs installed (10 builds the SQL project):

```sh
dotnet test src/Core/SqlServerIntegrationTests/SqlServerIntegrationTests.csproj
```

Testcontainers starts SQL Server 2025 CU9, waits for readiness, assigns a random host port and password, and disposes the container after the test. The first run downloads the image; allow enough Docker memory for SQL Server (at least 2 GB). No existing database or connection-string environment variable is required. Docker or startup failures fail the test rather than silently skipping it.

The test builds the real `MasterReference` SQL project and reads its referenced Microsoft `master.dacpac`. A shared test-only helper constructs a keyless object-property model from the column names, without querying server metadata. Against `sys.databases`, the test verifies:

- String casts in a query predicate and integer casts in a projection.
- Materialization of string, integer, date/time, and Boolean values.
- Materialization of every column declared by the DACPAC for this view.
- No tracked entities or migration table mappings.

This validates the object-fallback prototype for issue #3554; it does not implement the proposed CLI option. Offline metadata and code-generation coverage remains in `NUnitTestCore`, which does not require Docker. The CLI workflow runs this live suite in a separate Linux job.
