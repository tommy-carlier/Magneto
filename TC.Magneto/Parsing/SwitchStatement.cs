using System;
using System.Collections.Generic;
using System.Text;

namespace TC.Magneto.Parsing
{
	/// <summary>Represents a switch-statement.</summary>
	public class SwitchStatement : Statement
	{
		/// <summary>Initializes a new instance of the <see cref="SwitchStatement"/> class.</summary>
		/// <param name="context">The current <see cref="T:ParserContext"/>.</param>
		/// <param name="value">The value to compare with the case values.</param>
		/// <param name="cases">The case-statements to compare with the value.</param>
		/// <param name="elseBody">The statements that are executed when none of the cases match.</param>
		internal SwitchStatement(ParserContext context, Expression value, IEnumerable<CaseStatement> cases, StatementCollection elseBody)
			: base(context)
		{
			this.value = value;
			this.cases = cases;
			this.elseBody = elseBody;
		}

		readonly Expression value;
		/// <summary>Gets the value to compare with the case values.</summary>
		/// <value>The value to compare with the case values.</value>
		public Expression Value { get { return value; } }

		readonly IEnumerable<CaseStatement> cases;
		/// <summary>Gets the case-statements to compare with the value.</summary>
		/// <value>The cases to compare with the value.</value>
		public IEnumerable<CaseStatement> Cases { get { return cases; } }

		readonly StatementCollection elseBody;
		/// <summary>Gets the statements that are executed when none of the cases match.</summary>
		/// <value>The statements that are executed when none of the cases match.</value>
		public StatementCollection ElseBody { get { return elseBody; } }

		/// <summary>When overriden in a derived class, accepts the specified visitor.</summary>
		/// <param name="visitor">The visitor to accept.</param>
		protected override void AcceptCore(IStatementVisitor visitor)
		{
			visitor.Visit(this);
		}
	}
}
