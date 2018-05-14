using System;
using System.Linq;
using System.Linq.Expressions;
using System.Collections.Generic;

namespace HandlebarsDotNet.Compiler
{
    internal class IteratorBlockAccumulatorContext : BlockAccumulatorContext
    {
        private readonly HelperExpression _startingNode;
        private Expression _accumulatedExpression;
        private List<Expression> _body = new List<Expression>();

        public IteratorBlockAccumulatorContext(Expression startingNode)
            : base(startingNode)
        {
            startingNode = UnwrapStatement(startingNode);
            _startingNode = (HelperExpression)startingNode;
        }

        public string BlockName => _startingNode.HelperName;

        public override void HandleElement(Expression item)
        {
            if (IsElseBlock(item))
            {
                _accumulatedExpression = HandlebarsExpression.Iterator(
                    _startingNode.Arguments.Single(),
                    Expression.Block(_body));
                _body = new List<Expression>();
            }
            else
            {
                _body.Add((Expression)item);
            }
        }

        public override bool IsClosingElement(Expression item)
        {
            if (IsClosingNode(item))
            {
                if (_accumulatedExpression == null)
                {
                    _accumulatedExpression = HandlebarsExpression.Iterator(
                        _startingNode.Arguments.Single(),
                        Expression.Block(_body));
                }
                else
                {
                    _accumulatedExpression = HandlebarsExpression.Iterator(
                        ((IteratorExpression)_accumulatedExpression).Sequence,
                        ((IteratorExpression)_accumulatedExpression).Template,
                        Expression.Block(_body));
                }
                return true;
            }
            else
            {
                return false;
            }
        }

        public override Expression GetAccumulatedBlock()
        {
            return _accumulatedExpression;
        }

        private bool IsClosingNode(Expression item)
        {
            item = UnwrapStatement(item);
            return item is PathExpression && ((PathExpression)item).Path.Replace("#", "") == "/each";
        }

        private bool IsElseBlock(Expression item)
        {
            item = UnwrapStatement(item);
            return item is HelperExpression && ((HelperExpression)item).HelperName == "else";
        }
    }
}

