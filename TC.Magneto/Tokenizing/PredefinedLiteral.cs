using System;
using System.Collections.Generic;
using System.Text;

namespace TC.Magneto.Tokenizing
{
	/// <summary>Represents one of the predefined literals.</summary>
	public enum PredefinedLiteral
	{
		/// <summary>No literal.</summary>
		None = 0,

		/// <summary>The predefined literal 'true'.</summary>
		True,

		/// <summary>The predefined literal 'false'.</summary>
		False,

		/// <summary>The predefined literal 'pos'.</summary>
		Positive,

		/// <summary>The predefined literal 'neg'.</summary>
		Negative,

		/// <summary>The predefined literal 'north'.</summary>
		North,

		/// <summary>The predefined literal 'south'.</summary>
		South,

		/// <summary>The predefined literal 'cw'.</summary>
		Clockwise,

		/// <summary>The predefined literal 'ccw'.</summary>
		Counterclockwise,

		/// <summary>The predefined literal 'open'.</summary>
		Open,

		/// <summary>The predefined literal 'closed'.</summary>
		Closed
	}
}
