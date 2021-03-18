using System;
using System.Collections.Generic;
using System.Text;

namespace TC.Magneto.Parsing
{
	/// <summary>Represents a statement visitor.</summary>
	/// <remarks>Part of the visitor-pattern: http://en.wikipedia.org/wiki/Visitor_pattern
	/// </remarks>
	public interface IStatementVisitor
	{
		/// <summary>Visits the specified statement.</summary>
		/// <param name="statement">The statement to visit.</param>
		void Visit(IfStatement statement);

		/// <summary>Visits the specified statement.</summary>
		/// <param name="statement">The statement to visit.</param>
		void Visit(VariableDeclaration statement);

		/// <summary>Visits the specified statement.</summary>
		/// <param name="statement">The statement to visit.</param>
		void Visit(Assignment statement);

		/// <summary>Visits the specified statement.</summary>
		/// <param name="statement">The statement to visit.</param>
		void Visit(SwitchStatement statement);

		/// <summary>Visits the specified statement.</summary>
		/// <param name="statement">The statement to visit.</param>
		void Visit(CaseStatement statement);

		/// <summary>Visits the specified statement.</summary>
		/// <param name="statement">The statement to visit.</param>
		void Visit(WhileLoopStatement statement);

		/// <summary>Visits the specified statement.</summary>
		/// <param name="statement">The statement to visit.</param>
		void Visit(RepeatLoopStatement statement);

		/// <summary>Visits the specified statement.</summary>
		/// <param name="statement">The statement to visit.</param>
		void Visit(ForLoopStatement statement);

		/// <summary>Visits the specified statement.</summary>
		/// <param name="statement">The statement to visit.</param>
		void Visit(FunctionCallStatement statement);

		/// <summary>Visits the specified statement.</summary>
		/// <param name="statement">The statement to visit.</param>
		void Visit(BreakStatement statement);

		/// <summary>Visits the specified statement.</summary>
		/// <param name="statement">The statement to visit.</param>
		void Visit(ExitStatement statement);
	}
}
