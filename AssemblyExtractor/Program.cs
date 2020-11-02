using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

using System.IO;
using System.Windows.Forms;
using System.Threading;

namespace AssemblyExtractor
{
    class Program
    {
        static void Main(string[] args)
        {
            string outDir = Directory.GetCurrentDirectory() + @"\FoundAssemblies\";
            if (!Directory.Exists(outDir))
                Directory.CreateDirectory(outDir);
            int counter = 0;

            Console.WriteLine("Select folder to be checked");

            Thread t = new Thread((ThreadStart)(() => {

                FolderBrowserDialog fbd = new FolderBrowserDialog();

               

                fbd.ShowDialog();
                string inDir = fbd.SelectedPath;
                Console.WriteLine("SELECTED: " + inDir);

                List<string> files = GetValidFiles(inDir);
                Console.WriteLine($"Total files: {files.Count}");

                foreach (string filepath in files)
                {
                    string name = Path.GetFileName(filepath);
                    string outFile = $@"{outDir}\{name}";

                    Console.WriteLine($"Assembly found: {name}");

                    File.Copy(filepath, outFile, true);
                }

            Console.WriteLine($"Finished!");
                Console.ReadLine();
            }));

            t.SetApartmentState(ApartmentState.STA);
            t.Start();
            t.Join();


        }
        public static List<string> GetValidFiles(string path)
        {
            List<string> files = new List<string>();
            string[] dlls = Directory.GetFiles(path, "*", SearchOption.AllDirectories);
            for (int i = 0; i < dlls.Length; i++)
                if (IsValidFile(dlls[i]))
                    files.Add(dlls[i]);

            return files;
        }

        public static bool IsValidFile(string filepath)
        {
            bool isValid = false;

            try
            {
                var asm = AssemblyName.GetAssemblyName(filepath);
                isValid = true;
            }
            catch
            {
                isValid = false;
            }

            return isValid;
        }
    }
}
