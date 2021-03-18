using System;
using System.Collections.Generic;
using System.Text;

namespace TC.Magneto.Parsing
{
	/// <summary>Represents a statement that calls a function and ignores the resulting value.</summary>
	public class FunctionCallStatement : Statement
	{
		/// <summary>Initializes a new instance of the <see cref="FunctionCallStatement"/> class.</summary>
		/// <param name="context">The current <see cref="T:ParserContext"/>.</param>
		/// <param name="functionCall">The function call.</param>
		internal FunctionCallStatement(ParserContext context, FunctionCall functionCall)
			: base(context)
		{
			this.functionCall = functionCall;
		}

		readonly FunctionCall functionCall;
		/// <summary>Gets the function call.</summary>
		/// <value>The function call.</value>
		public FunctionCall FunctionCall { get { return functionCall; } }

		/// <summary>When overriden in a derived class, accepts the specified visitor.</summary>
		/// <param name="visitor">The visitor to accept.</param>
		protected override void AcceptCore(IStatementVisitor visitor)
		{
			visitor.Visit(this);
		}
	}
}
