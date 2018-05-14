using System.Linq;
using System.Linq.Expressions;
using System.Collections.Generic;

namespace HandlebarsDotNet.Compiler
{
    internal class DeferredBlockAccumulatorContext : BlockAccumulatorContext
    {
        private readonly PathExpression _startingNode;
        private List<Expression> _body = new List<Expression>();
        private BlockExpression _accumulatedBody;
        private BlockExpression _accumulatedInversion;


        public DeferredBlockAccumulatorContext(Expression startingNode)
            : base(startingNode)
        {
            startingNode = UnwrapStatement(startingNode);
            _startingNode = (PathExpression)startingNode;
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

            return HandlebarsExpression.DeferredSection(
                _startingNode,
                _accumulatedBody,
                _accumulatedInversion);
        }

        public override void HandleElement(Expression item)
        {
            if (IsInversionBlock(item))
            {
                _accumulatedBody = Expression.Block(_body);
                _body = new List<Expression>();
            }
            else
            {
                _body.Add(item);
            }
        }

        public override bool IsClosingElement(Expression item)
        {
            item = UnwrapStatement(item);
            var blockName = _startingNode.Path.Replace("#", "").Replace("^", "");
            return item is PathExpression && ((PathExpression)item).Path == "/" + blockName;
        }

        private bool IsInversionBlock(Expression item)
        {
            item = UnwrapStatement(item);
            return item is HelperExpression && ((HelperExpression)item).HelperName == "else";
        }
    }
}

