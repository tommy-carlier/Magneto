using System;
using System.Collections.Generic;
using System.Windows.Forms;
using TC.WinForms;

namespace TC.Magneto.Shell
{
	static class Program
	{
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main()
		{
			TApplication.Run<MagnetoApplication, MainForm>();
		}
	}
}
