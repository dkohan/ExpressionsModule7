using System;
using System.Linq.Expressions;

namespace ExpressionTrees.Task1.ExpressionsTransformer
{
    public class IncDecExpressionVisitor : ExpressionVisitor
    {
        public int indent = 0;
        public override Expression Visit(Expression node)
        {
            if (node == null)
            {
                return base.Visit(node);
            }

            Console.WriteLine("{0}{1} - {2}", new String(' ', indent * 4), node.NodeType, node.GetType());
            indent++;
            Expression result = base.Visit(node);
            indent--;

            return result;            
        }

        // todo: feel free to add your code here
        protected override Expression VisitBinary(BinaryExpression node)
        {
            if (node.NodeType == ExpressionType.Add)
            {
                if (node.Left.NodeType == ExpressionType.Parameter)
                {
                    if ((node.Right.Type == typeof(int)) && node.Right.NodeType == ExpressionType.Constant)
                    {
                        if ((node.Right as ConstantExpression)?.Value as int? == 1)
                            return Expression.Increment(node.Left);
                    }
                }
            }
            else
            if (node.NodeType == ExpressionType.Subtract)
            {
                if (node.Left.NodeType == ExpressionType.Parameter)
                {
                    if ((node.Right.Type == typeof(int)))
                    {
                        return Expression.Decrement(node.Left);
                    }
                }
            }

            return base.VisitBinary(node);
        }
    }
}
