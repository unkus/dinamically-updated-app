# dinamically_updated_app

### Linux
- Build
    - `dotnet build`
- Publish (Release produce 1.0.0.1 version and Debug 1.0.0.0 version)
    - `dotnet publish --configuration Release ./ConsoleApp/ConsoleApp.csproj`
    - `dotnet publish --configuration Debug ./ConsoleApp/ConsoleApp.csproj`
- Create version files
    - `echo "1.0.0.1" > ./ConsoleApp/ConsoleApp.version`
- Start Updater Service
    - `dotnet run --project ./UpdaterService/UpdaterService.csproj`
- Start ConsoleApp
    - `./ConsoleApp/bin/Debug/net6.0/win7-x64/publish/ConsoleApp`

### Windows
- Build
    - `dotnet build`
- Publish (Release produce 1.0.0.1 version and Debug 1.0.0.0 version)
    - Console: `dotnet publish --configuration Release .\ConsoleApp\ConsoleApp.csproj`
    - Console: `dotnet publish --configuration Debug .\ConsoleApp\ConsoleApp.csproj`
    - Wpf: `dotnet publish --configuration Release .\WpfApp\WpfApp.csproj`
    - Wpf: `dotnet publish --configuration Debug .\WpfApp\WpfApp.csproj`
- Create version files
    - Console: Put "1.0.0.1" into .\ConsoleApp\ConsoleApp.version
    - Wpf: Put "1.0.0.1" into .\WpfApp\WpfApp.version
- Start Updater Service
    - `dotnet run --project .\UpdaterService\UpdaterService.csproj`
- Start one of app
    - Console: `.\ConsoleApp\bin\Debug\net6.0-windows\win7-x64\publish\ConsoleApp.exe`
    - Wpf: `.\WpfApp\bin\Debug\net6.0-windows\win7-x64\publish\WpfApp.exe`
