# EF Core Power Tools

Reverse engineering and model visualization tools for EF Core in Visual Studio 2022.

The beta process aims to improve the developer UX

[Main README.md](README.md)

# Reverse Engineering Wizard
The following video demonstrates the wizard at work.  Don't be distracted by its current design as XAML can be easily configured 
to look as required.  At this point, it is a POC to demonstrate that a wizard can be used within the existing EFCorePowerTools 
project.  The code for the wizard comes from the [Microsoft WPF-Samples](https://github.com/microsoft/WPF-Samples) project [Windows/Wizard folder] which was slightly refactored for our purposes.

Where the patterns and design are subject to discussion and approval, the proposed path would be to use the Model-View-Presenter, View-Model [MPV-VM] pattern which is described below.  However, unlike true MVP-VM, the view's code-behind logic may remain intact to minimize the refactor effort; with MVP-VM the view does not contain business logic.

https://github.com/user-attachments/assets/8a4cd041-92eb-4a54-baf2-ae7ce30055c1

The sequence diagram below is a high level view of the wizard's primary classes.  Note that the location of each
class within the solution is located below each timeline box (in gray), e.g., RegEngWizardhandler.cs is located 
under the solution's Shared / Handlers / Wizard folder.
<img src="img/mvpvm-wizard.png"/>
Figure 1 

# MVP-VM
The pattern suggested for the proposed work is the Model-View-Presenter, View-Model pattern. The current design lends 
itself to this because it isn't a true MVVM pattern; unlike MVVM, the existing handler performs much of the business logic 
invoking business logic within each of the view's code-behind classes, thus effectively overcoming some MVVM limitations.
Traditional MVVM is more a "widget" pattern that mirrors the MVC pattern (adapted for WPF).  With MVVM, the view and view 
model contain all of the logic and as such cannot be easily reused because of the tight coupling between the view, view model, 
and its logic layers. 

Note: MVC evolved to the [Model-View-Presenter](https://www.wildcrest.com/Potel/Portfolio/mvp.pdf) pattern to overcome such 
limitations. MVP-VM (like MVP) is more of a framework pattern which will required for our use case.

## Presenter (RevEngWizardHandler)
The presenter has primary responsibility for communicating with the business logic layer to populate the view model(s).
This pattern allows the views and view models to remain decoupled (lending to easier reuse and unit testing); Views
and view models do not access the business and/or data layers directly.
## Model 
Business and data access layers compose the model (in the MVC days it might have been referred to as application model).  
The presenter will use the business logic layer to populate the view models as required.   Under MVP-VM, the business 
logic layer is the only component that communicate with the data access layer.
## Views / View Models
The view reflects the data that is on its view model. Outside of the UI behavior logic, there is generally no business 
logic within the view code-behind using MVP-VM. Under MVP-VM it is easy to reuse views and view-models because of this decoupling. 

Note: for more indepth information on MVP-VM you can read my MSDN 2011 article [MVP-VM Design Pattern for WPF](https://learn.microsoft.com/en-us/archive/msdn-magazine/2011/december/mvpvm-design-pattern-the-model-view-presenter-viewmodel-design-pattern-for-wpf) 

## Proposed design
The below use case diagram reflects the relationships between the existing, and new, components.

<img src="img/mvpvm-uml.png"/>
Figure 2

# High level UML for current design 
These diagrams are not all inclusive, but will provide a high level view of applicable classes and primary processes invoked.
## Page 1 - Choose Your Data Connection
<img src="img/mvpvm-pg1.png"/>
Figure 3

## Page 2 - Choose Your Database Objects
<img src="img/mvpvm-pg2.png"/>
Figure 4

## Page 3 - Choose Your Settings For Project
<img src="img/mvpvm-pg3.png"/>
Figure 5

