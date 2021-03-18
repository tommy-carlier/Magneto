using System;
using System.Collections.Generic;
using System.Text;

namespace TC.Magneto.Parsing
{
	/// <summary>Represents a numeric expression with 2 numeric child expressions.</summary>
	public abstract class NumericBinaryExpression : BinaryExpression
	{
		/// <summary>Initializes a new instance of the <see cref="NumericBinaryExpression"/> class.</summary>
		/// <param name="context">The current <see cref="T:ParserContext"/>.</param>
		/// <param name="leftChildExpression">The left child expression.</param>
		/// <param name="rightChildExpression">The right child expression.</param>
		protected NumericBinaryExpression(ParserContext context, Expression leftChildExpression, Expression rightChildExpression)
			: base(context, leftChildExpression, rightChildExpression) { }

		/// <summary>Gets the type of the result.</summary>
		/// <value>The type of the result.</value>
		public override DataType ResultType
		{
			get
			{
				return LeftChildExpression.ResultType == DataType.Real
					|| RightChildExpression.ResultType == DataType.Real
					? DataType.Real : DataType.Integer;
			}
		}
	}
}
