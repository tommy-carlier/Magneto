using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using TC.WinForms.Forms;
using TC.WinForms.Settings;
using System.Threading;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Reflection.Emit;
using TC.Magneto.Modules;
using TC.WinForms.Dialogs;
using TC.WinForms;

using SWF = System.Windows.Forms;
using System.Globalization;

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

		Process runningProcess;
		static readonly object lockTempDirectory = new object();
		static readonly MethodInfo
			exceptionMessageGetter = typeof(Exception).GetProperty("Message").GetGetMethod(),
			foregroundColorSetter = typeof(Console).GetProperty("ForegroundColor").GetSetMethod(),
			consoleWriteLineMethod = typeof(Console).GetMethod("WriteLine", new Type[] { typeof(string) }),
			consoleReadLineMethod = typeof(Console).GetMethod("ReadLine", Type.EmptyTypes);

		private void Run(object sender, EventArgs e)
		{
			SetRunState(true);

			string executableFolderPath = Path.GetDirectoryName(SWF.Application.ExecutablePath);
			string tempFolderPath = Path.Combine(executableFolderPath, "Temp");
			if (!Directory.Exists(tempFolderPath)) Directory.CreateDirectory(tempFolderPath);
			string tempSubFolderPath, tempFileName;
			lock (lockTempDirectory)
			{
				do { tempSubFolderPath = Path.Combine(tempFolderPath, DateTime.Now.ToString("yyyyMMddHHmmssfff", CultureInfo.InvariantCulture)); }
				while (Directory.Exists(tempSubFolderPath));
				Directory.CreateDirectory(tempSubFolderPath);

				File.Copy(Path.Combine(executableFolderPath, "TC.Magneto.dll"), Path.Combine(tempSubFolderPath, "TC.Magneto.dll"), true);
				File.Copy(Path.Combine(executableFolderPath, "TC.Core.dll"), Path.Combine(tempSubFolderPath, "TC.Core.dll"), true);
				string moduleFolderPath = Path.Combine(executableFolderPath, "Modules");
				if (Directory.Exists(moduleFolderPath))
					foreach (string file in Directory.GetFiles(moduleFolderPath))
						File.Copy(file, Path.Combine(tempSubFolderPath, Path.GetFileName(file)), true);
				tempFileName = Path.Combine(tempSubFolderPath, "Temp.exe");
			}

			try
			{
				using (StringReader reader = new StringReader(CodeEditor.Text))
				{
					AssemblyName assemblyName = new AssemblyName("Temp");
					AssemblyBuilder assemblyBuilder = AppDomain.CurrentDomain.DefineDynamicAssembly(assemblyName, AssemblyBuilderAccess.Save, Path.GetDirectoryName(tempFileName));
					ModuleBuilder moduleBuilder = assemblyBuilder.DefineDynamicModule(assemblyName.Name, Path.GetFileName(tempFileName));
					TypeBuilder typeBuilder = moduleBuilder.DefineType("Application", TypeAttributes.NotPublic | TypeAttributes.Sealed);
					MethodBuilder methodBuilder = typeBuilder.DefineMethod("Main", MethodAttributes.Private | MethodAttributes.Static, typeof(void), Type.EmptyTypes);
					ILGenerator generator = methodBuilder.GetILGenerator();

					generator.BeginExceptionBlock();
					MagnetoCompiler.Compile(MagnetoApplication.Current.ModuleManager, reader, generator);
					generator.BeginCatchBlock(typeof(Exception));
					generator.Emit(OpCodes.Callvirt, exceptionMessageGetter);
					generator.Emit(OpCodes.Ldc_I4, (int)ConsoleColor.Red);
					generator.Emit(OpCodes.Call, foregroundColorSetter);
					generator.Emit(OpCodes.Ldstr, "");
					generator.Emit(OpCodes.Call, consoleWriteLineMethod);
					generator.Emit(OpCodes.Call, consoleWriteLineMethod);
					generator.BeginFinallyBlock();
					generator.Emit(OpCodes.Ldc_I4, (int)ConsoleColor.Cyan);
					generator.Emit(OpCodes.Call, foregroundColorSetter);
					generator.Emit(OpCodes.Ldstr, "");
					generator.Emit(OpCodes.Call, consoleWriteLineMethod);
					generator.Emit(OpCodes.Ldstr, "Finished: press <Enter> to exit.");
					generator.Emit(OpCodes.Call, consoleWriteLineMethod);
					generator.Emit(OpCodes.Call, consoleReadLineMethod);
					generator.EndExceptionBlock();
					generator.Emit(OpCodes.Ret);

					//	Generated code:
					//		try
					//		{
					//			// compiled application code
					//		}
					//		catch(Exception lException)
					//		{
					//			Console.ForegroundColor = ConsoleColor.Red;
					//			Console.WriteLine("");
					//			Console.WriteLine(lException.Message);
					//		}
					//		finally
					//		{
					//			Console.ForegroundColor = ConsoleColor.Cyan;
					//			Console.WriteLine("");
					//			Console.WriteLine("Finished: press <Enter> to exit.");
					//			Console.ReadLine();
					//		}

					typeBuilder.CreateType();
					assemblyBuilder.SetEntryPoint(methodBuilder, PEFileKinds.ConsoleApplication);
					assemblyBuilder.Save(Path.GetFileName(tempFileName));
				}

				runningProcess = new Process();
				runningProcess.EnableRaisingEvents = true;
				runningProcess.Exited += HandlerRunningProcessExited;
				runningProcess.StartInfo = new ProcessStartInfo(tempFileName);
				runningProcess.StartInfo.UseShellExecute = false;
				runningProcess.Start();
			}
			catch (Exception exception)
			{
				if (SystemUtilities.IsCritical(exception)) throw;
				ShowError(exception);

                if (exception is MagnetoException magnetoException && magnetoException.Position.CharIndex > 0)
                    CodeEditor.Select(
                        CodeEditor.GetFirstCharIndexFromLine(magnetoException.Position.LineNumber - 1)
                            + magnetoException.Position.ColumnNumber - 1, 0);

                try
				{
					Directory.Delete(tempSubFolderPath, true);
				}
				catch (Exception deleteException)
				{
					if (SystemUtilities.IsCritical(deleteException))
						throw;
				}

				SetRunState(false);
			}
		}

		private void Stop(object sender, EventArgs e)
		{
			if (runningProcess != null)
				runningProcess.Kill();
		}

		void HandlerRunningProcessExited(object sender, EventArgs e)
		{
			string tempSubFolderPath = Path.GetDirectoryName(runningProcess.StartInfo.FileName);
			runningProcess.Exited -= HandlerRunningProcessExited;
			runningProcess.Dispose();
			runningProcess = null;

			try
			{
				Directory.Delete(tempSubFolderPath, true);
			}
			catch (Exception deleteException)
			{
				if (SystemUtilities.IsCritical(deleteException))
					throw;
			}

			Dispatcher.InvokeAsync(this, SetRunState, false);
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
			string languageGuidePath = Path.Combine(executableFolderPath, "langguide.html");

			try
			{
				if (File.Exists(languageGuidePath))
					Process.Start(languageGuidePath);
				else ShowError(
					"The language guide could not be found."
					+ Environment.NewLine + Environment.NewLine
					+ "(langguide.html should be placed in the same folder as Magneto.exe)");
			}
			catch (Exception exception)
			{
				if (SystemUtilities.IsCritical(exception))
					throw;
				else ShowError(exception);
			}
		}
	}
}
