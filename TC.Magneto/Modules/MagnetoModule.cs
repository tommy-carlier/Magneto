using System;
using System.Collections.Generic;
using System.Text;

namespace TC.Magneto.Modules
{
	/// <summary>Contains functions and constants that can be used by a Magneto application.</summary>
	public abstract class MagnetoModule
	{
		/// <summary>Is called when the module is started.</summary>
		public void Start()
		{
			if (!hasStarted)
			{
				hasStarted = true;
				StartCore();
			}
		}

		bool hasStarted;

		/// <summary>Is called when the module is started.</summary>
		protected virtual void StartCore() { }

		/// <summary>Is called when the module is stopped.</summary>
		public void Stop()
		{
			if (hasStarted)
			{
				hasStarted = false;
				StopCore();
			}
		}

		/// <summary>Is called when the module is stopped.</summary>
		protected virtual void StopCore() { }
	}
}
