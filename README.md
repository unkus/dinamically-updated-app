# Dinamically Updated App

NOTE: If you change the version only in the file .version, the update process will be endless.
Applications are created with two versions: 0 at debugging and 1 at release.
This is not very correct, but it seemed useful when debugging.

### Windows
- Build
    - `dotnet build windows.slnf`
- Publish (Release produce 1.0.0.1 version and Debug 1.0.0.0 version)
    - Console: `dotnet publish --configuration Release .\ConsoleApp\ConsoleApp.csproj`
    - Console: `dotnet publish --configuration Debug .\ConsoleApp\ConsoleApp.csproj`
    - Wpf: `dotnet publish --configuration Release .\WpfApp\WpfApp.csproj`
    - Wpf: `dotnet publish --configuration Debug .\WpfApp\WpfApp.csproj`
- Start one of app (UpdaterService offline)
    - Console: `.\ConsoleApp\bin\Debug\net6.0\win7-x64\publish\ConsoleApp.exe`
    - Wpf: `.\WpfApp\bin\Debug\net6.0-windows\win7-x64\publish\WpfApp.exe`
    - See version "1.0.0.0"
- Start Updater Service
    - `dotnet run --project .\UpdaterService\UpdaterService.csproj`
- Start one of app
    - Console: `.\ConsoleApp\bin\Debug\net6.0\win7-x64\publish\ConsoleApp.exe`
    - Wpf: `.\WpfApp\bin\Debug\net6.0-windows\win7-x64\publish\WpfApp.exe`
    - See update steps to version "1.0.0.1"
- Start one of app
    - Console: `.\ConsoleApp\bin\Debug\net6.0\win7-x64\publish\ConsoleApp.exe`
    - Wpf: `.\WpfApp\bin\Debug\net6.0-windows\win7-x64\publish\WpfApp.exe`
    - See version "1.0.0.1" without update process

### Linux
- Build
    - `dotnet build linux.slnf`
- Publish (Release produce 1.0.0.1 version and Debug 1.0.0.0 version)
    - `dotnet publish --configuration Release ./ConsoleApp/ConsoleApp.csproj`
    - `dotnet publish --configuration Debug ./ConsoleApp/ConsoleApp.csproj`
- Start ConsoleApp (UpdaterService offline)
    - `./ConsoleApp/bin/Debug/net6.0/linux-x64/publish/ConsoleApp`
    - See version "1.0.0.0"
- Start Updater Service
    - `dotnet run --project ./UpdaterService/UpdaterService.csproj`
- Start ConsoleApp
    - `./ConsoleApp/bin/Debug/net6.0/linux-x64/publish/ConsoleApp`
    - See update steps to version "1.0.0.1"
- Start ConsoleApp
    - `./ConsoleApp/bin/Debug/net6.0/linux-x64/publish/ConsoleApp`
    - See version "1.0.0.1" without update process
