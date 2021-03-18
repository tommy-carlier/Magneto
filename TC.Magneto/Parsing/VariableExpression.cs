using System;
using System.Collections.Generic;
using System.Text;

namespace TC.Magneto.Parsing
{
	/// <summary>Represents the reference to a variable.</summary>
	public class VariableExpression : Expression
	{
		/// <summary>Initializes a new instance of the <see cref="VariableExpression"/> class.</summary>
		/// <param name="context">The current <see cref="T:ParserContext"/>.</param>
		/// <param name="variable">The variable.</param>
		internal VariableExpression(ParserContext context, Variable variable)
			: base(context)
		{
			this.variable = variable;
		}

		readonly Variable variable;
		/// <summary>Gets the variable.</summary>
		/// <value>The variable.</value>
		public Variable Variable { get { return variable; } }

		/// <summary>Gets the type of the result.</summary>
		/// <value>The type of the result.</value>
		public override DataType ResultType { get { return variable.DataType; } }

		/// <summary>When overriden in a derived class, accepts the specified visitor.</summary>
		/// <param name="visitor">The visitor to accept.</param>
		protected override void AcceptCore(IExpressionVisitor visitor)
		{
			visitor.Visit(this);
		}
	}
}
