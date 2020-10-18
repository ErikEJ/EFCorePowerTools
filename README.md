# EF Core Power Tools

Reverse engineering, model visualization and migrations UI for EF Core.

Hopes to lower the bar for getting started with EF Core, by providing GUI based assistance with reverse engineering of an existing database, creating migrations and visualizing your DbContext model.

[![Visual Studio Marketplace Version](https://vsmarketplacebadge.apphb.com/version/ErikEJ.EFCorePowerTools.svg)](https://marketplace.visualstudio.com/items?itemName=ErikEJ.EFCorePowerTools) [![Twitter Follow](https://img.shields.io/twitter/follow/ErikEJ.svg?style=social&label=Follow)](https://twitter.com/ErikEJ) 

[![Demo at EF Core Community Standup](https://img.youtube.com/vi/OWuP_qOYwsk/1.jpg)](https://www.youtube.com/watch?v=OWuP_qOYwsk "Demo")

[EF Core Power Tools presentation](https://erikej.github.io/EFCorePowerTools/index.html)

[My tools and utilities for embedded database development](https://erikej.github.io/SqlCeToolbox/)

If you use my free tools, I would be very grateful for a [rating or review here](https://marketplace.visualstudio.com/items?itemName=ErikEJ.EFCorePowerTools#review-details)

# Documentation

[Getting started and user guide](https://github.com/ErikEJ/EFCorePowerTools/wiki)

# Downloads/builds

**Requiremnts** 

.NET Framework 4.7.1 or later is required. For EF Core reverse engineering, .NET Core 3.1 x64 runtime must be installed.

**Release**

Download the latest version of the Visual Studio extension from [Visual Studio MarketPlace](https://marketplace.visualstudio.com/items?itemName=ErikEJ.EFCorePowerTools)

Or just install from Tools, Extensions and Updates in Visual Studio! ![](https://github.com/ErikEJ/SqlCeToolbox/blob/master/img/ext.png)

**Daily build**

You can download the daily build from [Open VSIX Gallery](https://www.vsixgallery.com/extension/f4c4712c-ceae-4803-8e52-0e2049d5de9f)

Ensure you always have the latest daily build (if you are brave) by installing [this extension](https://marketplace.visualstudio.com/items?itemName=MadsKristensen.VSIXGallery-nightlybuilds)

# How do I contribute

If you encounter a bug or have a feature request, please use the [Issue Tracker](https://github.com/ErikEJ/EFCorePowerTools/issues/new). The project is also open for pull requests folliwng standard pull request guidelines

# Building and debugging

To build and debug, run latest version of Visual Studio with the "Visual Studio extension development workload" installed - as Administrator. Make EFCorePowerTools the startup project, and ensure your build configuration is Debug, x86 (not AnyCPU).

