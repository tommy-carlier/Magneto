using System;
using System.Collections.Generic;
using System.Text;

namespace TC.Magneto.Parsing
{
	/// <summary>Represents an assignment.</summary>
	public class Assignment : Statement
	{
		/// <summary>Initializes a new instance of the <see cref="Assignment"/> class.</summary>
		/// <param name="context">The current <see cref="T:ParserContext"/>.</param>
		/// <param name="variable">The variable to assign a value to.</param>
		/// <param name="value">The value to assign to the specified variable.</param>
		internal Assignment(ParserContext context, Variable variable, Expression value)
			: base(context)
		{
			this.variable = variable;
			this.value = value;
		}

		readonly Variable variable;
		/// <summary>Gets the variable to assign a value to.</summary>
		/// <value>The variable to assign a value to.</value>
		public Variable Variable { get { return variable; } }

		readonly Expression value;
		/// <summary>Gets the value to assign.</summary>
		/// <value>The value to assign.</value>
		public Expression Value { get { return value; } }

		/// <summary>When overriden in a derived class, accepts the specified visitor.</summary>
		/// <param name="visitor">The visitor to accept.</param>
		protected override void AcceptCore(IStatementVisitor visitor)
		{
			visitor.Visit(this);
		}
	}
}
