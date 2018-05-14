using System;
using System.Linq.Expressions;
using System.Collections.Generic;
using System.Linq;

namespace HandlebarsDotNet.Compiler
{
    internal class HelperExpression : HandlebarsExpression
    {
        private readonly IEnumerable<Expression> _arguments;
        private readonly string _helperName;

        public HelperExpression(string helperName, IEnumerable<Expression> arguments)
            : this(helperName)
        {
            _arguments = arguments;
        }

        public HelperExpression(string helperName)
        {
            _helperName = helperName;
            _arguments = Enumerable.Empty<Expression>();
        }

        public override ExpressionType NodeType
        {
            get { return (ExpressionType)HandlebarsExpressionType.HelperExpression; }
        }

        public override Type Type
        {
            get { return typeof(void); }
        }

        public string HelperName
        {
            get { return _helperName; }
        }

        public IEnumerable<Expression> Arguments
        {
            get { return _arguments; }
        }
    }
}

