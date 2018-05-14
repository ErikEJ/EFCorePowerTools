using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace HandlebarsDotNet.Compiler
{
    internal class HashParametersExpression : HandlebarsExpression
    {
        public Dictionary<string, object> Parameters { get; set; }

        public HashParametersExpression(Dictionary<string, object> parameters)
        {
            Parameters = parameters;
        }

        public override ExpressionType NodeType
        {
            get { return (ExpressionType)HandlebarsExpressionType.HashParametersExpression; }
        }

        public override Type Type
        {
            get { return typeof(object); }
        }
    }
}

