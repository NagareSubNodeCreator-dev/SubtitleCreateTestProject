using SubtitleCreateTestProject;
using Microsoft.Extensions.DependencyInjection;
using Photino.Blazor;

internal class Program
{
    [STAThread]
    private static void Main(string[] args)
    {
        var appBuilder = PhotinoBlazorAppBuilder.CreateDefault(args);
        
        appBuilder.RootComponents.Add<App>("#app");
        appBuilder.Services.AddMasaBlazor();

        var app = appBuilder.Build();

        app.MainWindow
            .SetLogVerbosity(0)
            .SetUseOsDefaultSize(false)
            .Center()
            .SetMinSize(620,550)
            .SetSize(1280,720)
            .SetFileSystemAccessEnabled(true)
            .SetTitle("Nagare Subtitle Blazor test");

        AppDomain.CurrentDomain.UnhandledException += (sender, error) => { };

        app.Run();
    }
}