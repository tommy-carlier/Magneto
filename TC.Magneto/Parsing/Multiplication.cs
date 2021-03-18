using System;
using System.Collections.Generic;
using System.Text;

namespace TC.Magneto.Parsing
{
	/// <summary>Represents a multiplication.</summary>
	public class Multiplication : NumericBinaryExpression
	{
		/// <summary>Initializes a new instance of the <see cref="Multiplication"/> class.</summary>
		/// <param name="context">The current <see cref="T:ParserContext"/>.</param>
		/// <param name="leftChildExpression">The left child expression.</param>
		/// <param name="rightChildExpression">The right child expression.</param>
		internal Multiplication(ParserContext context, Expression leftChildExpression, Expression rightChildExpression)
			: base(context, leftChildExpression, rightChildExpression) { }

		/// <summary>When overriden in a derived class, accepts the specified visitor.</summary>
		/// <param name="visitor">The visitor to accept.</param>
		protected override void AcceptCore(IExpressionVisitor visitor)
		{
			visitor.Visit(this);
		}
	}
}
