using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Reflection.Emit;
using System.Windows.Forms;

namespace TC.Magneto.Shell
{
    static class MagnetoRunner
    {
        static readonly object lockTempDirectory = new object();
        static Process runningProcess;

        public static void Run(string sourceCode, Action<bool> runStateCallback, Action<Exception> exceptionHandler)
        {
            runStateCallback?.Invoke(true);

            string tempFolderPath = CreateTempFolder();
            CopyModules(tempFolderPath);

            try
            {
                string executableFilePath = Path.Combine(tempFolderPath, "Temp.exe");
                BuildAssembly(sourceCode, executableFilePath);
                StartRunProcess(executableFilePath, runStateCallback);
            }
            catch(Exception ex)
            {
                if (SystemUtilities.IsCritical(ex)) throw;
                exceptionHandler?.Invoke(ex);
                CleanupAfterRun(tempFolderPath, runStateCallback);
            }
        }

        public static void StopRunningProcess()
        {
            runningProcess?.Kill();
        }

        private static string CreateTempFolder()
        {
            string tempFolderPath = MagnetoApplication.Current.TempFolderPath;
            if (!Directory.Exists(tempFolderPath)) Directory.CreateDirectory(tempFolderPath);

            string tempSubFolderPath;
            lock (lockTempDirectory)
            {
                do
                {
                    tempSubFolderPath = Path.Combine(tempFolderPath, DateTime.Now.ToString("yyyyMMddHHmmssfff", CultureInfo.InvariantCulture));
                }
                while (Directory.Exists(tempSubFolderPath));
                Directory.CreateDirectory(tempSubFolderPath);
            }

            return tempSubFolderPath;
        }

        private static void CopyModules(string tempFolderPath)
        {
            string executableFolderPath = Path.GetDirectoryName(Application.ExecutablePath);
            CopyModule(Path.Combine(executableFolderPath, "TC.Magneto.dll"), tempFolderPath);
            string moduleFolderPath = Path.Combine(executableFolderPath, "Modules");
            if (Directory.Exists(moduleFolderPath))
            {
                foreach (string file in Directory.EnumerateFiles(moduleFolderPath))
                {
                    CopyModule(file, tempFolderPath);
                }
            }
        }

        private static void CopyModule(string sourceFilePath, string tempFolderPath)
        {
            File.Copy(sourceFilePath, Path.Combine(tempFolderPath, Path.GetFileName(sourceFilePath)), overwrite: true);
        }

        static readonly MethodInfo
            exceptionMessageGetter = typeof(Exception).GetProperty("Message").GetGetMethod(),
            foregroundColorSetter = typeof(Console).GetProperty("ForegroundColor").GetSetMethod(),
            consoleWriteLineMethod = typeof(Console).GetMethod("WriteLine", new Type[] { typeof(string) }),
            consoleReadLineMethod = typeof(Console).GetMethod("ReadLine", Type.EmptyTypes);

        private static void BuildAssembly(string sourceCode, string assemblyFilePath)
        {
            AssemblyName assemblyName = new AssemblyName("Temp");
            AssemblyBuilder assemblyBuilder = AppDomain.CurrentDomain.DefineDynamicAssembly(assemblyName, AssemblyBuilderAccess.Save, Path.GetDirectoryName(assemblyFilePath));
            ModuleBuilder moduleBuilder = assemblyBuilder.DefineDynamicModule(assemblyName.Name, Path.GetFileName(assemblyFilePath));
            TypeBuilder typeBuilder = moduleBuilder.DefineType("Application", TypeAttributes.NotPublic | TypeAttributes.Sealed);
            MethodBuilder methodBuilder = typeBuilder.DefineMethod("Main", MethodAttributes.Private | MethodAttributes.Static, typeof(void), Type.EmptyTypes);
            ILGenerator generator = methodBuilder.GetILGenerator();

            generator.BeginExceptionBlock();
            using (StringReader reader = new StringReader(sourceCode))
            {
                MagnetoCompiler.Compile(MagnetoApplication.Current.ModuleManager, reader, generator);
            }
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
            assemblyBuilder.Save(Path.GetFileName(assemblyFilePath));
        }

        private static void StartRunProcess(string executableFilePath, Action<bool> runStateCallback)
        {
            string tempFolderPath = Path.GetDirectoryName(executableFilePath);
            try
            {
                runningProcess = new Process();
                runningProcess.EnableRaisingEvents = true;
                runningProcess.Exited += delegate
                {
                    runningProcess.Dispose();
                    runningProcess = null;
                    CleanupAfterRun(tempFolderPath, runStateCallback);
                };
                runningProcess.StartInfo = new ProcessStartInfo(executableFilePath);
                runningProcess.StartInfo.UseShellExecute = false;
                runningProcess.StartInfo.WorkingDirectory = tempFolderPath;
                runningProcess.Start();
            }
            catch (Exception ex)
            {
                if (SystemUtilities.IsCritical(ex)) throw;
                CleanupAfterRun(tempFolderPath, runStateCallback);
            }
        }

        private static void CleanupAfterRun(string tempFolderPath, Action<bool> runStateCallback)
        {
            TryDeleteFolder(tempFolderPath);
            runStateCallback?.Invoke(false);
        }

        private static void TryDeleteFolder(string folderPath)
        {
            try
            {
                Directory.Delete(folderPath, true);
            }
            catch (Exception ex)
            {
                if (SystemUtilities.IsCritical(ex)) throw;
            }
        }
    }
}
