** ErikEJ.EntityFrameworkCore.DgmlBuilder Readme **

To use the extension method to generate a DGML file of your DbContext model,
use code similar to this:
    
	using Microsoft.EntityFrameworkCore;
 

    using (var myContext = new MyDbContext())
    {
        System.IO.File.WriteAllText(System.IO.Path.GetTempFileName() + ".dgml", 
			myContext.AsDgml(), 
			System.Text.Encoding.UTF8);
    }
