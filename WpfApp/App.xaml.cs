using System;
using System.IO;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Diagnostics;
using System.Reflection;
using Updater;

namespace WpfApp
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            string executable = "WpfApp.exe";
            var updater = new AppUpdater(AppType.Wpf, new List<string> { executable, "WpfApp.pdb", "Updater.pdb" });
            var isUpdateNeeded = Task.Run(() => updater.IsUpdateNeededAsync(Assembly.GetExecutingAssembly().GetName().Version!)).Result;
            if (isUpdateNeeded)
            {
                if (Task.Run(() => updater.UpdateAsync()).Result)
                {
                    // Start new version
                    Process.Start(Path.Combine(AppContext.BaseDirectory, executable));
                    Environment.Exit(0);
                }
            }

            base.OnStartup(e);
        }
    }
}
