using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;

namespace VA.AutoHotkey.Interop
{
    /// <summary>
    /// This class expects an AutoHotkey.dll to be available on the machine. (UNICODE) version.
    /// </summary>
    public class AutoHotkeyEngine
    {
        public AutoHotkeyEngine(string inputPath) //^^MODIFY. Add inputPath as parameter, removed AHK thread initialization
        {
            Globals.MyAppPath = inputPath; //^^ADD. Define Globals.AppsPath

            Util.EnsureAutoHotkeyLoaded();
        }

        //^^ADD.
        /// <summary>
        /// Create an AHK thread
        /// </summary>
        /// /// <param name="FileOrScript">String containing new AHK script, file name(with extensino), or file path to be executed. When ommitted an "empty" script is started.</param>
        public void CreateThread(string FileOrScript = "")
        {
            // I haven't found good working examples showcasing the use of Parameters or Options, so they will be set to "" (blank) for simplicity (and mirrors what was done in the original AutoHotkey.Interop)
            string Parameters = "";
            string Options = "";

            if (File.Exists(FileOrScript)) // Check if string FileOrScript is a path to an existing file
                AutoHotkeyDll.ahkdll(FileOrScript, Parameters, Options);
            else if (FileOrScript.EndsWith(".ahk"))
            {
                string filePath = Path.Combine(Globals.MyAppPath, "AHK Scripts", FileOrScript);
                AutoHotkeyDll.ahkdll(filePath, Parameters, Options);
            }
            else
                AutoHotkeyDll.ahktextdll(FileOrScript, Parameters, Options);
        }

        /// <summary>
        /// Gets the value for a variable or an empty string if the variable does not exist.
        /// </summary>
        /// <param name="variableName">Name of the variable.</param>
        /// <returns>Returns the value of the variable, or an empty string if the variable does not exist.</returns>
        public string GetVar(string variableName)
        {
            var p = AutoHotkeyDll.ahkgetvar(variableName, 0);
            return Marshal.PtrToStringUni(p);
        }

        /// <summary>
        /// Sets the value of a variable.
        /// </summary>
        /// <param name="variableName">Name of the variable.</param>
        /// <param name="value">The value to set.</param>
        public void SetVar(string variableName, string value)
        {
            if (value == null)
                value = "";

            AutoHotkeyDll.ahkassign(variableName, value);
        }

        /// <summary>
        /// Evaulates an expression or function and returns the results
        /// </summary>
        /// <param name="code">The code to execute</param>
        /// <returns>Returns the result of an expression</returns>
        public string Eval(string code)
        {
            var codeToRun = "A__EVAL:=" + code;
            AutoHotkeyDll.ahkExec(codeToRun);
            return GetVar("A__EVAL");
        }

        /// <summary>
        /// Loads a file into the running thread
        /// </summary>
        /// <param name="fileName">The file name (including extention) of the script</param> //^^MODIFY.
        /// <param name="filePath">User-provided path to the folder containing fileName (optional)</param> //^^ADD. 
        public void Load(string fileName, string filePath = "default path")
        {            
            if (filePath == "default path") //^^ADD. Check if filePath was not provided
                filePath = Path.Combine(Globals.MyAppPath, "AHK Scripts", fileName); //^^ADD. Define filePath using provided fileName and default script folder
            else //^^ADD. custom filePath WAS provided
                filePath = Path.Combine(filePath, fileName); //^^ADD. Define filePath using provided fileName and script folder path
            AutoHotkeyDll.addFile(filePath, 1, 1);
        }

        /// <summary>
        /// Executes raw ahk code.
        /// </summary>
        /// <param name="code">The code to execute</param>
        public void ExecRaw(string code)
        {
            AutoHotkeyDll.ahkExec(code);
        }

        //^^MODIFY. Added some logic for checking thread state
        /// <summary>
        /// Terminates the running scripts
        /// </summary>
        public void Terminate(int timeout = 0) //^^MODIFY. Defaults to 0 ms timeout
        {
            if (ThreadExists())
            {
                AutoHotkeyDll.ahkTerminate((uint)timeout); //^^MODIFY. Terminate with delay based on timeout (originally 1000 ms)
                Thread.Sleep(100); //^^ADD. Pause execution to ensure clean exit. 
            }
        }

        //^^ADD.
        /// <summary>
        /// Terminate and restart a running script
        /// </summary>
        public void Reload(int timeout = 0)
        {
            if (ThreadExists())
            {
                AutoHotkeyDll.ahkReload((uint)timeout);
                Thread.Sleep(100); // Pause execution to ensure clean exit.
            }
        }

        //^^ADD.
        /// <summary>
        /// Pause/un-pause a thread and run traditional AutoHotkey Sleep internally
        /// </summary>
        /// <param name="state"></param> // Can be set to "O" or "Off" to pause and unpause Autohotkey, respectively.
        public void Pause(string state)
        {
            AutoHotkeyDll.ahkPause(state);
        }

        /// <summary>
        /// Suspends the scripts
        /// </summary>
        public void Suspend()
        {
            ExecRaw("Suspend, On");
        }

        /// <summary>
        /// Unsuspends the scripts
        /// </summary>
        public void UnSuspend()
        {
            ExecRaw("Suspend, Off");
        }

        //^^ADD.
        /// <summary>
        /// Check if AHK thread is active
        /// </summary>
        public bool ThreadExists()
        {
            return AutoHotkeyDll.ahkReady();
        }

        /// <summary>
        /// Executes an already defined function.
        /// </summary>
        /// <param name="functionName">The name of the function to execute.</param>
        /// <param name="param1">The 1st parameter</param>
        /// <param name="param2">The 2nd parameter</param>
        /// <param name="param3">The 3rd parameter</param>
        /// <param name="param4">The 4th parameter</param>
        /// <param name="param5">The 5th parameter</param>
        /// <param name="param6">The 6th parameter</param>
        /// <param name="param7">The 7th parameter</param>
        /// <param name="param8">The 8th parameter</param>
        /// <param name="param9">The 9th parameter</param>
        /// <param name="param10">The 10 parameter</param>
        public string ExecFunction(string functionName,
            string param1 = null,
            string param2 = null,
            string param3 = null,
            string param4 = null,
            string param5 = null,
            string param6 = null,
            string param7 = null,
            string param8 = null,
            string param9 = null,
            string param10 = null)
        {
            IntPtr ret = AutoHotkeyDll.ahkFunction(functionName, param1, param2, param3, param4, param5, param6, param7, param8, param9, param10);

            if (ret == IntPtr.Zero)
                return null;
            else
                return Marshal.PtrToStringUni(ret);
        }

        /// <summary>
        /// Determines if the function exists.
        /// </summary>
        /// <param name="functionName">Name of the function.</param>
        /// <returns>Returns true if the function exists, otherwise false.</returns>
        public bool FunctionExists(string functionName)
        {
            IntPtr funcptr = AutoHotkeyDll.ahkFindFunc(functionName);
            return funcptr != IntPtr.Zero;
        }

        /// <summary>
        /// Executes a label
        /// </summary>
        /// <param name="labelName">Name of the label.</param>
        public void ExecLabel(string labelName)
        {
            AutoHotkeyDll.ahkLabel(labelName, false);
        }

        /// <summary>
        /// Determines if the label exists.
        /// </summary>
        /// <param name="labelName">Name of the label.</param>
        /// <returns>Returns true if the label exists, otherwise false</returns>
        public bool LabelExists(string labelName)
        {
            IntPtr labelptr = AutoHotkeyDll.ahkFindLabel(labelName);
            return labelptr != IntPtr.Zero;
        }
    }

    //^^ADD.
    public class Globals // Class for storing global string AppsPath
    {
        public static string MyAppPath; // Declare AppsPath
    }
}