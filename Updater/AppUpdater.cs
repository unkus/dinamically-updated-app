using System.IO.Compression;
using System.Reflection;

namespace Updater;

public class AppUpdater
{
    // UpdaterService
    private readonly Uri _updaterServiceUri = new("https://localhost:7189");

    private readonly string ZIP_FILE_PATH = Path.Combine(Directory.GetCurrentDirectory(), "tmp.zip");

    private readonly IEnumerable<string> _files;
    private readonly string _latestVersionAction;
    private readonly string _latestAction;

    public AppUpdater(AppType appType, IEnumerable<string> files)
    {
        // Плохая реализация
        _latestVersionAction = $"Updater/Latest{appType}AppVersion";
        _latestAction = $"Updater/Latest{appType}App";
        _files = files;
    }

    public async Task<bool> IsUpdateNeededAsync(Version currentVersion)
    {
        // Запрашиваем последнюю версию
        var latestVersion = await GetLatestVersionAsync();
        if(latestVersion != null)
        {
            return currentVersion < latestVersion;
        }
        else
        {
            Console.WriteLine("Не удалось получить последную версию");
            return false;
        }
    }

    public async Task<bool> UpdateAsync()
    {
        // Запрашиваем архив с новой версией
        if (await RetrieveNewVersionAsync())
        {
            try
            {
                // Сохраняем файлы старой версии
                Backup();

                // Распаковываем
                ZipFile.ExtractToDirectory(ZIP_FILE_PATH, AppContext.BaseDirectory, true);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
            finally
            {
                Cleanup();

            }
        }
        return false;
    }

    private async Task<Version?> GetLatestVersionAsync()
    {
        using var httpHandler = new HttpClientHandler();

        using var http = new HttpClient(httpHandler)
        {
            BaseAddress = _updaterServiceUri
        };
        try
        {
            using var versionResponse = await http.GetAsync(_latestVersionAction);
            if (versionResponse.IsSuccessStatusCode)
            {
                string? versionStr = await versionResponse.Content.ReadAsStringAsync();
                try
                {
                    return Version.Parse(versionStr);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
                return null;
            }
            else
            {
                Console.WriteLine($"Error requesting latest version: {versionResponse.StatusCode} ({versionResponse.ReasonPhrase})");
            }
        }
        catch(Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
        return null;
    }

    public async Task<bool> RetrieveNewVersionAsync()
    {
        using var httpHandler = new HttpClientHandler()
        {
            // Проверка сертификата не входит в текущие задачи
            ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
        };

        using var http = new HttpClient(httpHandler)
        {
            BaseAddress = _updaterServiceUri
        };
        using var latestResponse = await http.GetAsync(_latestAction);
        if (latestResponse.IsSuccessStatusCode)
        {
            using var file = File.OpenWrite(ZIP_FILE_PATH);
            using var stream = await latestResponse.Content.ReadAsStreamAsync();
            // Заполняем локальный файл
            stream.CopyTo(file);

            return true;
        }
        else
        {
            Console.WriteLine($"Error retrieving zip with latest version: {latestResponse.StatusCode} ({latestResponse.ReasonPhrase})");
        }
        return false;
    }

    private void Backup()
    {
        foreach (var item in _files)
        {
            string path = Path.Combine(AppContext.BaseDirectory, item);
            if(File.Exists(path))
            {
                File.Move(path, Path.Combine(AppContext.BaseDirectory, $"{item}.old"), true);
            }
        }
    }

    public void Cleanup()
    {
        // Удаляем архив
        File.Delete(ZIP_FILE_PATH);
        // Удаляем файлы старой версии
        foreach (var item in _files)
        {
            // TODO: не хочу делать отдельный исполняемый файл и не хочу делать через аргументы запуска основного приложения
            // Остается только удаление с задержкой средствами OS?
            // string path = Path.Combine(AppContext.BaseDirectory, $"{item}.old");
            // File.Delete(path);
        }
    }

}