using System;
using System.Collections.Generic;
using System.Text;
using TC.Magneto.Modules;

namespace TC.Magneto.Parsing
{
	/// <summary>Represents a function call.</summary>
	public class FunctionCall : Expression
	{
		/// <summary>Initializes a new instance of the <see cref="FunctionCall"/> class.</summary>
		/// <param name="context">The current <see cref="T:ParserContext"/>.</param>
		/// <param name="signature">The signature of the function.</param>
		/// <param name="arguments">The arguments of the function.</param>
		internal FunctionCall(ParserContext context, FunctionSignature signature, IEnumerable<Expression> arguments)
			: base(context)
		{
			this.signature = signature;
			this.arguments = arguments;
		}

		readonly FunctionSignature signature;
		/// <summary>Gets the signature of the function.</summary>
		/// <value>The signature of the function.</value>
		public FunctionSignature Signature { get { return signature; } }
		
		readonly IEnumerable<Expression> arguments;
		/// <summary>Gets the arguments of the function.</summary>
		/// <value>The arguments of the function.</value>
		public IEnumerable<Expression> Arguments { get { return arguments; } }

		/// <summary>Gets the type of the result.</summary>
		/// <value>The type of the result.</value>
		public override DataType ResultType { get { return signature.ResultType; } }

		/// <summary>When overriden in a derived class, accepts the specified visitor.</summary>
		/// <param name="visitor">The visitor to accept.</param>
		protected override void AcceptCore(IExpressionVisitor visitor)
		{
			visitor.Visit(this);
		}
	}
}
