using System;
using System.IO;
using System.Threading;
using System.Windows.Forms;

namespace Examples
{
    class Program
    {
        static void Main(string[] args)
        {
            #region 1. Initialize AutoHotkey (AHK) functionality
            // Locate the required AHK library (dll) and create an instance for accessing its functionality

            #region 1.1 Determine path to "VA-AHK.Integration" directory in VoiceAttack Apps folder
            // This is needed to locate the included 32 or 64-bit AutoHotkey.dll files as well as other resources
            ///string AppsPath = VA.ParseTokens("{VA_APPS}"); // Obtain directory path to VoiceAttack "Apps" folder
            string AppsPath = @"C:\Program Files (x86)\VoiceAttack\Apps"; // Store directory path to VoiceAttack "Apps" folder
            string MyAppName = "VA-AHK.Integration"; // Initialize string variable for storing relevant App (folder) name
            string MyAppPath = Path.Combine(AppsPath, MyAppName); // Initialize variable for storing path to relevant App directory
            #endregion

            #region 1.2 Initialize AHK functionality
            var ahk = new VA.AutoHotkey.Interop.AutoHotkeyEngine(MyAppPath); // Create an AutoHotkeyEngine instance associated with ahk variable
            #endregion

            #endregion

            #region 2. Create an AHK thread that runs an empty AHK script
            ahk.CreateThread(""); // Creates an AHK thread that runs an empty, #Persistent, and #NoTrayIcon AHK script. Also terminates any other active AHK threads.
            #endregion

            #region 3. Execute and manipulate AHK scripts or raw code using active AHK thread
            /* The methods in this section for executing and manipulating AHK scripts should meet the needs of 95% of users.
            As long as an AHK thread is active the code in this section will work.
            Note that AHK processing in this section will be performed in series with the C# code ==> C# will wait for AHK code to finish before continuing. */

            // Execute any raw AHK code
            ahk.ExecRaw("MsgBoxd, Hello World!");

            // Create new AHK hotkeys
            ahk.ExecRaw("^a::Send, Hello World!"); // CTRL+A types "Hello World!"

            // Suspends all hotkeys and hotstrings (with exceptions, see official AHK documentation for the Suspend command)
            ahk.Suspend();

            // Previously defined "Hello World" hotkey should be disabled now

            // Unsuspend all hotkeys and hotstrings
            ahk.UnSuspend();

            // Set variables in AHK
            ahk.SetVar("x", "1");
            ahk.SetVar("y", "4");
            ahk.SetVar("MyString", "Hello");

            // Execute statements in AHK
            ahk.ExecRaw("z:=x+y");
            ahk.ExecRaw(@"MyString := MyString . "" World!"""); // Note that double quotes ("") are needed to encapsulate literal strings
            ahk.ExecRaw("MsgBox, Today is %A_DDDD% %A_MMMM% %A_DD% %A_YYYY%");
            ahk.ExecRaw("Menu, Tray, Icon"); // Overrides the default #NoTrayIcon and makes the AHK icon appear in the tray
            ahk.ExecRaw("MyScriptName:=A_ScriptName"); // Use this method to store built-in AHK variables within named variables for later AHK processing or retrieval

            // Retrieve variables from AHK
            string zValue = ahk.GetVar("z");
            ///VA.WriteToLog("Value of z = " + zValue); // "Value of z = 5"
            Console.WriteLine("Value of z = {0}", zValue);
            ///VA.WriteToLog("Value of MyString = " + ahk.GetVar("MyString")); // "Value of MyString = Hello World!"
            Console.WriteLine("Value of MyString = " + ahk.GetVar("MyString"));
            ///VA.WriteToLog("AHK version = " + ahk.GetVar("A_AhkVersion")); // AHK built-in variables can be retrieved directly using ahk.GetVar
            Console.WriteLine("AHK version = " + ahk.GetVar("A_AhkVersion")); // AHK built-in variables can be retrieved directly using ahk.GetVar
            ///VA.WriteToLog("ScriptName = " + ahk.GetVar("MyScriptName")); // The empty script uses "AutoHotkey.dll" as its script name
            Console.WriteLine("ScriptName = " + ahk.GetVar("MyScriptName")); // The empty script uses "AutoHotkey.dll" as its script name

            // Send AHK value back to VA as a variable
            ///VA.SetInt("~myInt", Convert.ToInt32(zValue));

            // Program and execute statements in AHK with formatting similar to what is inputted natively in AHK scripts
            // Everything between the double quotes contains the AHK script
            ahk.ExecRaw(@"
                a := 6
                b := 7
                c := a + b
                MyLongerString := ""This is another string.""
            "); // Note that double quotes ("") are needed to encapsulate literal strings in the above method
            string cValue = ahk.GetVar("c"); // Retrieve the value of "c" from AHK and store in cValue string
            ///VA.WriteToLog("Value of c = " + cValue); // "Value of c = 13"
            Console.WriteLine("Value of c = {0}", cValue); // "Value of c = 13"
            ///VA.WriteToLog("Value of MyLongerString = " + ahk.GetVar("MyLongerString")); // "Value of MyLongerString = This is another string."
            Console.WriteLine("Value of MyLongerString = " + ahk.GetVar("MyLongerString")); // "Value of MyLongerString = This is another string."
            ahk.ExecRaw(@"
                counter=3
                Loop, %counter%
                {
	                TimerString:=""Message box countdown: "" . (counter-A_Index+1) . "" s""
                    MsgBox,, AHK Processing, %TimerString%, 1
                    Sleep, 1000
                }
                 MsgBox,,AHK Processing, C# execution will now continue
            "); // Demonstrates how C# will wait for AHK processing to complete before continuing

            //Perform additional calculations with previously defined variables in both AHK and C#
            ahk.ExecRaw(@"
                d = 2
                c := c + d
                MyString := MyString . "" "" . ""It is a good string.""
            ");
            cValue = ahk.GetVar("c"); // Retrieve the value of "c" from AHK and store in cValue string
            ///VA.WriteToLog("Value of c = " + cValue); // "Value of c = 15"
            Console.WriteLine("Value of c = " + cValue); // "Value of c = 15"
            ///VA.WriteToLog("Value of MyString = " + ahk.GetVar("MyString")); // "Value of MyString = Hello World! It is a good string."
            Console.WriteLine("Value of MyString = " + ahk.GetVar("MyString")); // "Value of MyString = Hello World! It is a good string."
            int c = Convert.ToInt32(cValue); // Convert string cValue to integer and store in c
            int e = c + 2; // Initialize integer e
            ///VA.WriteToLog("Value of e = " + e); // "Value of e = 17"
            Console.WriteLine("Value of e = " + e); // "Value of e = 17"

            // Load a library or execute scripts found within "AHK Scripts" folder inside of "VA-AHK.Integration" directory
            ahk.Load("HelloWorld.ahk");

            // Load a library or execute scripts found within a specified directory
            //string MyFileDirectory = @"C:\Program Files (x86)\VoiceAttack\Apps\VA-AHK.Integration\AHK Scripts\My Directory"; // Just an example to show the syntax needed to provide a raw directory path
            string MyFileDirectory = Path.Combine(MyAppPath, "AHK Scripts", "My Directory"); // Example path that pulls from "My Directory" folder inside the "AHK Scripts" folder
            ahk.Load("HelloWorld2.ahk", MyFileDirectory);

            // Determine if a specific function exists (has been loaded) within the current AHK thread and execute it if it exists
            ahk.Load("functions.ahk"); // Load the script "functions.ahk"
            if (ahk.FunctionExists("MyFunction") == true) // Check if "MyFunction" function exists within the current AHK thread. Should return TRUE since the function "MyFunction" is found within "functions.ahk" that was previously loaded.
                ahk.ExecFunction("MyFunction", "Hello", "World"); // Execute "MyFunction" (with 2 parameters passed). ExecFunction accepts 1-10 parameters.

            // Eval also returns results from functions already loaded in the current AHK thread
            // The number of parameters that can be passed to AHK via Eval may not be as limited as it is for ExecFunction
            string addResults = ahk.Eval("Add(5, 1)"); // Execute the function "Add" (found within "functions.ahk" which was previously loaded) with 2 parameters passed and store the value in the string addResults
            ///VA.WriteToLog("Eval: Result of Add function = " + addResults); // Ouptut value stored in add5Results. "Eval: Result of Add function = 6"
            Console.WriteLine("Eval: Result of Add function = " + addResults); // Ouptut value stored in add5Results. "Eval: Result of Add function = 6"

            // Determine if a specific label exists (has been loaded) within the current AHK thread and execute it if it exists
            if (ahk.LabelExists("DOSTUFF") == true) // Check if "DOSTUFF" label exists within the current AHK thread. Should return TRUE since "DOSTUFF" label is found within "functions.ahk" that was previously loaded.
                ahk.ExecLabel("DOSTUFF"); // Execute "DOSTUFF" label

            // Create a new function with formatting similar to what is inputted natively in AHK scripts, then execute it
            // The raw AHK code is first stored within a C# string variable
            string SayHelloFunction = @"
                SayHello(name)
                {
                    MsgBox, Hello %name%
                    return
                }
            "; // Note that the SayHello AHK function requires a "name" parameter
            MessageBox.Show(SayHelloFunction); // Useful for viewing the string that will be written to the AHK thread
            ahk.ExecRaw(SayHelloFunction); // This "writes" the created function to the AHK thread
            ahk.ExecFunction("SayHello", "Chester"); // Execute "SayHello" function while passing it "Chester" as the "name" parameter
            ahk.ExecRaw(@"SayHello(""Fluffy"")"); // Another way to execute "SayHello" function while passing it "Fluffy" as the "name" parameter

            // Pause the current AHK thread (though hotkeys and hotstrings will still be active). See official AHK documentation for the "Pause" command.
            ahk.Pause("On");

            // Unpause the current AHK thread
            ahk.Pause("Off");

            // Reload current thread (terminate and restart it) based on parameters used with CreateThread (in section 2)
            // Note that all the scripts and raw code executed in this section were built on top of an empty and persistent script ==> ahk.CreateThread("")
            // Therefore reloading the thread will remove all the previous scripts and code and restart the empty script
            // If you need to directly reload a running script (stop and then restart the script) it must be executed using the method in Section 4
            ahk.Reload(100); // Reload active AHK thread with a wait time of 100 ms. If no number is provided the wait time will be 0.

            // Terminate active AHK thread and thereby stop all AHK scripts.
            // You can specify a time in milliseconds to wait for the thread to terminate. Negative values force an exit after the time is reached. Positive values may take longer but ensures a clean exit. 0 = no time out.
            // If you do not include this at the end of the inline function then any AHK threads, scripts, hotkeys, etc. previously executed will continue to run in the background until VA closes.
            // Pressing the "Stop All Commands" button on the main VA menu will NOT stop AHK processing that is running on an existing background AHK thread.
            // Note though that running a background AHK thread from within VA may have its uses. See the Multi-Function Example for a demonstration of using a background AHK process. 
            ahk.Terminate(); // Terminate active AHK thread with a wait time of 100 ms. If no number is provided the wait time will be 0.
            #endregion

            #region 4. Second method for executing and manipulating AHK scripts
            /*  There are a few important characteristics about using the method outlined in Section 3 for executing AHK scripts and code:
                1.) AHK processing is executed in series with the C# code
                2.) Scripts and code cannot be reloaded since the base script (which is the foundation for the rest of the code) that gets reloaded starts out empty
                Point 1 is likely advantageous for most applications since it offers more control and ease for communicating back and forth between VA, C#, and AHK.
                Point 2 can be overcome with C# conditional logic as needed. 
                However for the sake of demonstrating all the possible functionality the below examples outline how to execute AHK code or scripts that are directly tied to a thread. */

            // Create new AHK thread that runs AHK code. Also terminates any other active AHK threads.
            // Note that as soon as the message box is closed the AHK code execution finishes and the thread is automatically terminated
            // Also note that code executed by this method will use "AutoHotkey.dll" as its script name
            // Just to reiterate, running AHK code (or a script file) directly through the thread as shown above will cause C# and AHK to process in parallel
            ahk.CreateThread("MsgBox,,%A_ScriptName%, Hello World!");

            // Perform additional processing
            Thread.Sleep(2000); // Pause C# for 2000 ms. Inserted to keep this section from turning into a message box mess.
            MessageBox.Show(ahk.GetVar("A_ScriptName") + " Thread Exists = " + ahk.ThreadExists().ToString()); // Report if an AHK thread is running
            ahk.Terminate(); // Terminate current AHK thread (if available)

            // Create new AHK thread that runs a script found in the "AHK Scripts" folder. Also terminates any other active AHK threads.
            ahk.CreateThread("MyScript.ahk");

            // Perform additional processing
            Thread.Sleep(2000);
            MessageBox.Show("MyScript.ahk has hotkeys and is effectively #Persistent, so its thread won't terminate even after the user closes the AHK message box.");
            MessageBox.Show(ahk.GetVar("A_ScriptName") + " Thread Exists = " + ahk.ThreadExists().ToString()); // Report if an AHK thread is running

            // Terminate current thread and restart it based on parameters used with CreateThread (in section 4)
            // Note that MyScript.ahk CAN be reloaded since it was launched when the thread was created AND the associated thread is still active
            ahk.Reload();

            // Perform additional processing
            Thread.Sleep(2000);
            MessageBox.Show(ahk.GetVar("A_ScriptName") + " Thread Exists = " + ahk.ThreadExists().ToString()); // Report if an AHK thread is running

            // Create an AHK thread that runs a script found in the "My Directory" folder within "AHK Scripts" (or any other directory besides "AHK Scripts"). Also terminates any other active AHK threads.
            string MyScriptPath = Path.Combine(MyAppPath, "AHK Scripts", "My Directory", "HelloWorld2.ahk"); // Again, any AHK script file path will work here
            ahk.CreateThread(MyScriptPath);

            // Perform additional processing
            while (ahk.ThreadExists() == true) { } // Do nothing while waiting for the previous AHK thread to terminate. This prevents the C# code from finishing before AHK is done with its processing.
            ahk.Terminate(); // Terminate active AHK thread (if available) and thereby ensure all AHK scripts are stopped
            #endregion

            ///VA.WriteToLog("Single function VA-AHK.Integration example complete.");
            Console.WriteLine("Single function VA-AHK.Integration example complete.");
            Console.ReadLine();
        }
    }
}

// Some of these examples were pulled or modified from AutoHotkey.Interop
