using System;
using System.Linq.Expressions;

namespace HandlebarsDotNet.Compiler
{
    internal class PathExpression : HandlebarsExpression
    {
        private readonly string _path;

        public PathExpression(string path)
        {
            _path = path;
        }

        public string Path
        {
            get { return _path; }
        }

        public override ExpressionType NodeType
        {
            get { return (ExpressionType)HandlebarsExpressionType.PathExpression; }
        }

        public override Type Type
        {
            get { return typeof(object); }
        }
    }
}

