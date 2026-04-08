# README

This DockerPlayground allows us to quickly verify the results of:
- live-db reverse engineering
- dacpac reverse engineering

This helps when wanting to quickly compare both of these paths as there are some minor differences between the two paths

## Setup/Build

1. First you must build the database project

Run the following:

```bash
dotnet build
```

This will get you a dacpac at 'bin/Debug/net10.0/DockerPlayground.dacpac'

### Setting up a containerized database

**Local SQL Server 2022**

This directory includes a `docker-compose.yml` to quickly spin up a SQL Server 2022 container locally.

1. Run the following:

```bash
docker compose up -d
```

2. Publish this database project to the running container:

```bash
dotnet publish -t:PublishDatabase \
  -p:TargetServerName=localhost \
  -p:TargetPort=1433 \
  -p:TargetDatabaseName=DockerPlayground \
  -p:TargetUser=sa \
  -p:TargetPassword="Password!"
```

3. (optional) connect to it using SQL authentication:

Connection string:

```text
Server=localhost,1433;Initial Catalog=DockerPlayground;User ID=sa;Password=Password!;Encrypt=False;TrustServerCertificate=True
```

For tools that expect separate arguments, use:

- Server: `localhost`
- Port: `1433`
- Database: `DockerPlayground`
- Username: `sa`

## Reverse Engineering

### efcpt from a live database

This is how we reverse engineer from the containeried database, what I'm calling the "live-db path"

For `efcpt` CLI commands, run:

```bash
efcpt 'Server=localhost,1433;Initial Catalog=DockerPlayground;User ID=sa;Password=Password!;Encrypt=False;TrustServerCertificate=True' mssql
```

You can verify the output that will exist in the 'Models' directory

### efcpt from a dacpac

This is how we reverse engineer from the dacpac, what I'm calling the "dacpac path"

```bash
efcpt 'bin/Debug/net10.0/DockerPlayground.dacpac' mssql
```

## Testing locally

After you fix bugs within the EFCorePowerTools repo directly, you can run the following to test your changes


### From dacpac

```sh
cd /home/nmummau/Code/EFCorePowerTools-Fork/test/ScaffoldingTester/DockerPlayground

dotnet run --project /home/nmummau/Code/EFCorePowerTools-Fork/src/Core/efcpt.8/efcpt.8.csproj -- \
  'bin/Debug/net10.0/DockerPlayground.dacpac' \
  mssql
```

### From database

```sh
cd /home/nmummau/Code/EFCorePowerTools-Fork/test/ScaffoldingTester/DockerPlayground

dotnet run --project /home/nmummau/Code/EFCorePowerTools-Fork/src/Core/efcpt.8/efcpt.8.csproj -- \
  'Server=localhost,1433;Initial Catalog=DockerPlayground;User ID=sa;Password=Password!;Encrypt=False;TrustServerCertificate=True' \
  mssql
```