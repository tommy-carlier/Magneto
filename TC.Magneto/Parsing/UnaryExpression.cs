using System;
using System.Collections.Generic;
using System.Text;

namespace TC.Magneto.Parsing
{
	/// <summary>Represents a unary expression.</summary>
	public abstract class UnaryExpression : Expression
	{
		/// <summary>Initializes a new instance of the <see cref="UnaryExpression"/> class.</summary>
		/// <param name="context">The current <see cref="T:ParserContext"/>.</param>
		/// <param name="childExpression">The child expression.</param>
		protected UnaryExpression(ParserContext context, Expression childExpression)
			: base(context)
		{
			this.childExpression = childExpression;
		}

		readonly Expression childExpression;
		/// <summary>Gets the child expression.</summary>
		/// <value>The child expression.</value>
		public Expression ChildExpression { get { return childExpression; } }
	}
}
