using System;
using System.Collections.Generic;
using System.Text;

namespace TC.Magneto.Parsing
{
	/// <summary>Represents an expression visitor.</summary>
	/// <remarks>Part of the visitor-pattern: http://en.wikipedia.org/wiki/Visitor_pattern
	/// </remarks>
	public interface IExpressionVisitor
	{
		/// <summary>Visits the specified expression.</summary>
		/// <param name="expression">The expression to visit.</param>
		void Visit(Negation expression);

		/// <summary>Visits the specified expression.</summary>
		/// <param name="expression">The expression to visit.</param>
		void Visit(NotExpression expression);

		/// <summary>Visits the specified expression.</summary>
		/// <param name="expression">The expression to visit.</param>
		void Visit(VariableExpression expression);

		/// <summary>Visits the specified expression.</summary>
		/// <param name="expression">The expression to visit.</param>
		void Visit(VariableReference expression);

		/// <summary>Visits the specified expression.</summary>
		/// <param name="expression">The expression to visit.</param>
		void Visit(FunctionCall expression);

		/// <summary>Visits the specified expression.</summary>
		/// <param name="expression">The expression to visit.</param>
		void Visit<T>(Literal<T> expression);

		/// <summary>Visits the specified expression.</summary>
		/// <param name="expression">The expression to visit.</param>
		void Visit(AndExpression expression);

		/// <summary>Visits the specified expression.</summary>
		/// <param name="expression">The expression to visit.</param>
		void Visit(OrExpression expression);

		/// <summary>Visits the specified expression.</summary>
		/// <param name="expression">The expression to visit.</param>
		void Visit(XorExpression expression);

		/// <summary>Visits the specified expression.</summary>
		/// <param name="expression">The expression to visit.</param>
		void Visit(Addition expression);

		/// <summary>Visits the specified expression.</summary>
		/// <param name="expression">The expression to visit.</param>
		void Visit(Subtraction expression);

		/// <summary>Visits the specified expression.</summary>
		/// <param name="expression">The expression to visit.</param>
		void Visit(Multiplication expression);

		/// <summary>Visits the specified expression.</summary>
		/// <param name="expression">The expression to visit.</param>
		void Visit(Division expression);

		/// <summary>Visits the specified expression.</summary>
		/// <param name="expression">The expression to visit.</param>
		void Visit(ModuloExpression expression);

		/// <summary>Visits the specified expression.</summary>
		/// <param name="expression">The expression to visit.</param>
		void Visit(PowerExpression expression);

		/// <summary>Visits the specified expression.</summary>
		/// <param name="expression">The expression to visit.</param>
		void Visit(Comparison expression);

		/// <summary>Visits the specified expression.</summary>
		/// <param name="expression">The expression to visit.</param>
		void Visit(Concatenation expression);
	}
}
