using System;
using System.Collections.Generic;
using System.Text;

namespace TC.Magneto.Parsing
{
	/// <summary>Represents a string-concatenation.</summary>
	public class Concatenation : Expression
	{
		/// <summary>Initializes a new instance of the <see cref="Concatenation"/> class.</summary>
		/// <param name="context">The current <see cref="T:ParserContext"/>.</param>
		/// <param name="childExpressions">The child expressions.</param>
		internal Concatenation(ParserContext context, Expression[] childExpressions)
			: base(context)
		{
			this.childExpressions = childExpressions;
		}

		readonly Expression[] childExpressions;
		/// <summary>Gets the child expressions.</summary>
		/// <value>The child expressions.</value>
		public Expression[] ChildExpressions { get { return childExpressions; } }

		/// <summary>Gets the type of the result.</summary>
		/// <value>The type of the result.</value>
		public override DataType ResultType { get { return DataType.String; } }

		/// <summary>When overriden in a derived class, accepts the specified visitor.</summary>
		/// <param name="visitor">The visitor to accept.</param>
		protected override void AcceptCore(IExpressionVisitor visitor)
		{
			visitor.Visit(this);
		}
	}
}
