using System.IO.Compression;
using System.Reflection;

namespace MyApp.Updater;

public class ProgramUpdater
{
    private readonly Version? _currentVersion = Assembly.GetExecutingAssembly().GetName().Version;
    // UpdaterService
    private readonly Uri _updaterServiceUri = new("https://localhost:7189");

    private readonly string ZIP_FILE_PATH = Path.Combine(AppContext.BaseDirectory, "MyApp.zip");

    private readonly IEnumerable<string> LOCAL_FILES = new List<string> { "MyApp", "MyApp.pdb" };

    public async Task<bool> IsUpdateNeeded()
    {
        // Запрашиваем последнюю версию
        var latestVersion = await GetLatestVersion();
        if(latestVersion != null)
        {
            Console.WriteLine($"Последняя версия: {latestVersion}");
            return _currentVersion < latestVersion;
        }
        else
        {
            Console.WriteLine("Не удалось получить последную версию");
            return false;
        }
    }

    public async Task<bool> Update()
    {
        // Запрашиваем архив с новой версией
        if (await RetrieveNewVersion())
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

    private async Task<Version?> GetLatestVersion()
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
        using var versionResponse = await http.GetAsync("Updater/LatestVersion");
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
        return null;
    }

    public async Task<bool> RetrieveNewVersion()
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
        using var latestResponse = await http.GetAsync("Updater/Latest");
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
        foreach (var item in LOCAL_FILES)
        {
            File.Move(Path.Combine(AppContext.BaseDirectory, item), Path.Combine(AppContext.BaseDirectory, $"{item}.old"), true);
        }
    }

    private void Cleanup()
    {
        // Удаляем архив
        File.Delete(ZIP_FILE_PATH);
        // Удаляем файлы старой версии
        foreach (var item in LOCAL_FILES)
        {
            File.Delete(Path.Combine(AppContext.BaseDirectory, $"{item}.old"));
        }
    }

}