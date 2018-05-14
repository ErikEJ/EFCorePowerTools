using System;
using System.Linq;
using System.Linq.Expressions;
using System.Collections.Generic;

namespace HandlebarsDotNet.Compiler
{
    internal class PartialBlockAccumulatorContext : BlockAccumulatorContext
    {
        private readonly PartialExpression _startingNode;
        private string _blockName;
        private List<Expression> _body = new List<Expression>();

        public PartialBlockAccumulatorContext(Expression startingNode)
            : base(startingNode)
        {
            _startingNode = ConvertToPartialExpression(UnwrapStatement(startingNode));
        }

        public override void HandleElement(Expression item)
        {
            _body.Add((Expression)item);
        }

        public override Expression GetAccumulatedBlock()
        {
            return HandlebarsExpression.Partial(
                _startingNode.PartialName,
                _startingNode.Argument,
                _body.Count > 1 ? Expression.Block(_body) : _body.First());
        }

        public override bool IsClosingElement(Expression item)
        {
            item = UnwrapStatement(item);
            return IsClosingNode(item);
        }

        private bool IsClosingNode(Expression item)
        {
            return item is PathExpression && ((PathExpression)item).Path == "/" + _blockName;
        }

        private PartialExpression ConvertToPartialExpression(Expression expression)
        {
            if (expression is PathExpression)
            {
                var pathExpression = (PathExpression)expression;
                _blockName = pathExpression.Path.Replace ("#>", "");
                return HandlebarsExpression.Partial(Expression.Constant(_blockName));
            } 
            else if (expression is HelperExpression)
            {
                var helperExpression = (HelperExpression)expression;
                _blockName = helperExpression.HelperName.Replace ("#>", "");
                if (helperExpression.Arguments.Count() == 0)
                {
                    return HandlebarsExpression.Partial(Expression.Constant(_blockName));
                }
                else if (helperExpression.Arguments.Count() == 1)
                {
                    return HandlebarsExpression.Partial(
                        Expression.Constant(_blockName),
                        helperExpression.Arguments.First());
                }
                else
                {
                    throw new InvalidOperationException("Cannot convert a multi-argument helper expression to a partial expression");
                }
            }
            else
            {
                throw new InvalidOperationException(string.Format("Cannot convert '{0}' to a partial expression", expression));
            }
        }
    }
}

