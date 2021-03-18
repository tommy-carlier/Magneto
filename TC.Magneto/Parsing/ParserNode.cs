using System;
using System.Collections.Generic;
using System.Text;

namespace TC.Magneto.Parsing
{
	/// <summary>Represents a node that was created by the <see cref="T:Parser"/>.</summary>
	public abstract class ParserNode
	{
		/// <summary>Initializes a new instance of the <see cref="ParserNode"/> class.</summary>
		/// <param name="context">The current <see cref="T:ParserContext"/>.</param>
		protected ParserNode(ParserContext context)
		{
			this.context = context;
		}

		readonly ParserContext context;
		/// <summary>Gets the current <see cref="T:ParserContext"/>.</summary>
		/// <value>The current <see cref="T:ParserContext"/>.</value>
		public ParserContext Context { get { return context; } }
	}
}
