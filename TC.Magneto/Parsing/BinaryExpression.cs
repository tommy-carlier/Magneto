using System;
using System.Collections.Generic;
using System.Text;

namespace TC.Magneto.Parsing
{
	/// <summary>Represents an expression with 2 child expressions.</summary>
	public abstract class BinaryExpression : Expression
	{
		/// <summary>Initializes a new instance of the <see cref="BinaryExpression"/> class.</summary>
		/// <param name="context">The current <see cref="T:ParserContext"/>.</param>
		/// <param name="leftChildExpression">The left child expression.</param>
		/// <param name="rightChildExpression">The right child expression.</param>
		protected BinaryExpression(ParserContext context, Expression leftChildExpression, Expression rightChildExpression)
			: base(context)
		{
			this.leftChildExpression = leftChildExpression;
			this.rightChildExpression = rightChildExpression;
		}

		readonly Expression leftChildExpression;
		/// <summary>Gets the left child expression.</summary>
		/// <value>The left child expression.</value>
		public Expression LeftChildExpression { get { return leftChildExpression; } }

		readonly Expression rightChildExpression;
		/// <summary>Gets the right child expression.</summary>
		/// <value>The right child expression.</value>
		public Expression RightChildExpression { get { return rightChildExpression; } }
	}
}
