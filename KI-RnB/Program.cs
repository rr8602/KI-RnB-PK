using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Diagnostics;
using System.Threading;
using System.Reflection;
using System.IO;

namespace KI_RnB
{
    static class Program
    {
        /// <summary>
        /// 해당 응용 프로그램의 주 진입점입니다.
        /// </summary>
        [STAThread]
        static void Main()
        {
           //string proc = Process.GetCurrentProcess().ProcessName;
           //Process[] processes = Process.GetProcessesByName(proc);

           //if (processes.Length > 1)
           //{
           //    //MessageBox.Show("KI-RnB Running", "Running", MessageBoxButtons.OK, MessageBoxIcon.Error);
           //}
           //else
           //{
           //    Application.EnableVisualStyles();
           //    Application.SetCompatibleTextRenderingDefault(false);
           //    Application.Run(new fom_Main());
           //}

           bool createdNew;
           Mutex dup = new Mutex(true, "KI-RnB", out createdNew);
           if (createdNew)
           {
               Application.EnableVisualStyles();
               Application.SetCompatibleTextRenderingDefault(false);
               Application.Run(new fom_Main());
               dup.ReleaseMutex();
           }
           else
           {
               ////중복실행에 대한 처리
               System.Media.SystemSounds.Beep.Play();
               MessageBox.Show("[0] Program Running... System OFF!");
           }
        }

        //// .NET 4.0 이상
        //static Assembly ResolveAssembly(object sender, ResolveEventArgs args)
        //{
        //    Assembly thisAssembly = Assembly.GetExecutingAssembly();
        //    var name = args.Name.Substring(0, args.Name.IndexOf(',')) + ".dll";

        //    var resources = thisAssembly.GetManifestResourceNames().Where(s => s.EndsWith(name));
        //    if (resources.Count() > 0)
        //    {
        //        string resourceName = resources.First();
        //        using (Stream stream = thisAssembly.GetManifestResourceStream(resourceName))
        //        {
        //            if (stream != null)
        //            {
        //                byte[] assembly = new byte[stream.Length];
        //                stream.Read(assembly, 0, assembly.Length);
        //                Console.WriteLine("Dll file load : " + resourceName);
        //                return Assembly.Load(assembly);
        //            }
        //        }
        //    }
        //    return null;
        //}

        // LINQ가 지원되지 않는 .NET 버전
        //static Assembly ResolveAssembly(object sender, ResolveEventArgs args)
        //{
        //    Assembly thisAssembly = Assembly.GetExecutingAssembly();
        //    string resourceName = null;
        //    string fileName = args.Name.Substring(0, args.Name.IndexOf(',')) + ".dll";
        //    foreach (string name in thisAssembly.GetManifestResourceNames())
        //    {
        //        if (name.EndsWith(fileName))
        //        {
        //            resourceName = name;
        //        }
        //    }

        //    if (resourceName != null)
        //    {
        //        using (Stream stream = thisAssembly.GetManifestResourceStream(resourceName))
        //        {
        //            if (stream != null)
        //            {
        //                byte[] assembly = new byte[stream.Length];
        //                stream.Read(assembly, 0, assembly.Length);
        //                Console.WriteLine("Dll file load : " + resourceName);
        //                return Assembly.Load(assembly);
        //            }
        //        }
        //    }
        //    return null;
        //}
    }
}
