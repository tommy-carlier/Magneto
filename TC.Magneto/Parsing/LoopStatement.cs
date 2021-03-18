using System;
using System.Collections.Generic;
using System.Text;

namespace TC.Magneto.Parsing
{
	/// <summary>Represents a looping statement.</summary>
	public abstract class LoopStatement : Statement
	{
		/// <summary>Initializes a new instance of the <see cref="LoopStatement"/> class.</summary>
		/// <param name="context">The current <see cref="T:ParserContext"/>.</param>
		/// <param name="body">The body of the loop.</param>
		protected LoopStatement(ParserContext context, StatementCollection body)
			: base(context)
		{
			this.body = body;
		}

		readonly StatementCollection body;
		/// <summary>Gets the statements that represent the body of the loop.</summary>
		/// <value>The statements that represent the body of the loop.</value>
		public StatementCollection Body { get { return body; } }
	}
}
