using System;
using System.Collections.Generic;
using System.Text;

namespace TC.Magneto.Parsing
{
	/// <summary>Represents a for-loop statement.</summary>
	public class ForLoopStatement : LoopStatement
	{
		/// <summary>Initializes a new instance of the <see cref="ForLoopStatement"/> class.</summary>
		/// <param name="context">The current <see cref="T:ParserContext"/>.</param>
		/// <param name="variable">The variable that contains the loop value.</param>
		/// <param name="startExpression">The start-expression.</param>
		/// <param name="endExpression">The end-expression.</param>
		/// <param name="stepExpression">The step-expression.</param>
		/// <param name="body">The body of the loop.</param>
		internal ForLoopStatement(ParserContext context, Variable variable, Expression startExpression, Expression endExpression, Expression stepExpression, StatementCollection body)
			: base(context, body)
		{
			this.variable = variable;
			this.startExpression = startExpression;
			this.endExpression = endExpression;
			this.stepExpression = stepExpression;
		}

		readonly Variable variable;
		/// <summary>Gets the variable that contains the loop value.</summary>
		/// <value>The variable that contains the loop value.</value>
		public Variable Variable { get { return variable; } }

		readonly Expression startExpression;
		/// <summary>Gets the start-expression.</summary>
		/// <value>The start-expression.</value>
		public Expression StartExpression { get { return startExpression; } }

		readonly Expression endExpression;
		/// <summary>Gets the end-expression.</summary>
		/// <value>The end-expression.</value>
		public Expression EndExpression { get { return endExpression; } }

		readonly Expression stepExpression;
		/// <summary>Gets the step-expression.</summary>
		/// <value>The step-expression.</value>
		public Expression StepExpression { get { return stepExpression; } }

		/// <summary>When overriden in a derived class, accepts the specified visitor.</summary>
		/// <param name="visitor">The visitor to accept.</param>
		protected override void AcceptCore(IStatementVisitor visitor)
		{
			visitor.Visit(this);
		}
	}
}
