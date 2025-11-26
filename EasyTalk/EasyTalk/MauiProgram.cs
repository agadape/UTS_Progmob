using EasyTalk.Services;
using EasyTalk.Shared.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;



namespace EasyTalk
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                });

            // Device-specific service
            builder.Services.AddSingleton<IFormFactor, FormFactor>();

            builder.Services.AddMauiBlazorWebView();

#if DEBUG
            builder.Services.AddBlazorWebViewDeveloperTools();
            builder.Logging.AddDebug();
#endif

            // ✅ Tambahkan ini
            builder.Services.AddScoped<APIServices>();
            builder.Services.AddScoped<ICameraService, MauiCameraService>();
            builder.Services.AddHttpClient<APIServices>(client =>
            {
                client.BaseAddress = new Uri("https://localhost:7177/"); // sesuaikan port backend
            });

            return builder.Build();
        }
    }
}
