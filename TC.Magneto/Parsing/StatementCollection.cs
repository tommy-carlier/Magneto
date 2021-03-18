using System;
using System.Collections.Generic;
using System.Text;

namespace TC.Magneto.Parsing
{
	/// <summary>Represents a collection of statements.</summary>
	public class StatementCollection : Statement, IEnumerable<Statement>
	{
		/// <summary>Initializes a new instance of the <see cref="StatementCollection"/> class.</summary>
		/// <param name="context">The current <see cref="T:ParserContext"/>.</param>
		/// <param name="statements">The statements inside the collection.</param>
		internal StatementCollection(ParserContext context, IEnumerable<Statement> statements)
			: base(context)
		{
            this.statements = statements ?? throw new ArgumentNullException("statements");
		}

		readonly IEnumerable<Statement> statements;

		#region IEnumerable<Statement> Members

		/// <summary>Returns an enumerator that iterates through the collection.</summary>
		/// <returns>A <see cref="T:IEnumerator`1"/> that can be used to iterate through the collection.</returns>
		public IEnumerator<Statement> GetEnumerator() { return statements.GetEnumerator(); }

		#endregion

		#region IEnumerable Members

		/// <summary>Returns an enumerator that iterates through a collection.</summary>
		/// <returns>An <see cref="T:System.Collections.IEnumerator"/> object that can be used to iterate through the collection.</returns>
		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() { return statements.GetEnumerator(); }

		#endregion

		/// <summary>When overriden in a derived class, accepts the specified visitor.</summary>
		/// <param name="visitor">The visitor to accept.</param>
		protected override void AcceptCore(IStatementVisitor visitor)
		{
			foreach (Statement statement in statements)
				statement.AcceptInternal(visitor);
		}
	}
}
