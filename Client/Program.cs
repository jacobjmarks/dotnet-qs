using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Options;
using MudBlazor.Services;
using Client;
using MudBlazor;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddMudServices(config =>
{
    config.SnackbarConfiguration.PositionClass = Defaults.Classes.Position.TopCenter;
    config.SnackbarConfiguration.NewestOnTop = true;
});

builder.Services.Configure<AppConfig>(builder.Configuration.Bind);
builder.Services.AddScoped<AppConfig>(services => services.GetRequiredService<IOptions<AppConfig>>().Value);

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

builder.Services.AddSingleton<ClipboardService>();
builder.Services.AddSingleton<LocalStorageService>();

await builder.Build().RunAsync();
