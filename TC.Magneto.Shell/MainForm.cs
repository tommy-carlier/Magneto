using System;
using TC.WinForms.Forms;
using TC.WinForms.Settings;
using System.Diagnostics;
using System.IO;
using TC.WinForms;

using SWF = System.Windows.Forms;

namespace TC.Magneto.Shell
{
    public partial class MainForm : TDocumentForm
	{
		/// <summary>Initializes a new instance of the <see cref="MainForm"/> class.</summary>
		public MainForm()
		{
			InitializeComponent();
			CodeEditor.FileName = string.Format("Untitled {0}.magneto", newFileNameCounter += 1);
			SetTitle();
		}

		static int newFileNameCounter = 0;

		/// <summary>When overriden in a derived class, loads the settings of this form.</summary>
		protected override void LoadSettingsCore()
		{
			base.LoadSettingsCore();
			LoadBaseFormSettings(MagnetoApplication.Current.Settings.MainForm);
		}

		/// <summary>When overriden in a derived class, saves the settings of this form.</summary>
		protected override void SaveSettingsCore()
		{
			base.SaveSettingsCore();
			SaveBaseFormSettings(MagnetoApplication.Current.Settings.MainForm);
		}

		/// <summary>Raises the <see cref="E:DocumentTitleChanged"/> event.</summary>
		/// <param name="e">The <see cref="T:System.EventArgs"/> instance containing the event data.</param>
		protected override void OnDocumentTitleChanged(EventArgs e)
		{
			base.OnDocumentTitleChanged(e);
			SetTitle();
		}

		void SetTitle() { Text = DocumentTitle + " - Magneto"; }

		#region inner class Settings

		public class Settings : DocumentFormSettings
		{
			/// <summary>Initializes a new instance of the <see cref="Settings"/> class.</summary>
			public Settings() : base("main-form") { }
		}

		#endregion

		private void SetLabelCurrentColumn(object sender, EventArgs e)
		{
			LabelCurrentColumn.Text = string.Format("Col {0}", CodeEditor.CurrentColumnNumber);
		}

		private void SetLabelCurrentLine(object sender, EventArgs e)
		{
			LabelCurrentLine.Text = string.Format("Ln {0}", CodeEditor.CurrentLineNumber);
		}

		private void SetLabelOverwriteMode(object sender, EventArgs e)
		{
			LabelOverwriteMode.Text = CodeEditor.OverwriteMode ? "OVW" : "INS";
		}

		private void NewDocument(object sender, EventArgs e)
		{
			new MainForm().Show();
		}

		private void Run(object sender, EventArgs e)
		{
			MagnetoRunner.Run(
				sourceCode: CodeEditor.Text,
				runStateCallback: running => Dispatcher.InvokeAsync(this, SetRunState, running),
				exceptionHandler: HandleRunException);
		}

		private void HandleRunException(Exception ex)
        {
			ShowError(ex);

			if (ex is MagnetoException mEx && mEx.Position.CharIndex > 0)
				CodeEditor.Select(
					CodeEditor.GetFirstCharIndexFromLine(mEx.Position.LineNumber - 1)
						+ mEx.Position.ColumnNumber - 1, 0);
		}

		private void Stop(object sender, EventArgs e)
		{
			MagnetoRunner.StopRunningProcess();
		}

		void SetRunState(bool running)
		{
			RunCommand.CanExecute = !running;
			StopCommand.CanExecute = running;
			CodeEditor.ReadOnly = running;
		}

		private void DisplayLanguageGuide(object sender, EventArgs e)
		{
			string executableFolderPath = Path.GetDirectoryName(SWF.Application.ExecutablePath);
			string languageGuidePath = Path.Combine(executableFolderPath, "Documentation", "langguide.html");

			try
			{
				if (File.Exists(languageGuidePath))
					Process.Start(languageGuidePath);
				else ShowError(
					"The language guide could not be found."
					+ Environment.NewLine + Environment.NewLine
					+ "(langguide.html should be placed in the Documentation sub-folder of the folder where Magneto.exe is placed)");
			}
			catch (Exception exception)
			{
				if (SystemUtilities.IsCritical(exception)) throw;
				else ShowError(exception);
			}
		}
	}
}
