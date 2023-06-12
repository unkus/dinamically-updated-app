using System.Diagnostics;
using System.Reflection;
using MyApp.Updater;

// Пропустим использование логгеров
Console.WriteLine($"Текущая версия: {Assembly.GetExecutingAssembly().GetName().Version}");
Console.WriteLine("Проверяем наличие новой версии");
var updater = new ProgramUpdater();
if (await updater.IsUpdateNeeded())
{
    Console.WriteLine("Загружаем новую версию");
    if (await updater.Update())
    {
        Console.WriteLine("Запускаем новой версии");
        // Start new version
        Process.Start(Path.Combine(AppContext.BaseDirectory, "MyApp"));
        Console.WriteLine("Выходим");
        Thread.Sleep(1000); // Посмотрим что напишет новый процесс
        return;
    }
}

Console.WriteLine("Нет новой версии - используем текущую");
