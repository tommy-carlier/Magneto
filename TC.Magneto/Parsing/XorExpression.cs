using System;
using System.Collections.Generic;
using System.Text;

namespace TC.Magneto.Parsing
{
	/// <summary>Represents a logical XOR-expression.</summary>
	public class XorExpression : BooleanBinaryExpression
	{
		/// <summary>Initializes a new instance of the <see cref="XorExpression"/> class.</summary>
		/// <param name="context">The current <see cref="T:ParserContext"/>.</param>
		/// <param name="leftChildExpression">The left child expression.</param>
		/// <param name="rightChildExpression">The right child expression.</param>
		/// <param name="trueValue">The value that represents true.</param>
		/// <param name="falseValue">The value that represents false.</param>
		internal XorExpression(ParserContext context
				, Expression leftChildExpression, Expression rightChildExpression
				, Expression trueValue, Expression falseValue)
			: base(context, leftChildExpression, rightChildExpression, trueValue, falseValue) { }

		/// <summary>When overriden in a derived class, accepts the specified visitor.</summary>
		/// <param name="visitor">The visitor to accept.</param>
		protected override void AcceptCore(IExpressionVisitor visitor)
		{
			visitor.Visit(this);
		}
	}
}
