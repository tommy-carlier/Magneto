using System;
using System.Collections.Generic;
using System.Text;

namespace TC.Magneto.Parsing
{
	/// <summary>Represents a variable declaration.</summary>
	public class VariableDeclaration : Statement
	{
		/// <summary>Initializes a new instance of the <see cref="VariableDeclaration"/> class.</summary>
		/// <param name="context">The current <see cref="T:ParserContext"/>.</param>
		/// <param name="variable">The declared variable.</param>
		internal VariableDeclaration(ParserContext context, Variable variable)
			: base(context)
		{
			this.variable = variable;
		}

		readonly Variable variable;
		/// <summary>Gets the declared variable.</summary>
		/// <value>The declared variable.</value>
		public Variable Variable { get { return variable; } }

		/// <summary>When overriden in a derived class, accepts the specified visitor.</summary>
		/// <param name="visitor">The visitor to accept.</param>
		protected override void AcceptCore(IStatementVisitor visitor)
		{
			visitor.Visit(this);
		}
	}
}
