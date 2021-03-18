using System;
using System.Collections.Generic;
using System.Text;

namespace TC.Magneto.Parsing
{
	/// <summary>Represents a repeat-loop statement.</summary>
	public class RepeatLoopStatement : LoopStatement
	{
		/// <summary>Initializes a new instance of the <see cref="RepeatLoopStatement"/> class.</summary>
		/// <param name="context">The current <see cref="T:ParserContext"/>.</param>
		/// <param name="condition">The condition of the loop.</param>
		/// <param name="body">The body of the loop.</param>
		internal RepeatLoopStatement(ParserContext context, Expression condition, StatementCollection body)
			: base(context, body)
		{
			this.condition = condition;
		}

		readonly Expression condition;
		/// <summary>Gets the condition of the loop.</summary>
		/// <value>The condition of the loop.</value>
		public Expression Condition { get { return condition; } }

		/// <summary>When overriden in a derived class, accepts the specified visitor.</summary>
		/// <param name="visitor">The visitor to accept.</param>
		protected override void AcceptCore(IStatementVisitor visitor)
		{
			visitor.Visit(this);
		}
	}
}
