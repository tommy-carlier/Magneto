using System;
using System.Collections.Generic;
using System.Text;
using TC.Magneto.Tokenizing;

namespace TC.Magneto.Parsing
{
	/// <summary>Represents a comparison.</summary>
	public class Comparison : BinaryExpression
	{
		/// <summary>Initializes a new instance of the <see cref="Comparison"/> class.</summary>
		/// <param name="context">The current <see cref="T:ParserContext"/>.</param>
		/// <param name="operator">The comparison operator.</param>
		/// <param name="leftChildExpression">The left child expression.</param>
		/// <param name="rightChildExpression">The right child expression.</param>
		internal Comparison(ParserContext context, ComparisonOperator @operator, Expression leftChildExpression, Expression rightChildExpression)
			: base(context, leftChildExpression, rightChildExpression)
		{
			this.@operator = @operator;
		}

		readonly ComparisonOperator @operator;
		/// <summary>Gets the comparison operator.</summary>
		/// <value>The comparison operator.</value>
		public ComparisonOperator Operator { get { return @operator; } }

		/// <summary>Gets the type of the result.</summary>
		/// <value>The type of the result.</value>
		public override DataType ResultType { get { return DataType.Logic; } }

		/// <summary>Gets the <see cref="T:ComparisonOperator"/> that matches the specified <see cref="T:PredefinedSymbol"/>.</summary>
		/// <param name="symbol">The <see cref="T:PredefinedSymbol"/> to match.</param>
		/// <returns>The matched <see cref="T:ComparisonOperator"/>.</returns>
		public static ComparisonOperator GetOperator(PredefinedSymbol symbol)
		{
			switch (symbol)
			{
				case PredefinedSymbol.Equal: return ComparisonOperator.Equal;
				case PredefinedSymbol.NotEqual: return ComparisonOperator.NotEqual;
				case PredefinedSymbol.LessThan: return ComparisonOperator.LessThan;
				case PredefinedSymbol.GreaterThan: return ComparisonOperator.GreaterThan;
				case PredefinedSymbol.LessThanOrEqual: return ComparisonOperator.LessThanOrEqual;
				case PredefinedSymbol.GreaterThanOrEqual: return ComparisonOperator.GreaterThanOrEqual;
				default: return ComparisonOperator.None;
			}
		}

		/// <summary>When overriden in a derived class, accepts the specified visitor.</summary>
		/// <param name="visitor">The visitor to accept.</param>
		protected override void AcceptCore(IExpressionVisitor visitor)
		{
			visitor.Visit(this);
		}
	}
}
