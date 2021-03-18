using System;
using System.Collections.Generic;
using System.Text;

namespace TC.Magneto.Parsing
{
	/// <summary>Represents an expression.</summary>
	public abstract class Expression : ParserNode
	{
		/// <summary>Initializes a new instance of the <see cref="Expression"/> class.</summary>
		/// <param name="context">The current <see cref="T:ParserContext"/>.</param>
		protected Expression(ParserContext context) : base(context) { }

		/// <summary>Gets the type of the result.</summary>
		/// <value>The type of the result.</value>
		public abstract DataType ResultType { get; }

		/// <summary>Accepts the specified visitor.</summary>
		/// <param name="visitor">The visitor to accept.</param>
		public void Accept(IExpressionVisitor visitor)
		{
			if (visitor == null) throw new ArgumentNullException("visitor");
			AcceptCore(visitor);
		}

		/// <summary>Accepts the specified visitor.</summary>
		/// <param name="visitor">The visitor to accept.</param>
		internal void AcceptInternal(IExpressionVisitor visitor) { AcceptCore(visitor); }

		/// <summary>When overriden in a derived class, accepts the specified visitor.</summary>
		/// <param name="visitor">The visitor to accept.</param>
		protected abstract void AcceptCore(IExpressionVisitor visitor);
	}
}
