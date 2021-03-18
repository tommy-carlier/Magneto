using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.Xml.XPath;
using TC.Settings;

namespace TC.Magneto.Shell
{
	class ApplicationSettings : BaseApplicationSettings
	{
		/// <summary>Initializes a new instance of the <see cref="ApplicationSettings"/> class.</summary>
		public ApplicationSettings()
			: base(Application.ExecutablePath + ".settings")
		{
			MainForm = RegisterSettings(new MainForm.Settings());
		}

		public readonly MainForm.Settings MainForm;
	}
}
