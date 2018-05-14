using System;
using System.Linq;
using System.Linq.Expressions;
using System.Collections.Generic;

namespace HandlebarsDotNet.Compiler
{
    internal class BlockHelperAccumulatorContext : BlockAccumulatorContext
    {
        private readonly HelperExpression _startingNode;
        private Expression _accumulatedBody;
        private Expression _accumulatedInversion;
        private List<Expression> _body = new List<Expression>();

        public BlockHelperAccumulatorContext(Expression startingNode)
            : base(startingNode)
        {
            startingNode = UnwrapStatement(startingNode);
            _startingNode = (HelperExpression)startingNode;
        }

        public string HelperName => _startingNode.HelperName;

        public override void HandleElement(Expression item)
        {
            if (IsInversionBlock(item))
            {
                _accumulatedBody = Expression.Block(_body);
                _body = new List<Expression>();
            }
            else
            {
                _body.Add((Expression)item);
            }
        }

        private bool IsInversionBlock(Expression item)
        {
            item = UnwrapStatement(item);
            return item is HelperExpression && ((HelperExpression)item).HelperName == "else";
        }

        public override bool IsClosingElement(Expression item)
        {
            item = UnwrapStatement(item);
            return IsClosingNode(item);
        }

        private bool IsClosingNode(Expression item)
        {
            var helperName = _startingNode.HelperName.Replace("#", "").Replace("*", "");
            return item is PathExpression && ((PathExpression)item).Path == "/" + helperName;
        }

        public override Expression GetAccumulatedBlock()
        {
            if (_accumulatedBody == null)
            {
                _accumulatedBody = Expression.Block(_body);
                _accumulatedInversion = Expression.Block(Expression.Empty());
            }
            else if (_accumulatedInversion == null && _body.Any())
            {
                _accumulatedInversion = Expression.Block(_body);
            }
            else
            {
                _accumulatedInversion = Expression.Block(Expression.Empty());
            }
            return HandlebarsExpression.BlockHelper(
                _startingNode.HelperName,
                _startingNode.Arguments,
                _accumulatedBody,
                _accumulatedInversion);
        }

    }
}

