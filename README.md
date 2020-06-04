# EF Core Power Tools

Reverse engineering, model visualization and migrations UI for EF Core.

Hopes to lower the bar for getting started with EF Core, by providing GUI based assistance with reverse engineering of an existing database, creating migrations and visualizing your DbContext model.

[![Gitter](https://badges.gitter.im/EFCorePowerTools/community.svg)](https://gitter.im/EFCorePowerTools/community?utm_source=badge&utm_medium=badge&utm_campaign=pr-badge)
[![Visual Studio Marketplace Version](https://vsmarketplacebadge.apphb.com/version/ErikEJ.EFCorePowerTools.svg)](https://marketplace.visualstudio.com/items?itemName=ErikEJ.EFCorePowerTools)

[![Twitter Follow](https://img.shields.io/twitter/follow/ErikEJ.svg?style=social&label=Follow)](https://twitter.com/ErikEJ) 

[EF Core Power Tools presentation](https://erikej.github.io/EFCorePowerTools/index.html)

[My tools and utilities for embedded database development](https://erikej.github.io/SqlCeToolbox/)

If you use my free tools, I would be very grateful for a [rating or review here](https://marketplace.visualstudio.com/items?itemName=ErikEJ.EFCorePowerTools#review-details)

# Documentation

[Getting started](https://github.com/ErikEJ/EFCorePowerTools/wiki)

# Downloads/builds

The EF Core Power Tools contains DbContext design time feature: Reverse Engineering, Model inspection and more to come - see the [wiki page](https://github.com/ErikEJ/EFCorePowerTools/wiki) for more information. .NET Framework 4.7.1 or later is required. For EF Core 3 reverse engineering, .NET Core 3 must be installed.

**Release**

Download the latest version of the Visual Studio extension from [Visual Studio MarketPlace](https://marketplace.visualstudio.com/items?itemName=ErikEJ.EFCorePowerTools)

Or just install from Tools, Extensions and Updates in Visual Studio! ![](https://github.com/ErikEJ/SqlCeToolbox/blob/master/img/ext.png)

**Daily build**

You can download the daily build from [VSIX Gallery](https://vsixgallery.com/extensions/f4c4712c-ceae-4803-8e52-0e2049d5de9f/extension.vsix)

Ensure you always have the latest daily build (if you are brave) by installing [this extension](https://marketplace.visualstudio.com/items?itemName=MadsKristensen.VSIXGallery-nightlybuilds)

# How do I contribute

If you encounter a bug or have a feature request, please use the [Issue Tracker](https://github.com/ErikEJ/EFCorePowerTools/issues/new)

# Building and debugging

To build and debug, run latest version of Visual Studio with the "Visual Studio extension development workload" installed - as Administrator. Make EFCorePowerTools the startup project, and ensure your build configuration is Debug, x86 (not AnyCPU).

