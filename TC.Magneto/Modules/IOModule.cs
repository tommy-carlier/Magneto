using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace TC.Magneto.Modules
{
	/// <summary>Contains functions for input and output.</summary>
	public class IOModule : MagnetoModule
	{
		/// <summary>Is called when the module is stopped.</summary>
		protected override void StopCore()
		{
			foreach (OpenFile file in openFiles.Values)
				file.Close();

			openFiles.Clear();
			freeHandles.Clear();
		}

		/// <summary>Clears the screen.</summary>
		[Function("clearScreen")]
		public void ClearScreen() { Console.Clear(); }

		#region Write(value)

		/// <summary>Writes the specified value to the console.</summary>
		/// <param name="value">The value to write.</param>
		[Function("write")]
		public void Write(string value) { Console.Write(value); }

		/// <summary>Writes the specified value to the console.</summary>
		/// <param name="value">The value to write.</param>
		[Function("write")]
		public void Write(long value) { Console.Write(CoreFunctions.ConvertToString(value)); }

		/// <summary>Writes the specified value to the console.</summary>
		/// <param name="value">The value to write.</param>
		[Function("write")]
		public void Write(decimal value) { Console.Write(CoreFunctions.ConvertToString(value)); }

		/// <summary>Writes the specified value to the console.</summary>
		/// <param name="value">The value to write.</param>
		[Function("write")]
		public void Write(bool value) { Console.Write(CoreFunctions.ConvertToString(value)); }

		/// <summary>Writes the specified value to the console.</summary>
		/// <param name="value">The value to write.</param>
		[Function("write")]
		public void Write(Polarity value) { Console.Write(CoreFunctions.ConvertToString(value)); }

		/// <summary>Writes the specified value to the console.</summary>
		/// <param name="value">The value to write.</param>
		[Function("write")]
		public void Write(Magnetic value) { Console.Write(CoreFunctions.ConvertToString(value)); }

		/// <summary>Writes the specified value to the console.</summary>
		/// <param name="value">The value to write.</param>
		[Function("write")]
		public void Write(Curl value) { Console.Write(CoreFunctions.ConvertToString(value)); }

		/// <summary>Writes the specified value to the console.</summary>
		/// <param name="value">The value to write.</param>
		[Function("write")]
		public void Write(Circuit value) { Console.Write(CoreFunctions.ConvertToString(value)); }

		#endregion

		#region WriteLine(value)

		/// <summary>Writes the specified value to the console, followed by a line terminator.</summary>
		/// <param name="value">The value to write.</param>
		[Function("writeLine")]
		public void WriteLine(string value) { Console.WriteLine(value); }

		/// <summary>Writes the specified value to the console, followed by a line terminator.</summary>
		/// <param name="value">The value to write.</param>
		[Function("writeLine")]
		public void WriteLine(long value) { Console.WriteLine(CoreFunctions.ConvertToString(value)); }

		/// <summary>Writes the specified value to the console, followed by a line terminator.</summary>
		/// <param name="value">The value to write.</param>
		[Function("writeLine")]
		public void WriteLine(decimal value) { Console.WriteLine(CoreFunctions.ConvertToString(value)); }

		/// <summary>Writes the specified value to the console, followed by a line terminator.</summary>
		/// <param name="value">The value to write.</param>
		[Function("writeLine")]
		public void WriteLine(bool value) { Console.WriteLine(CoreFunctions.ConvertToString(value)); }

		/// <summary>Writes the specified value to the console, followed by a line terminator.</summary>
		/// <param name="value">The value to write.</param>
		[Function("writeLine")]
		public void WriteLine(Polarity value) { Console.WriteLine(CoreFunctions.ConvertToString(value)); }

		/// <summary>Writes the specified value to the console, followed by a line terminator.</summary>
		/// <param name="value">The value to write.</param>
		[Function("writeLine")]
		public void WriteLine(Magnetic value) { Console.WriteLine(CoreFunctions.ConvertToString(value)); }

		/// <summary>Writes the specified value to the console, followed by a line terminator.</summary>
		/// <param name="value">The value to write.</param>
		[Function("writeLine")]
		public void WriteLine(Curl value) { Console.WriteLine(CoreFunctions.ConvertToString(value)); }

		/// <summary>Writes the specified value to the console, followed by a line terminator.</summary>
		/// <param name="value">The value to write.</param>
		[Function("writeLine")]
		public void WriteLine(Circuit value) { Console.WriteLine(CoreFunctions.ConvertToString(value)); }

		#endregion

		/// <summary>Reads the next line from the console.</summary>
		/// <returns>The next line from the console.</returns>
		[Function("readLine")]
		public string ReadLine() { return Console.ReadLine(); }

		/// <summary>Opens the specified file to read text from.</summary>
		/// <param name="path">The path of the file to read text from.</param>
		/// <returns>The handle of the opened file.</returns>
		[Function("readFile")]
		public long ReadFile(string path)
		{
			if (string.IsNullOrEmpty(path))
				throw new IOException("Invalid path.");

			TextReader reader = new StreamReader(path);
			long handle = GetFreeHandle();
			openFiles[handle] = new OpenReadFile(reader);
			return handle;
		}

		/// <summary>Opens the specified file to write text to.</summary>
		/// <param name="path">The path of the file to write text to.</param>
		/// <returns>The handle of the opened file.</returns>
		[Function("writeFile")]
		public long WriteFile(string path)
		{
			if (string.IsNullOrEmpty(path))
				throw new IOException("Invalid path.");

			TextWriter writer = new StreamWriter(path, false, Encoding.UTF8);
			long handle = GetFreeHandle();
			openFiles[handle] = new OpenWriteFile(writer);
			return handle;
		}

		/// <summary>Opens the specified file to append text to.</summary>
		/// <param name="path">The path of the file to append text to.</param>
		/// <returns>The handle of the opened file.</returns>
		[Function("appendFile")]
		public long AppendFile(string path)
		{
			if (string.IsNullOrEmpty(path))
				throw new IOException("Invalid path.");

			TextWriter writer = new StreamWriter(path, true, Encoding.UTF8);
			long handle = GetFreeHandle();
			openFiles[handle] = new OpenWriteFile(writer);
			return handle;
		}

		/// <summary>Closes the file with the specified handle.</summary>
		/// <param name="handle">The handle of the handle to close.</param>
		[Function("closeFile")]
		public void CloseFile(long handle)
		{
            if (openFiles.TryGetValue(handle, out OpenFile file))
            {
                file.Close();
                openFiles.Remove(handle);
                freeHandles.Enqueue(handle);
            }
            else throw new IOException("Invalid file handle.");
        }

		#region Write(handle, value)

		/// <summary>Writes the specified value to the specified file.</summary>
		/// <param name="handle">The handle of the file to write to.</param>
		/// <param name="value">The value to write.</param>
		[Function("write")]
		public void Write(long handle, string value)
		{
			if (!string.IsNullOrEmpty(value))
				GetFile(handle).TextWriter.Write(value);
		}

		/// <summary>Writes the specified value to the specified file.</summary>
		/// <param name="handle">The handle of the file to write to.</param>
		/// <param name="value">The value to write.</param>
		[Function("write")]
		public void Write(long handle, long value) { Write(handle, CoreFunctions.ConvertToString(value)); }

		/// <summary>Writes the specified value to the specified file.</summary>
		/// <param name="handle">The handle of the file to write to.</param>
		/// <param name="value">The value to write.</param>
		[Function("write")]
		public void Write(long handle, decimal value) { Write(handle, CoreFunctions.ConvertToString(value)); }

		/// <summary>Writes the specified value to the specified file.</summary>
		/// <param name="handle">The handle of the file to write to.</param>
		/// <param name="value">The value to write.</param>
		[Function("write")]
		public void Write(long handle, bool value) { Write(handle, CoreFunctions.ConvertToString(value)); }

		/// <summary>Writes the specified value to the specified file.</summary>
		/// <param name="handle">The handle of the file to write to.</param>
		/// <param name="value">The value to write.</param>
		[Function("write")]
		public void Write(long handle, Polarity value) { Write(handle, CoreFunctions.ConvertToString(value)); }

		/// <summary>Writes the specified value to the specified file.</summary>
		/// <param name="handle">The handle of the file to write to.</param>
		/// <param name="value">The value to write.</param>
		[Function("write")]
		public void Write(long handle, Magnetic value) { Write(handle, CoreFunctions.ConvertToString(value)); }

		/// <summary>Writes the specified value to the specified file.</summary>
		/// <param name="handle">The handle of the file to write to.</param>
		/// <param name="value">The value to write.</param>
		[Function("write")]
		public void Write(long handle, Curl value) { Write(handle, CoreFunctions.ConvertToString(value)); }

		/// <summary>Writes the specified value to the specified file.</summary>
		/// <param name="handle">The handle of the file to write to.</param>
		/// <param name="value">The value to write.</param>
		[Function("write")]
		public void Write(long handle, Circuit value) { Write(handle, CoreFunctions.ConvertToString(value)); }

		#endregion

		#region WriteLine(handle, value)

		/// <summary>Writes the specified value to the specified file, followed by a line terminator.</summary>
		/// <param name="handle">The handle of the file to write to.</param>
		/// <param name="value">The value to write.</param>
		[Function("writeLine")]
		public void WriteLine(long handle, string value)
		{
			if (!string.IsNullOrEmpty(value))
				GetFile(handle).TextWriter.WriteLine(value);
		}

		/// <summary>Writes the specified value to the specified file, followed by a line terminator.</summary>
		/// <param name="handle">The handle of the file to write to.</param>
		/// <param name="value">The value to write.</param>
		[Function("writeLine")]
		public void WriteLine(long handle, long value) { WriteLine(handle, CoreFunctions.ConvertToString(value)); }

		/// <summary>Writes the specified value to the specified file, followed by a line terminator.</summary>
		/// <param name="handle">The handle of the file to write to.</param>
		/// <param name="value">The value to write.</param>
		[Function("writeLine")]
		public void WriteLine(long handle, decimal value) { WriteLine(handle, CoreFunctions.ConvertToString(value)); }

		/// <summary>Writes the specified value to the specified file, followed by a line terminator.</summary>
		/// <param name="handle">The handle of the file to write to.</param>
		/// <param name="value">The value to write.</param>
		[Function("writeLine")]
		public void WriteLine(long handle, bool value) { WriteLine(handle, CoreFunctions.ConvertToString(value)); }

		/// <summary>Writes the specified value to the specified file, followed by a line terminator.</summary>
		/// <param name="handle">The handle of the file to write to.</param>
		/// <param name="value">The value to write.</param>
		[Function("writeLine")]
		public void WriteLine(long handle, Polarity value) { WriteLine(handle, CoreFunctions.ConvertToString(value)); }

		/// <summary>Writes the specified value to the specified file, followed by a line terminator.</summary>
		/// <param name="handle">The handle of the file to write to.</param>
		/// <param name="value">The value to write.</param>
		[Function("writeLine")]
		public void WriteLine(long handle, Magnetic value) { WriteLine(handle, CoreFunctions.ConvertToString(value)); }

		/// <summary>Writes the specified value to the specified file, followed by a line terminator.</summary>
		/// <param name="handle">The handle of the file to write to.</param>
		/// <param name="value">The value to write.</param>
		[Function("writeLine")]
		public void WriteLine(long handle, Curl value) { WriteLine(handle, CoreFunctions.ConvertToString(value)); }

		/// <summary>Writes the specified value to the specified file, followed by a line terminator.</summary>
		/// <param name="handle">The handle of the file to write to.</param>
		/// <param name="value">The value to write.</param>
		[Function("writeLine")]
		public void WriteLine(long handle, Circuit value) { WriteLine(handle, CoreFunctions.ConvertToString(value)); }

		#endregion

		/// <summary>Reads the next line from the specified file.</summary>
		/// <param name="handle">The handle of the file to read from.</param>
		/// <returns>The next line from the specified file.</returns>
		[Function("readLine")]
		public string ReadLine(long handle)
		{
			return GetFile(handle).TextReader.ReadLine();
		}

		/// <summary>Reads the next characters from the specified file.</summary>
		/// <param name="handle">The handle of the file to read from.</param>
		/// <param name="count">The number of characters to read.</param>
		/// <returns>The next characters from the specified file.</returns>
		[Function("readChars")]
		public string ReadChars(long handle, long count)
		{
			if (count < 0L) return "";
			if (count <= short.MaxValue)
			{
				char[] buffer = new char[(int)count];
				int chars = GetFile(handle).TextReader.Read(buffer, 0, buffer.Length);
				return chars > 0 ? new string(buffer, 0, chars) : "";
			}
			else throw new IOException("count is too large.");
		}

		/// <summary>Reads the entire content of the specified file.</summary>
		/// <param name="handle">The handle of the file to read from.</param>
		/// <returns>The entire content of the specified file.</returns>
		[Function("readAll")]
		public string ReadAll(long handle)
		{
			return GetFile(handle).TextReader.ReadToEnd();
		}

		/// <summary>Indicates whether the end of the file has been reached.</summary>
		/// <param name="handle">The handle of the file.</param>
		/// <returns>If the end of the file has been reached, true; otherwise, false.</returns>
		[Function("endOfFile")]
		public bool EndOfFile(long handle)
		{
			return GetFile(handle).TextReader.Peek() == -1;
		}

		#region file handles

		readonly Dictionary<long, OpenFile> openFiles = new Dictionary<long, OpenFile>();
		readonly Queue<long> freeHandles = new Queue<long>();

		/// <summary>Gets the file with the specified handle.</summary>
		/// <param name="handle">The handle of the file to get.</param>
		/// <returns>The file with the specified handle.</returns>
		OpenFile GetFile(long handle)
		{
            return openFiles.TryGetValue(handle, out OpenFile file)
				? file 
				: throw new IOException("Invalid file handle.");
        }

		/// <summary>Gets a free handle.</summary>
		/// <returns>A free handle.</returns>
		long GetFreeHandle()
		{
			return freeHandles.Count > 0
				? freeHandles.Dequeue()
				: openFiles.Count + 1;
		}

		#endregion

		#region inner classes OpenFile, OpenReadFile and OpenWriteFile

		/// <summary>Represents an opened file.</summary>
		abstract class OpenFile
		{
			/// <summary>Gets a <see cref="T:TextReader"/> to read text from this file.</summary>
			/// <value>The <see cref="T:TextReader"/> to read text from this file.</value>
			public virtual TextReader TextReader { get { throw new NotSupportedException("Cannot read from this file handle."); } }

			/// <summary>Gets a <see cref="T:TextWriter"/> to write text to this file.</summary>
			/// <value>The <see cref="T:TextWriter"/> to write text to this file.</value>
			public virtual TextWriter TextWriter { get { throw new NotSupportedException("Cannot write to this file handle."); } }

			/// <summary>Closes this file.</summary>
			public virtual void Close() { }
		}

		/// <summary>Represents a file, opened for reading.</summary>
		class OpenReadFile : OpenFile
		{
			/// <summary>Initializes a new instance of the <see cref="OpenReadFile"/> class.</summary>
			/// <param name="reader">The <see cref="T:TextReader"/> to read text from.</param>
			public OpenReadFile(TextReader reader) { this.reader = reader; }

			readonly TextReader reader;
			/// <summary>Gets a <see cref="T:TextReader"/> to read text from this file.</summary>
			/// <value>The <see cref="T:TextReader"/> to read text from this file.</value>
			public override TextReader TextReader { get { return reader; } }

			/// <summary>Closes this file.</summary>
			public override void Close() { reader.Close(); }
		}

		/// <summary>Represents a file, opened for writing.</summary>
		class OpenWriteFile : OpenFile
		{
			/// <summary>Initializes a new instance of the <see cref="OpenWriteFile"/> class.</summary>
			/// <param name="writer">The <see cref="T:TextWriter"/> to write text to.</param>
			public OpenWriteFile(TextWriter writer) { this.writer = writer; }

			readonly TextWriter writer;
			/// <summary>Gets a <see cref="T:TextWriter"/> to write text to this file.</summary>
			/// <value>The <see cref="T:TextWriter"/> to write text to this file.</value>
			public override TextWriter TextWriter { get { return writer; } }

			/// <summary>Closes this file.</summary>
			public override void Close() { writer.Close(); }
		}

		#endregion
	}
}
