using System;
using System.Collections.Generic;
using System.Text;

namespace TC.Magneto.Parsing
{
	/// <summary>Represents a logical NOT-expression.</summary>
	public class NotExpression : UnaryExpression
	{
		/// <summary>Initializes a new instance of the <see cref="NotExpression"/> class.</summary>
		/// <param name="context">The current <see cref="T:ParserContext"/>.</param>
		/// <param name="childExpression">The child expression.</param>
		internal NotExpression(ParserContext context, Expression childExpression)
			: base(context, childExpression) { }

		/// <summary>Gets the type of the result.</summary>
		/// <value>The type of the result.</value>
		public override DataType ResultType { get { return ChildExpression.ResultType; } }

		/// <summary>When overriden in a derived class, accepts the specified visitor.</summary>
		/// <param name="visitor">The visitor to accept.</param>
		protected override void AcceptCore(IExpressionVisitor visitor)
		{
			visitor.Visit(this);
		}
	}
}
