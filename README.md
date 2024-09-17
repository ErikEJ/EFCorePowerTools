# EF Core Power Tools

Reverse engineering and model visualization tools for EF Core in Visual Studio 2022 - and reverse engineering from command line.

Aims to lower the bar for getting started with EF Core, by providing GUI based assistance with reverse engineering of an existing database and visualizing your DbContext model.

[![Visual Studio Marketplace Rating](https://img.shields.io/visual-studio-marketplace/r/ErikEJ.EFCorePowerTools)](https://marketplace.visualstudio.com/items?itemName=ErikEJ.EFCorePowerTools&ssr=false#review-details)
[![Visual Studio Marketplace Downloads](https://img.shields.io/visual-studio-marketplace/i/ErikEJ.EFCorePowerTools)](https://marketplace.visualstudio.com/items?itemName=ErikEJ.EFCorePowerTools&ssr=false#review-details)
[![Twitter Follow](https://img.shields.io/twitter/follow/ErikEJ.svg?style=social&label=Follow)](https://twitter.com/ErikEJ) 

[Quick Start and 10 minute intro video](https://github.com/ErikEJ/EFCorePowerTools/wiki/Reverse-Engineering-Quick-Start)

[Demo video - Introduction](https://youtu.be/uph-AGyOd8c)

[Demo video - Advanced features](https://youtu.be/3-Izu_qLDqY)

[EF Core Power Tools presentation](https://erikej.github.io/EFCorePowerTools/index.html)

[My tools and utilities for embedded database development](https://erikej.github.io/SqlCeToolbox/)

The tool and GitHub based support is free, but I would be very grateful for a [rating or review here](https://marketplace.visualstudio.com/items?itemName=ErikEJ.EFCorePowerTools&ssr=false#review-details), and I also appreciate [sponsorships](https://github.com/sponsors/ErikEJ)

# Documentation

[Reverse Engineering Quick Start](https://github.com/ErikEJ/EFCorePowerTools/wiki/Reverse-Engineering-Quick-Start)

[User guide](https://github.com/ErikEJ/EFCorePowerTools/wiki)

[Release notes](https://github.com/ErikEJ/EFCorePowerTools/wiki/Release-notes)

# Downloads/builds

## Requirements 

.NET Framework 4.8 or later is required. For EF Core reverse engineering, the .NET 6.0 or .NET 8.0 x64 runtime must be installed.

## Release

Download the latest version of the Visual Studio extension from [Visual Studio MarketPlace](https://marketplace.visualstudio.com/items?itemName=ErikEJ.EFCorePowerTools)

Or simply install from the Extensions dialog in Visual Studio.

I have also published [EF Core Power Pack](https://marketplace.visualstudio.com/items?itemName=ErikEJ.EFCorePowerPack) which includes some helpful DDEX providers as well as EF Core Power Tools.

## Daily build

You can download the daily build from [Open VSIX Gallery](https://www.vsixgallery.com/extension/f4c4712c-ceae-4803-8e52-0e2049d5de9f)

Ensure you always have the latest daily build (if you are brave) by installing [this extension](https://marketplace.visualstudio.com/items?itemName=MadsKristensen.VSIXGallery-nightlybuilds)

## EF Core Power Tools CLI - efcpt

If you do not use Visual Studio, but for example Visual Studio Code, a cross platform dotnet tool for reverse engineering is available, more information [here](https://github.com/ErikEJ/EFCorePowerTools/blob/master/src/Core/efcpt.8/readme.md)

## Related NuGet packages

[ErikEJ.EntityFrameworkCore.DgmlBuilder](https://github.com/ErikEJ/EFCorePowerTools/blob/master/src/Nupkg/ErikEJ.EntityFrameworkCore.DgmlBuilder/readme.md)

Adds the AsDgml() extension method to any derived DbContext. The method will create a DGML graph of your DbContext Model, that you can then view in the Visual Studio DGML viewer

[ErikEJ.EntityFrameworkCore.SqlServer.Dacpac](https://github.com/ErikEJ/EFCorePowerTools/blob/master/src/Nupkg/ErikEJ.EntityFrameworkCore.SqlServer.Dacpac/readme.md)

Reverse engineer a SQL Server .dacpac with the EF Core tooling

[ErikEJ.EntityFrameworkCore.SqlServer.SqlQuery](https://github.com/ErikEJ/EFCorePowerTools/blob/master/src/Nupkg/ErikEJ.EntityFrameworkCore.6.SqlServer.SqlQuery/readme.md)

Materialize abritary classes and scalar values from EF Core using raw SQL

# How do I contribute

If you encounter a bug or have a feature request, please use the [Issue Tracker](https://github.com/ErikEJ/EFCorePowerTools/issues/new). The project is also open for pull requests following [standard pull request guidelines](https://github.com/dotnet/aspnetcore/blob/master/CONTRIBUTING.md)

# Building and debugging

To build and debug, run latest version of Visual Studio as Administrator with the "Visual Studio extension development workload" installed. Make EFCorePowerTools the startup project, and ensure your build configuration is Debug, AnyCPU.

You can smoke test changes to reverse engineering. Launch one of the efreveng console apps in the debugger using the name of a file in the TestFiles folder as parameter.

In the ScaffoldingTester solution there are scripts to populate Northwind and Chinook databases with the required objects.

# Sponsors

A massive thanks to [AWS](https://github.com/aws), who sponsors EFCorePowerTools from January 2024 via the [.NET on AWS Open Source Software Fund](https://github.com/aws/dotnet-foss).

<div style="display:inline">
<img src="https://raw.githubusercontent.com/ErikEJ/EFCorePowerTools/master/img/aws-logo-small.png" width="200" height="200"/>
</div>

And a huge thanks to everyone who sponsors this project through [Github sponsors](https://github.com/sponsors/ErikEJ).
