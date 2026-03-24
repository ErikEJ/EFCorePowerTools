# PR Ready Notes: ARM64 Build Failure in `EFCorePowerTools`

## Copy/Paste PR Summary

On Windows ARM64 with Visual Studio 2026 Insiders, `GUI/EFCorePowerTools/EFCorePowerTools.csproj` can fail to build because `Microsoft.VisualStudio.TemplateWizardInterface` is not resolved automatically for the legacy `Reference` item in the old-style project. The failure surfaces in `GUI/Shared/ItemWizard/ReverseEngineerWizardLauncher.cs` as missing `Microsoft.VisualStudio.TemplateWizard`, `IWizard`, and `WizardRunKind`.

The local workaround that successfully unblocked the build was to override the reference in `EFCorePowerTools.csproj.user` using the full assembly identity and a `HintPath` fallback chain covering current Visual Studio install layouts. No tracked `.csproj` change was required, and `<Content Include="EFCorePowerTools.csproj.user" />` was not part of the fix.

## Summary

On Windows ARM64 with Visual Studio 2026 Insiders, `GUI/EFCorePowerTools/EFCorePowerTools.csproj` can fail to compile because the `Microsoft.VisualStudio.TemplateWizardInterface` assembly is not resolved automatically.

The failure showed up in `GUI/Shared/ItemWizard/ReverseEngineerWizardLauncher.cs`, where these types were missing:

- `Microsoft.VisualStudio.TemplateWizard`
- `IWizard`
- `WizardRunKind`

This was not a source-code bug in `ReverseEngineerWizardLauncher.cs`. The issue was assembly resolution for the legacy `Reference` item in the old-style `.csproj`.

## Symptoms

Typical errors:

```text
error CS0234: The type or namespace name 'TemplateWizard' does not exist in the namespace 'Microsoft.VisualStudio'
error CS0246: The type or namespace name 'IWizard' could not be found
error CS0246: The type or namespace name 'WizardRunKind' could not be found
```

## Root Cause

`EFCorePowerTools.csproj` contains this legacy reference:

```xml
<Reference Include="Microsoft.VisualStudio.TemplateWizardInterface, Version=17.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a, processorArchitecture=MSIL" />
```

Visual Studio was already auto-importing `EFCorePowerTools.csproj.user`, but the original user file used:

```xml
<Reference Update="Microsoft.VisualStudio.TemplateWizardInterface">
```

For this old-style project, that `Update` value was not specific enough to reliably bind to the existing `Reference` item. Changing the `Update` identity to the full assembly identity allowed the `HintPath` override to apply correctly.

## Working Local Fix

The successful fix was to update `GUI/EFCorePowerTools/EFCorePowerTools.csproj.user` to use the full reference identity and provide a resilient `HintPath` lookup.

```xml
<?xml version="1.0" encoding="utf-8"?>
<Project xmlns="http://schemas.microsoft.com/developer/msbuild/2003">
  <PropertyGroup>
    <_TemplateWizardInterfaceDll Condition="Exists('$(VSToolsPath)\PublicAssemblies\Microsoft.VisualStudio.TemplateWizardInterface.dll')">$(VSToolsPath)\PublicAssemblies\Microsoft.VisualStudio.TemplateWizardInterface.dll</_TemplateWizardInterfaceDll>
    <_TemplateWizardInterfaceDll Condition="'$(_TemplateWizardInterfaceDll)' == '' and Exists('$(DevEnvDir)VSExtensions\Microsoft\AppModernizationForDotNet\Microsoft.VisualStudio.TemplateWizardInterface.dll')">$(DevEnvDir)VSExtensions\Microsoft\AppModernizationForDotNet\Microsoft.VisualStudio.TemplateWizardInterface.dll</_TemplateWizardInterfaceDll>
    <_TemplateWizardInterfaceDll Condition="'$(_TemplateWizardInterfaceDll)' == '' and Exists('$(VSINSTALLDIR)Common7\IDE\VSExtensions\Microsoft\AppModernizationForDotNet\Microsoft.VisualStudio.TemplateWizardInterface.dll')">$(VSINSTALLDIR)Common7\IDE\VSExtensions\Microsoft\AppModernizationForDotNet\Microsoft.VisualStudio.TemplateWizardInterface.dll</_TemplateWizardInterfaceDll>
    <_TemplateWizardInterfaceDll Condition="'$(_TemplateWizardInterfaceDll)' == '' and Exists('$(ProgramFiles)\Microsoft Visual Studio\18\Community\Common7\IDE\VSExtensions\Microsoft\AppModernizationForDotNet\Microsoft.VisualStudio.TemplateWizardInterface.dll')">$(ProgramFiles)\Microsoft Visual Studio\18\Community\Common7\IDE\VSExtensions\Microsoft\AppModernizationForDotNet\Microsoft.VisualStudio.TemplateWizardInterface.dll</_TemplateWizardInterfaceDll>
    <_TemplateWizardInterfaceDll Condition="'$(_TemplateWizardInterfaceDll)' == '' and Exists('$(ProgramFiles)\Microsoft Visual Studio\18\Professional\Common7\IDE\VSExtensions\Microsoft\AppModernizationForDotNet\Microsoft.VisualStudio.TemplateWizardInterface.dll')">$(ProgramFiles)\Microsoft Visual Studio\18\Professional\Common7\IDE\VSExtensions\Microsoft\AppModernizationForDotNet\Microsoft.VisualStudio.TemplateWizardInterface.dll</_TemplateWizardInterfaceDll>
    <_TemplateWizardInterfaceDll Condition="'$(_TemplateWizardInterfaceDll)' == '' and Exists('$(ProgramFiles)\Microsoft Visual Studio\18\Enterprise\Common7\IDE\VSExtensions\Microsoft\AppModernizationForDotNet\Microsoft.VisualStudio.TemplateWizardInterface.dll')">$(ProgramFiles)\Microsoft Visual Studio\18\Enterprise\Common7\IDE\VSExtensions\Microsoft\AppModernizationForDotNet\Microsoft.VisualStudio.TemplateWizardInterface.dll</_TemplateWizardInterfaceDll>
    <_TemplateWizardInterfaceDll Condition="'$(_TemplateWizardInterfaceDll)' == '' and Exists('$(ProgramFiles)\Microsoft Visual Studio\18\Preview\Common7\IDE\VSExtensions\Microsoft\AppModernizationForDotNet\Microsoft.VisualStudio.TemplateWizardInterface.dll')">$(ProgramFiles)\Microsoft Visual Studio\18\Preview\Common7\IDE\VSExtensions\Microsoft\AppModernizationForDotNet\Microsoft.VisualStudio.TemplateWizardInterface.dll</_TemplateWizardInterfaceDll>
    <_TemplateWizardInterfaceDll Condition="'$(_TemplateWizardInterfaceDll)' == '' and Exists('$(ProgramFiles)\Microsoft Visual Studio\18\Insiders\Common7\IDE\VSExtensions\Microsoft\AppModernizationForDotNet\Microsoft.VisualStudio.TemplateWizardInterface.dll')">$(ProgramFiles)\Microsoft Visual Studio\18\Insiders\Common7\IDE\VSExtensions\Microsoft\AppModernizationForDotNet\Microsoft.VisualStudio.TemplateWizardInterface.dll</_TemplateWizardInterfaceDll>
    <_TemplateWizardInterfaceDll Condition="'$(_TemplateWizardInterfaceDll)' == '' and Exists('$(ProgramFiles)\Microsoft Visual Studio\17\Community\Common7\IDE\VSExtensions\Microsoft\AppModernizationForDotNet\Microsoft.VisualStudio.TemplateWizardInterface.dll')">$(ProgramFiles)\Microsoft Visual Studio\17\Community\Common7\IDE\VSExtensions\Microsoft\AppModernizationForDotNet\Microsoft.VisualStudio.TemplateWizardInterface.dll</_TemplateWizardInterfaceDll>
    <_TemplateWizardInterfaceDll Condition="'$(_TemplateWizardInterfaceDll)' == '' and Exists('$(ProgramFiles)\Microsoft Visual Studio\17\Professional\Common7\IDE\VSExtensions\Microsoft\AppModernizationForDotNet\Microsoft.VisualStudio.TemplateWizardInterface.dll')">$(ProgramFiles)\Microsoft Visual Studio\17\Professional\Common7\IDE\VSExtensions\Microsoft\AppModernizationForDotNet\Microsoft.VisualStudio.TemplateWizardInterface.dll</_TemplateWizardInterfaceDll>
    <_TemplateWizardInterfaceDll Condition="'$(_TemplateWizardInterfaceDll)' == '' and Exists('$(ProgramFiles)\Microsoft Visual Studio\17\Enterprise\Common7\IDE\VSExtensions\Microsoft\AppModernizationForDotNet\Microsoft.VisualStudio.TemplateWizardInterface.dll')">$(ProgramFiles)\Microsoft Visual Studio\17\Enterprise\Common7\IDE\VSExtensions\Microsoft\AppModernizationForDotNet\Microsoft.VisualStudio.TemplateWizardInterface.dll</_TemplateWizardInterfaceDll>
    <_TemplateWizardInterfaceDll Condition="'$(_TemplateWizardInterfaceDll)' == '' and Exists('$(ProgramFiles)\Microsoft Visual Studio\17\Preview\Common7\IDE\VSExtensions\Microsoft\AppModernizationForDotNet\Microsoft.VisualStudio.TemplateWizardInterface.dll')">$(ProgramFiles)\Microsoft Visual Studio\17\Preview\Common7\IDE\VSExtensions\Microsoft\AppModernizationForDotNet\Microsoft.VisualStudio.TemplateWizardInterface.dll</_TemplateWizardInterfaceDll>
    <_TemplateWizardInterfaceDll Condition="'$(_TemplateWizardInterfaceDll)' == '' and Exists('$(ProgramFiles(x86))\Microsoft Visual Studio\18\Community\Common7\IDE\VSExtensions\Microsoft\AppModernizationForDotNet\Microsoft.VisualStudio.TemplateWizardInterface.dll')">$(ProgramFiles(x86))\Microsoft Visual Studio\18\Community\Common7\IDE\VSExtensions\Microsoft\AppModernizationForDotNet\Microsoft.VisualStudio.TemplateWizardInterface.dll</_TemplateWizardInterfaceDll>
    <_TemplateWizardInterfaceDll Condition="'$(_TemplateWizardInterfaceDll)' == '' and Exists('$(ProgramFiles(x86))\Microsoft Visual Studio\18\Professional\Common7\IDE\VSExtensions\Microsoft\AppModernizationForDotNet\Microsoft.VisualStudio.TemplateWizardInterface.dll')">$(ProgramFiles(x86))\Microsoft Visual Studio\18\Professional\Common7\IDE\VSExtensions\Microsoft\AppModernizationForDotNet\Microsoft.VisualStudio.TemplateWizardInterface.dll</_TemplateWizardInterfaceDll>
    <_TemplateWizardInterfaceDll Condition="'$(_TemplateWizardInterfaceDll)' == '' and Exists('$(ProgramFiles(x86))\Microsoft Visual Studio\18\Enterprise\Common7\IDE\VSExtensions\Microsoft\AppModernizationForDotNet\Microsoft.VisualStudio.TemplateWizardInterface.dll')">$(ProgramFiles(x86))\Microsoft Visual Studio\18\Enterprise\Common7\IDE\VSExtensions\Microsoft\AppModernizationForDotNet\Microsoft.VisualStudio.TemplateWizardInterface.dll</_TemplateWizardInterfaceDll>
    <_TemplateWizardInterfaceDll Condition="'$(_TemplateWizardInterfaceDll)' == '' and Exists('$(ProgramFiles(x86))\Microsoft Visual Studio\18\Preview\Common7\IDE\VSExtensions\Microsoft\AppModernizationForDotNet\Microsoft.VisualStudio.TemplateWizardInterface.dll')">$(ProgramFiles(x86))\Microsoft Visual Studio\18\Preview\Common7\IDE\VSExtensions\Microsoft\AppModernizationForDotNet\Microsoft.VisualStudio.TemplateWizardInterface.dll</_TemplateWizardInterfaceDll>
    <_TemplateWizardInterfaceDll Condition="'$(_TemplateWizardInterfaceDll)' == '' and Exists('$(ProgramFiles(x86))\Microsoft Visual Studio\18\Insiders\Common7\IDE\VSExtensions\Microsoft\AppModernizationForDotNet\Microsoft.VisualStudio.TemplateWizardInterface.dll')">$(ProgramFiles(x86))\Microsoft Visual Studio\18\Insiders\Common7\IDE\VSExtensions\Microsoft\AppModernizationForDotNet\Microsoft.VisualStudio.TemplateWizardInterface.dll</_TemplateWizardInterfaceDll>
    <_TemplateWizardInterfaceDll Condition="'$(_TemplateWizardInterfaceDll)' == '' and Exists('$(ProgramFiles(x86))\Microsoft Visual Studio\17\Community\Common7\IDE\VSExtensions\Microsoft\AppModernizationForDotNet\Microsoft.VisualStudio.TemplateWizardInterface.dll')">$(ProgramFiles(x86))\Microsoft Visual Studio\17\Community\Common7\IDE\VSExtensions\Microsoft\AppModernizationForDotNet\Microsoft.VisualStudio.TemplateWizardInterface.dll</_TemplateWizardInterfaceDll>
    <_TemplateWizardInterfaceDll Condition="'$(_TemplateWizardInterfaceDll)' == '' and Exists('$(ProgramFiles(x86))\Microsoft Visual Studio\17\Professional\Common7\IDE\VSExtensions\Microsoft\AppModernizationForDotNet\Microsoft.VisualStudio.TemplateWizardInterface.dll')">$(ProgramFiles(x86))\Microsoft Visual Studio\17\Professional\Common7\IDE\VSExtensions\Microsoft\AppModernizationForDotNet\Microsoft.VisualStudio.TemplateWizardInterface.dll</_TemplateWizardInterfaceDll>
    <_TemplateWizardInterfaceDll Condition="'$(_TemplateWizardInterfaceDll)' == '' and Exists('$(ProgramFiles(x86))\Microsoft Visual Studio\17\Enterprise\Common7\IDE\VSExtensions\Microsoft\AppModernizationForDotNet\Microsoft.VisualStudio.TemplateWizardInterface.dll')">$(ProgramFiles(x86))\Microsoft Visual Studio\17\Enterprise\Common7\IDE\VSExtensions\Microsoft\AppModernizationForDotNet\Microsoft.VisualStudio.TemplateWizardInterface.dll</_TemplateWizardInterfaceDll>
    <_TemplateWizardInterfaceDll Condition="'$(_TemplateWizardInterfaceDll)' == '' and Exists('$(ProgramFiles(x86))\Microsoft Visual Studio\17\Preview\Common7\IDE\VSExtensions\Microsoft\AppModernizationForDotNet\Microsoft.VisualStudio.TemplateWizardInterface.dll')">$(ProgramFiles(x86))\Microsoft Visual Studio\17\Preview\Common7\IDE\VSExtensions\Microsoft\AppModernizationForDotNet\Microsoft.VisualStudio.TemplateWizardInterface.dll</_TemplateWizardInterfaceDll>
  </PropertyGroup>
  <ItemGroup>
    <Reference Update="Microsoft.VisualStudio.TemplateWizardInterface, Version=17.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a, processorArchitecture=MSIL">
      <HintPath Condition="'$(_TemplateWizardInterfaceDll)' != ''">$(_TemplateWizardInterfaceDll)</HintPath>
      <Private>false</Private>
      <SpecificVersion>true</SpecificVersion>
    </Reference>
  </ItemGroup>
</Project>
```

## Important Notes

- The fix did **not** require modifying the tracked `.csproj` file.
- The fix did **not** require changing:

```xml
<Content Include="EFCorePowerTools.csproj.user" />
```

- That `Content` entry only controls project visibility. It is not what made the build fix work.
- The successful change was inside `EFCorePowerTools.csproj.user`.
- A temporary `Directory.Build.targets` shim was tested during debugging, but it was ultimately unnecessary because Visual Studio was already auto-importing the `.user` file.

## Why This Matters for ARM Developers

This issue is easy to misdiagnose as:

- an ARM compiler problem
- a Visual Studio 2026 regression
- a broken source file
- a missing NuGet package

It was actually a legacy MSBuild reference-resolution problem that only surfaced because the expected Visual Studio assembly path was different on the local machine.

## Maintainer Follow-Up

If this scenario should be supported in-repo rather than as a local workaround, the project may need a more explicit built-in `HintPath` strategy for `Microsoft.VisualStudio.TemplateWizardInterface`, or a different approach to resolving that assembly across current Visual Studio install layouts.
