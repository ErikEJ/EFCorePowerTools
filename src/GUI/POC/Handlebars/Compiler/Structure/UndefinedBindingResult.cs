using System;
using System.Diagnostics;

namespace HandlebarsDotNet.Compiler
{
    [DebuggerDisplay("undefined")]
    internal class UndefinedBindingResult
    {
	    public readonly string Value;
	    private readonly HandlebarsConfiguration _configuration;

	    public UndefinedBindingResult(string value, HandlebarsConfiguration configuration)
	    {
		    Value = value;
		    _configuration = configuration;
	    }

        public override string ToString()
        {
	        var formatter = _configuration.UnresolvedBindingFormatter ?? string.Empty;
	        return string.Format( formatter, Value );
        }
    }
}

