using System;
using System.Collections.Generic;
using System.Text;

namespace TC.Magneto.Parsing
{
	/// <summary>Represents a case-statement (inside a switch-statement).</summary>
	public class CaseStatement : Statement
	{
		/// <summary>Initializes a new instance of the <see cref="CaseStatement"/> class.</summary>
		/// <param name="context">The current <see cref="T:ParserContext"/>.</param>
		/// <param name="value">The value to compare with the value of the switch-statement.</param>
		/// <param name="body">The statements that are executed when the case-value matches the switch-value.</param>
		internal CaseStatement(ParserContext context, Expression value, StatementCollection body)
			: base(context)
		{
			this.value = value;
			this.body = body;
		}

		readonly Expression value;
		/// <summary>Gets the value to compare with the value of the switch-statement.</summary>
		/// <value>The value to compare with the value of the switch-statement.</value>
		public Expression Value { get { return value; } }

		readonly StatementCollection body;
		/// <summary>Gets the statements that are executed when the case-value matches the switch-value.</summary>
		/// <value>The statements that are executed when the case-value matches the switch-value.</value>
		public StatementCollection Body { get { return body; } }

		/// <summary>When overriden in a derived class, accepts the specified visitor.</summary>
		/// <param name="visitor">The visitor to accept.</param>
		protected override void AcceptCore(IStatementVisitor visitor)
		{
			visitor.Visit(this);
		}
	}
}
