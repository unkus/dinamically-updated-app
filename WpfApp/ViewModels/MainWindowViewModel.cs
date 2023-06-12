using System.Reflection;

namespace WpfApp.ViewModels;

public class MainWindowViewModel
{
    public string Greeting { get; }

    public MainWindowViewModel()
    {
        Greeting = $"Hello, World! ({Assembly.GetExecutingAssembly().GetName().Version?.ToString()})";
    } 
}