using System;
using System.Collections.Generic;
using System.Text;

namespace TC.Magneto.Parsing
{
	/// <summary>Represents a boolean expression with 2 boolean child expressions.</summary>
	public abstract class BooleanBinaryExpression : BinaryExpression
	{
		/// <summary>Initializes a new instance of the <see cref="BooleanBinaryExpression"/> class.</summary>
		/// <param name="context">The current <see cref="T:ParserContext"/>.</param>
		/// <param name="leftChildExpression">The left child expression.</param>
		/// <param name="rightChildExpression">The right child expression.</param>
		/// <param name="trueValue">The value that represents true.</param>
		/// <param name="falseValue">The value that represents false.</param>
		protected BooleanBinaryExpression(ParserContext context
				, Expression leftChildExpression, Expression rightChildExpression
				, Expression trueValue, Expression falseValue)
			: base(context, leftChildExpression, rightChildExpression)
		{
			this.trueValue = trueValue;
			this.falseValue = falseValue;
		}

		readonly Expression trueValue;
		/// <summary>Gets the value that represents true.</summary>
		/// <value>The value that represents true.</value>
		public Expression TrueValue { get { return trueValue; } }

		readonly Expression falseValue;
		/// <summary>Gets the value that represents false.</summary>
		/// <value>The value that represents false.</value>
		public Expression FalseValue { get { return falseValue; } }

		/// <summary>Gets the type of the result.</summary>
		/// <value>The type of the result.</value>
		public override DataType ResultType { get { return LeftChildExpression.ResultType; } }
	}
}
