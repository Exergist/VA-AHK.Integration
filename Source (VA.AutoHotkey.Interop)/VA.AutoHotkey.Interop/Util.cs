using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace VA.AutoHotkey.Interop
{
    internal static class Util
    {
        public static string FindEmbededResourceName(Assembly assembly, string path)
        {
            path = Regex.Replace(path, @"[/\\]", ".");

            if (!path.StartsWith("."))
                path = "." + path;

            var names = assembly.GetManifestResourceNames();

            foreach (var name in names)
            {
                if (name.EndsWith(path))
                {
                    return name;
                }
            }

            return null;
        }

        public static void ExtractEmbededResourceToFile(Assembly assembly, string embededResourcePath, string targetFileName)
        {
            //ensure directory exists
            var dir = Path.GetDirectoryName(targetFileName);

            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);

            using (var readStream = assembly.GetManifestResourceStream(embededResourcePath))
            using (var writeStream = File.Open(targetFileName, FileMode.Create))
            {
                readStream.CopyTo(writeStream);
                readStream.Flush();
            }
        }

        public static bool Is64Bit()
        {
            return IntPtr.Size == 8;
        }
        public static bool Is32Bit()
        {
            return IntPtr.Size == 4;
        }


        public static void EnsureAutoHotkeyLoaded()
        {
            if (dllHandle.IsValueCreated)
                return;

            var handle = dllHandle.Value;
        }

        private static Lazy<SafeLibraryHandle> dllHandle = new Lazy<SafeLibraryHandle>(
            () => Util.LoadAutoHotKeyDll());
        private static SafeLibraryHandle LoadAutoHotKeyDll()
        {
            // Locate and Load 32-bit or 64-bit version of AutoHotkey.dll
            string tempFolderPath = Path.Combine(Path.GetTempPath(), "VA.AutoHotkey.Interop"); //^^MODIFY. Initialize Temp folder path in APPDATA for possibly storing extracted AutoHotkey.dll file
            string RelativePath = null; //^^ADD. Initialize string variable for storing relative path to AutoHotky.dll files
            string path32 = @"x86\" + AutoHotkeyDll.DLLPATH; //^^MODIFY. Initialize relative path for 32-bit AutoHotkey.dll
            string path64 = @"x64\" + AutoHotkeyDll.DLLPATH; //^^MODIFY. Initialize relative path for 64-bit AutoHotkey.dll


            var loadDllFromFileOrResource = new Func<string, SafeLibraryHandle>(ActualPath => //^^MODIFY. Changed "relativePath" to "ActualPath"
            {
                if (File.Exists(ActualPath)) //^^MODIFY. Changed "relativePath" to "ActualPath"
                {
                    //MessageBox.Show("ActualPath found!"); //^^debug
                    return SafeLibraryHandle.LoadLibrary(ActualPath); //^^MODIFY. Changed "relativePath" to "ActualPath"
                }
                else
                {
                    //MessageBox.Show("AutoHotkey.dll library not found!"); //^^debug
                    return null;

                    //^^MODIFY. Removed so as to not deal with embedded resources
                    //var assembly = typeof(AutoHotkeyEngine).Assembly;
                    //var resource = Util.FindEmbededResourceName(assembly, RelativePath);

                    //if (resource != null)
                    //{
                    //    //MessageBox.Show("Extracting embedded resources"); //^^debug
                    //    //var target = Path.Combine(tempFolderPath, RelativePath);
                    //    //Util.ExtractEmbededResourceToFile(assembly, resource, target);
                    //    //return SafeLibraryHandle.LoadLibrary(target);
                    //}

                    //return null;
                }
            });


            if (Util.Is32Bit())
            {
                RelativePath = path32; //^^ADD. Define RelativePath
                return loadDllFromFileOrResource(Path.Combine(Globals.MyAppPath, RelativePath)); //^^MODIFY. return as loadDllFromFileOrResource with combined path string as parameter
            }
            else if (Util.Is64Bit())
            {
                RelativePath = path64; //^^ADD. Define RelativePath
                return loadDllFromFileOrResource(Path.Combine(Globals.MyAppPath, RelativePath)); //^^MODIFY. return as loadDllFromFileOrResource with combined path string as parameter
            }
            else
            {
                return null;
            }
        }
    }
}
