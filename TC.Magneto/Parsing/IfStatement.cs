using System;
using System.Collections.Generic;
using System.Text;

namespace TC.Magneto.Parsing
{
	/// <summary>Represents an if-statement.</summary>
	public class IfStatement : Statement
	{
		/// <summary>Initializes a new instance of the <see cref="IfStatement"/> class.</summary>
		/// <param name="context">The current <see cref="T:ParserContext"/>.</param>
		/// <param name="condition">The condition to evaluate.</param>
		/// <param name="trueStatements">The statements that are executed when the condition evaluates to true.</param>
		/// <param name="falseStatements">The statements that are executed when the condition evaluates to false.</param>
		internal IfStatement(ParserContext context, Expression condition, StatementCollection trueStatements, StatementCollection falseStatements)
			: base(context)
		{
			this.condition = condition;
			this.trueStatements = trueStatements;
			this.falseStatements = falseStatements;
		}

		readonly Expression condition;
		/// <summary>Gets the condition to evaluate.</summary>
		/// <value>The condition to evaluate.</value>
		public Expression Condition { get { return condition; } }

		readonly StatementCollection trueStatements;
		/// <summary>Gets the statements that are executed when the condition evaluates to true.</summary>
		/// <value>The statements that are executed when the condition evaluates to true.</value>
		public StatementCollection TrueStatements { get { return trueStatements; } }

		readonly StatementCollection falseStatements;
		/// <summary>Gets the statements that are executed when the condition evaluates to false.</summary>
		/// <value>The statements that are executed when the condition evaluates to false.</value>
		public StatementCollection FalseStatements { get { return falseStatements; } }

		/// <summary>When overriden in a derived class, accepts the specified visitor.</summary>
		/// <param name="visitor">The visitor to accept.</param>
		protected override void AcceptCore(IStatementVisitor visitor)
		{
			visitor.Visit(this);
		}
	}
}
