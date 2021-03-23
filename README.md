# Magneto

Magneto is a small programming language, created for quickly interfacing with hardware via .NET-based DLLs.
It has a very small IDE that's basically a plain text editor with a Run-command.

## Project structure

The solution has 3 projects:
- `TC.Magneto`: a C# class library that contains the Magneto runtime library, parser and compiler
- `TC.Magneto.Shell`: a C# Windows Forms application that contains the IDE
- `TC.Magneto.Test`: a VB.NET class library that demonstrates how to write a custom Magneto module

## Project dependencies

`TC.Magneto` does not depend on any other library, except what's provided by .NET 4.6.

TC.Magneto.Shell depends on `TC.Core` and `TC.WinForms`. These libraries are included as DLLs. The source code of these library can be found at https://github.com/tommy-carlier/tc-libs

## Installation

To run the IDE, copy the following files into a directory where the user has permission to write and execute programs:
- Magneto.exe
- Magneto.exe.config
- TC.Magneto.dll
- TC.Core.dll
- TC.WinForms.dll
- langguide.html (found in TC.Magneto/Documentation)
- style.css (found in TC.Magneto/Documentation)

Custom Magneto modules are .NET DLLs that provide extra functions and constants to the Magneto language. They have to follow these requirements:
- The file name of the DLL has to end with ".MagnetoModule.dll" (example: `MyModule.MagnetoModule.dll`)
- It has to reference `TC.Magneto.dll`
- The DLL of the module, and all the DLLs it depends on have to be copied to the "Modules" sub-folder of the folder where `Magneto.exe` is stored
- The extra functions and constants it provides are in a class that inherits from `TC.Magneto.Modules.MagnetoModule`
- Each constant it provides is defined as a `Constant`-attribute on the class
- Each function it provides is defined as a `Function`-attribute on a method of the class
- Only the following data types can be used for constant values, function arguments and return values:
  - `System.String` (C# string)
  - `System.Int64` (C# long)
  - `System.Boolean` (C# bool)
  - `System.Decimal` (C# decimal)
  - `TC.Magneto.Polarity`
  - `TC.Magneto.Magnetic`
  - `TC.Magneto.Curl`
  - `TC.Magneto.Circuit`
- The module class can optionally override methods `StartCore` and `StopCore`, which are called at the beginning and end of the program

Here's an example of a custom module class:
```csharp
[Constant("pi", 3.1415)] // provide the constant "pi" to Magneto
public class MyModule : MagnetoModule
{
    [Function("greet")] // provide the function "greet" to Magneto
    public string Greet(string name)
    {
        return "Hello " + name;
    }
    
    [Function("swap")] // passing arguments by reference is supported
    public void Swap(ref string a, ref string b)
    {
        string temp = a;
        a = b;
        b = temp;
    }
}
```

## Running a Magneto-program
When you run a Magneto-program from the IDE, the IDE will do the following:
- create a new sub-folder in the current user's temporary folder (`%TEMP%/Magneto/{timestamp}`)
- copy `TC.Magneto.dll` to this folder
- copy all the files from the "Modules" sub-folder to this folder
- compile the Magneto-code to "Temp.exe" in this folder
- run the compiled executable, and wait for it to exit
- try to delete the folder after the process exits

The compiled executable does the following steps when running:
- for each built-in and custom module: create an instance of the module and call its `Start`-method
- execute the compiled Magneto-code, using the module instances for custom functions and constants
- for each built-in and custom module: call its `Stop`-method
- when an exception occurs, write its error message to the console, in red
- write "Finished: press Enter to exit" to the console, in cyan
- wait until the user presses Enter
