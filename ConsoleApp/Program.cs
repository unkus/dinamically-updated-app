using System.Diagnostics;
using System.Reflection;
using System.Runtime.InteropServices;
using Updater;

// Пропустим использование логгеров
Console.WriteLine($"Текущая версия: {Assembly.GetExecutingAssembly().GetName().Version}");
Console.WriteLine("Проверяем наличие новой версии");
string executable = RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ? "ConsoleApp.exe" : "ConsoleApp";
var updater = new AppUpdater(AppType.Console, new List<string> { executable, "ConsoleApp.pdb", "Updater.pdb" });
if (await updater.IsUpdateNeededAsync(Assembly.GetExecutingAssembly().GetName().Version!))
{
    Console.WriteLine("Загружаем новую версию");
    if (await updater.UpdateAsync())
    {
        Console.WriteLine("Запускаем новую версию");
        // Start new version
        Process.Start(Path.Combine(AppContext.BaseDirectory, executable));
        Console.WriteLine("Выходим");
        Thread.Sleep(1000);
        Environment.Exit(0);
    }
} else {
    AppUpdater.Cleanup();
}

Console.WriteLine("Нет новой версии - используем текущую");
