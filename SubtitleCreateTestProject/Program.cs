using System.Reflection;
using System.Text.Json;
using Masa.Blazor;
using Masa.Blazor.Core.I18n;
using Microsoft.Extensions.DependencyInjection;
using Photino.Blazor;

namespace SubtitleCreateTestProject;

internal static class Program
{
    [STAThread]
    private static void Main(string[] args)
    {
        var appBuilder = PhotinoBlazorAppBuilder.CreateDefault(args);

        appBuilder.RootComponents.Add<App>("#app");
        appBuilder.Services.AddMasaBlazor(options => { options.Locale = new Locale("zh-CN"); })
            .AddI18NForEmbeddedResource();

        var app = appBuilder.Build();

        app.MainWindow
            .SetLogVerbosity(0)
            .SetUseOsDefaultSize(false)
            .Center()
            .SetMinSize(620, 550)
            .SetSize(1280, 720)
            .SetFileSystemAccessEnabled(true)
            .SetTitle("Nagare Subtitle Blazor test");

        // AppDomain.CurrentDomain.UnhandledException += (sender, error) => { };

        app.Run();
    }
}

public static class MasaBlazorBuilderExtensions
{
    public static IMasaBlazorBuilder AddI18NForEmbeddedResource(this IMasaBlazorBuilder builder)
    {
        var assembly = Assembly.GetExecutingAssembly();
        var namespaceString = assembly.GetName().Name;
        var i18FolderName = $"{namespaceString}.I18n";
        using var streamCultures = assembly.GetManifestResourceStream($"{i18FolderName}.supportedCultures.json");
        using var readerCultures = new StreamReader(streamCultures ??
                                                    throw new InvalidOperationException(
                                                        "Failed to read supportedCultures json file data!"));
        var contents = readerCultures.ReadToEnd();
        var cultures = JsonSerializer.Deserialize<string[]>(contents);
        List<(string cultures, Dictionary<string, string>)> locales = [];

        foreach (var culture in cultures!)
        {
            using var stream = assembly.GetManifestResourceStream($"{i18FolderName}.{culture}.json");
            if (stream == null) continue;
            using var reader = new StreamReader(stream);
            var map = I18nReader.Read(reader.ReadToEnd());
            locales.Add((culture, map));
        }

        builder.AddI18n(locales.ToArray());
        return builder;
    }
}