using System;
using System.Collections.Generic;
using System.Text;
using TC.WinForms;
using TC.Magneto.Modules;
using System.IO;
using System.Windows.Forms;
using System.Reflection;
using TC.WinForms.Dialogs;

namespace TC.Magneto.Shell
{
	class MagnetoApplication : TApplication
	{
		/// <summary>Gets the current application.</summary>
		/// <value>The current application.</value>
		public static new MagnetoApplication Current { get { return TApplication.Current as MagnetoApplication; } }

		/// <summary>Gets the application settings.</summary>
		public readonly ApplicationSettings Settings = new ApplicationSettings();

		/// <summary>Gets the <see cref="T:ModuleManager"/>.</summary>
		public readonly ModuleManager ModuleManager = new ModuleManager();

		/// <summary>Gets the full path of the temporary folder that Magneto uses.</summary>
		public readonly string TempFolderPath = Path.Combine(Path.GetTempPath(), "Magneto");

		/// <summary>Initializes a new instance of the <see cref="MagnetoApplication"/> class.</summary>
		public MagnetoApplication()
		{
			Settings.Load();
			LoadModules();
			ClearTempFolder();
		}

		/// <summary>Terminates the message loop of the thread.</summary>
		protected override void ExitThreadCore()
		{
			base.ExitThreadCore();
			Settings.Save();
		}

		void LoadModules()
		{
			string moduleFolderPath = Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), "Modules");
			if (Directory.Exists(moduleFolderPath))
			{
				foreach (string moduleFile in Directory.EnumerateFiles(moduleFolderPath, "*.MagnetoModule.dll"))
					try
					{
						ModuleManager.Register(Assembly.LoadFile(moduleFile));
					}
					catch (Exception exception)
					{
						if (!SystemUtilities.IsCritical(exception))
						{
							TMessageDialog.ShowError(null
								, "Could not load module " + Path.GetFileName(moduleFile) + ":"
								+ Environment.NewLine + exception.Message);
						}
						else throw;
					}
			}
		}

		void ClearTempFolder()
		{
			if (Directory.Exists(TempFolderPath))
				foreach(string tempSubFolderPath in Directory.EnumerateDirectories(TempFolderPath))
					try { Directory.Delete(tempSubFolderPath, true); }
					catch (Exception exception) { if (SystemUtilities.IsCritical(exception)) throw; }
		}
	}
}
