using System;
using System.Collections.Generic;
using System.Text;

namespace TC.Magneto.Parsing
{
	/// <summary>Represents a statement.</summary>
	public abstract class Statement : ParserNode
	{
		/// <summary>Initializes a new instance of the <see cref="Statement"/> class.</summary>
		/// <param name="context">The current <see cref="T:ParserContext"/>.</param>
		protected Statement(ParserContext context) : base(context) { }

		/// <summary>Accepts the specified visitor.</summary>
		/// <param name="visitor">The visitor to accept.</param>
		public void Accept(IStatementVisitor visitor)
		{
			if (visitor == null) throw new ArgumentNullException("visitor");
			AcceptCore(visitor);
		}

		/// <summary>Accepts the specified visitor.</summary>
		/// <param name="visitor">The visitor to accept.</param>
		internal void AcceptInternal(IStatementVisitor visitor) { AcceptCore(visitor); }

		/// <summary>When overriden in a derived class, accepts the specified visitor.</summary>
		/// <param name="visitor">The visitor to accept.</param>
		protected abstract void AcceptCore(IStatementVisitor visitor);
	}
}
