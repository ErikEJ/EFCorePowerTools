<#
.SYNOPSIS
    Verifies that the efcpt "schemas" option scaffolds exactly the objects in the selected schemas.

.DESCRIPTION
    Builds the AdventureWorks dacpac and the EF Core 10 efcpt CLI from source, then scaffolds
    the dacpac once per scenario. Each scenario selects two or three schemas, and the tables,
    views, stored procedures and functions found in the generated code must match the expected
    objects for those schemas exactly, plus any objects from other schemas that a scenario lists
    with "exclude": false. efcpt must not add any objects to the efcpt-config.json object lists.

.EXAMPLE
    pwsh ./Test-SchemaScaffolding.ps1

    The generated code for each scenario is written to the output folder next to this script,
    which is cleared at the start of every run.

.EXAMPLE
    pwsh ./Test-SchemaScaffolding.ps1 -SkipBuild
#>
[CmdletBinding()]
param(
    # Use the existing dacpac and efcpt build output.
    [switch] $SkipBuild
)

$ErrorActionPreference = 'Stop'

$repoRoot = (Resolve-Path (Join-Path $PSScriptRoot '../../..')).Path
$sqlProject = Join-Path $PSScriptRoot 'ErikEJ.Dacpac.AdventureWorks.csproj'
$dacpac = Join-Path $PSScriptRoot 'bin/Debug/netstandard2.1/ErikEJ.Dacpac.AdventureWorks.dacpac'
$efcptProject = Join-Path $repoRoot 'src/Core/efcpt.10/efcpt.10.csproj'
$efcpt = Join-Path $repoRoot 'src/Core/efcpt.10/bin/Debug/net10.0/efcpt.10.dll'

# Every object in the AdventureWorks dacpac, by schema.
$database = @{
    dbo            = @{
        Tables     = 'AWBuildVersion', 'ErrorLog'
        Views      = @()
        Procedures = @('uspPrintError')
        Functions  = 'ufnGetSalesOrderStatusText', 'ufnGetStock'
    }
    HumanResources = @{
        Tables     = 'Department', 'Employee', 'EmployeeDepartmentHistory', 'EmployeePayHistory', 'JobCandidate', 'Shift'
        Views      = @('vEmployee')
        Procedures = 'uspGetEmployeesByDepartment', 'uspUpdateEmployeeHireInfo'
        Functions  = @()
    }
    Person         = @{
        Tables     = 'Address', 'AddressType', 'BusinessEntity', 'BusinessEntityAddress', 'CountryRegion', 'EmailAddress', 'Person', 'PersonPhone', 'PhoneNumberType', 'StateProvince'
        Views      = @('vStateProvinceCountryRegion')
        Procedures = @()
        Functions  = @()
    }
    Production     = @{
        Tables     = 'Location', 'Product', 'ProductCategory', 'ProductInventory', 'ProductModel', 'ProductReview', 'ProductSubcategory', 'ScrapReason', 'UnitMeasure', 'WorkOrder'
        Views      = @()
        Procedures = @()
        Functions  = @()
    }
    Purchasing     = @{
        Tables     = 'ProductVendor', 'PurchaseOrderDetail', 'PurchaseOrderHeader', 'ShipMethod', 'Vendor'
        Views      = @('vVendorWithAddresses')
        Procedures = @()
        Functions  = @()
    }
    Sales          = @{
        Tables     = 'CreditCard', 'Currency', 'Customer', 'SalesOrderDetail', 'SalesOrderHeader', 'SalesOrderHeaderSalesReason', 'SalesPerson', 'SalesReason', 'SalesTerritory', 'SpecialOffer', 'Store'
        Views      = @('vSalesPerson')
        Procedures = @('uspGetCustomerOrders')
        Functions  = @('ufnGetCustomerOrderCount')
    }
}

$scenarios = @(
    @{ Name = 'Two schemas'; Schemas = 'Sales', 'Production' }
    @{ Name = 'Three schemas'; Schemas = 'HumanResources', 'Person', 'Purchasing' }
    @{ Name = 'Three schemas including dbo'; Schemas = 'dbo', 'HumanResources', 'Sales' }
    @{ Name = 'Case-sensitive schemas without refresh'; Schemas = 'Sales', 'person'; NoRefresh = $true }

    # Objects outside the schemas are generated when they are listed with "exclude": false.
    @{
        Name    = 'One schema plus objects from other schemas'
        Schemas = @('Sales')
        Extras  = 'Tables Production.Product', 'Tables Person.Person', 'Procedures HumanResources.uspGetEmployeesByDepartment'
    }
)

# Config file section for each object type.
$sections = [ordered]@{ Tables = 'tables'; Views = 'views'; Procedures = 'stored-procedures'; Functions = 'functions' }

function Invoke-Checked([string] $what, [scriptblock] $command) {
    Write-Host "==> $what" -ForegroundColor Cyan
    $output = & $command 2>&1
    if ($LASTEXITCODE -ne 0) {
        $output | Write-Host
        throw "$what failed with exit code $LASTEXITCODE"
    }
}

function Get-ExpectedObjects([string[]] $schemas) {
    $expected = foreach ($schema in $schemas) {
        # Schema names are case-sensitive, so a schema with different casing matches nothing.
        $key = $database.Keys | Where-Object { $_ -ceq $schema }
        if (-not $key) { continue }

        $objects = $database[$key]
        foreach ($type in 'Tables', 'Views', 'Procedures', 'Functions') {
            foreach ($name in $objects[$type]) { "$type $key.$name" }
        }
    }
    return @($expected | Sort-Object)
}

function Get-ScaffoldedObjects([string] $modelsDir) {
    $code = (Get-ChildItem $modelsDir -Filter *.cs | Get-Content -Raw) -join "`n"
    $found = @(
        # dbo is the default schema, so EF Core may leave it out of ToTable/ToView.
        # ToTable can take a third argument, such as a table comment.
        [regex]::Matches($code, 'ToTable\("(\w+)"(?:, "(\w+)")?[,)]') | ForEach-Object {
            $schema = if ($_.Groups[2].Success) { $_.Groups[2].Value } else { 'dbo' }
            "Tables $schema.$($_.Groups[1].Value)"
        }
        [regex]::Matches($code, 'ToView\("(\w+)"(?:, "(\w+)")?[,)]') | ForEach-Object {
            $schema = if ($_.Groups[2].Success) { $_.Groups[2].Value } else { 'dbo' }
            "Views $schema.$($_.Groups[1].Value)"
        }
        [regex]::Matches($code, 'EXEC @returnValue = \[(\w+)\]\.\[(\w+)\]') | ForEach-Object {
            "Procedures $($_.Groups[1].Value).$($_.Groups[2].Value)"
        }
        [regex]::Matches($code, 'DbFunction\("(\w+)", "(\w+)"\)') | ForEach-Object {
            "Functions $($_.Groups[2].Value).$($_.Groups[1].Value)"
        }
    )
    return @($found | Sort-Object -Unique)
}

function Get-ConfiguredObjects([string] $configPath) {
    $config = Get-Content $configPath -Raw | ConvertFrom-Json
    $found = foreach ($type in $sections.Keys) {
        foreach ($entry in $config.($sections[$type])) {
            if ($entry.name -match '^\[(\w+)\]\.\[(\w+)\]$') { "$type $($Matches[1]).$($Matches[2])" }
        }
    }
    return @($found | Where-Object { $_ } | Sort-Object -Unique)
}

function New-ObjectLists([string[]] $extras) {
    $lists = [ordered]@{}
    foreach ($extra in $extras) {
        $type, $schema, $name = $extra -split '[ .]'
        $section = $sections[$type]
        if (-not $lists.Contains($section)) {
            $lists[$section] = [System.Collections.Generic.List[object]]::new()
        }

        $lists[$section].Add([ordered]@{ name = "[$schema].[$name]"; exclude = $false })
    }
    return $lists
}

function Test-Objects([string] $what, [string[]] $expected, [string[]] $actual) {
    # A function returning an empty array returns $null, so drop empty entries.
    $expected = @($expected | Where-Object { $_ })
    $actual = @($actual | Where-Object { $_ })
    $missing = @($expected | Where-Object { $_ -notin $actual })
    $unexpected = @($actual | Where-Object { $_ -notin $expected })

    if ($missing.Count -eq 0 -and $unexpected.Count -eq 0) {
        $result = if ($actual.Count -eq 0) { 'no objects, as expected' } else { "the $($actual.Count) expected objects" }
        Write-Host "    PASS: $what has $result" -ForegroundColor Green
        return $true
    }

    Write-Host "    FAIL: $what" -ForegroundColor Red
    $missing | ForEach-Object { Write-Host "      missing:    $_" -ForegroundColor Red }
    $unexpected | ForEach-Object { Write-Host "      unexpected: $_" -ForegroundColor Red }
    return $false
}

if (-not $SkipBuild) {
    # The SQL project folder has a global.json that pins an SDK MSBuild.Sdk.SqlProj supports.
    Push-Location $PSScriptRoot
    try {
        Invoke-Checked 'Build AdventureWorks dacpac' { dotnet build $sqlProject }
    }
    finally {
        Pop-Location
    }

    Invoke-Checked 'Build efcpt' { dotnet build $efcptProject }
}

foreach ($file in $dacpac, $efcpt) {
    if (-not (Test-Path $file)) { throw "Missing $file, run without -SkipBuild" }
}

$workRoot = Join-Path $PSScriptRoot 'output'
if (Test-Path $workRoot) {
    Remove-Item $workRoot -Recurse -Force
}

$failures = 0

foreach ($scenario in $scenarios) {
    $outputDir = Join-Path $workRoot ($scenario.Name -replace '\W+', '-')
    New-Item -ItemType Directory -Path $outputDir | Out-Null

    $config = [ordered]@{
        'code-generation' = @{ 'refresh-object-lists' = -not $scenario.NoRefresh }
        'names'           = @{ 'dbcontext-name' = 'AdventureWorksContext'; 'root-namespace' = 'SchemaTest' }
        'schemas'         = @($scenario.Schemas)
    }
    $extras = @($scenario.Extras | Where-Object { $_ })
    $lists = New-ObjectLists $extras
    foreach ($section in $lists.Keys) { $config[$section] = $lists[$section] }

    $configPath = Join-Path $outputDir 'efcpt-config.json'
    $config | ConvertTo-Json -Depth 5 | Set-Content $configPath -Encoding utf8

    Invoke-Checked "Scaffold '$($scenario.Name)' [$($scenario.Schemas -join ', ')]" {
        dotnet $efcpt $dacpac mssql -i $configPath -o $outputDir --no-readme
    }

    $expected = @(Get-ExpectedObjects $scenario.Schemas) + $extras | Sort-Object
    $codeOk = Test-Objects 'generated code' $expected (Get-ScaffoldedObjects (Join-Path $outputDir 'Models'))

    # efcpt must not add objects to the config file, so it only lists the extras written above.
    $configOk = Test-Objects 'efcpt-config.json' $extras (Get-ConfiguredObjects $configPath)

    if (-not ($codeOk -and $configOk)) {
        $failures++
    }
}

Write-Host "Generated code is in $workRoot"

if ($failures -gt 0) {
    Write-Host "$failures of $($scenarios.Count) scenarios failed" -ForegroundColor Red
    exit 1
}

Write-Host "All $($scenarios.Count) scenarios passed" -ForegroundColor Green
