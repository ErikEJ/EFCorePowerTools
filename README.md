# EF Core Power Tools

Reverse engineering, model visualization and migrations UI for EF Core. For Visual Studio 2019.

Aims to lower the bar for getting started with EF Core, by providing GUI based assistance with reverse engineering of an existing database, creating migrations and visualizing your DbContext model.

[![Visual Studio Marketplace Version](https://vsmarketplacebadge.apphb.com/version/ErikEJ.EFCorePowerTools.svg)](https://marketplace.visualstudio.com/items?itemName=ErikEJ.EFCorePowerTools) [![Twitter Follow](https://img.shields.io/twitter/follow/ErikEJ.svg?style=social&label=Follow)](https://twitter.com/ErikEJ) 

Demo video - Introduction

[![Demo at .NET Conf](https://img.youtube.com/vi/uph-AGyOd8c/2.jpg)](https://youtu.be/uph-AGyOd8c "Demo")

Demo video - Advanced features

[![Demo at EF Core Community Standup](https://img.youtube.com/vi/3-Izu_qLDqY/1.jpg)](https://youtu.be/3-Izu_qLDqY "Demo")

[EF Core Power Tools presentation](https://erikej.github.io/EFCorePowerTools/index.html)

[My tools and utilities for embedded database development](https://erikej.github.io/SqlCeToolbox/)

If you use my free tools, I would be very grateful for a [rating or review here](https://marketplace.visualstudio.com/items?itemName=ErikEJ.EFCorePowerTools#review-details)

# Documentation

[Getting started and user guide](https://github.com/ErikEJ/EFCorePowerTools/wiki)

[Release notes](https://github.com/ErikEJ/EFCorePowerTools/wiki/Release-notes)

# Downloads/builds

## Requirements 

.NET Framework 4.7.1 or later is required. For EF Core reverse engineering, .NET Core 3.1 x64 runtime must be installed.

## Release

Download the latest version of the Visual Studio extension from [Visual Studio MarketPlace](https://marketplace.visualstudio.com/items?itemName=ErikEJ.EFCorePowerTools)

Or just install from the Extensions dialog in Visual Studio.

## Daily build

You can download the daily build from [Open VSIX Gallery](https://www.vsixgallery.com/extension/f4c4712c-ceae-4803-8e52-0e2049d5de9f)

Ensure you always have the latest daily build (if you are brave) by installing [this extension](https://marketplace.visualstudio.com/items?itemName=MadsKristensen.VSIXGallery-nightlybuilds)

# How do I contribute

If you encounter a bug or have a feature request, please use the [Issue Tracker](https://github.com/ErikEJ/EFCorePowerTools/issues/new). The project is also open for pull requests following [standard pull request guidelines](https://github.com/dotnet/aspnetcore/blob/master/CONTRIBUTING.md#identifying-the-scale)

# Building and debugging

To build and debug, run latest version of Visual Studio as Administrator with the "Visual Studio extension development workload" installed. Make EFCorePowerTools the startup project, and ensure your build configuration is Debug, x86 (not AnyCPU).
