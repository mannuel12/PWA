using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using PWA;
using PWA.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped<IndexedDbService>();
builder.Services.AddScoped<CitasApiService>();
builder.Services.AddScoped<CitasSyncService>();

builder.Services.AddScoped<LocalStorageService>();


builder.Services.AddScoped<SaludApiService>();
builder.Services.AddScoped<SaludSyncService>();

builder.Services.AddScoped<TratamientosApiService>();
builder.Services.AddScoped<TratamientosSyncService>();

builder.Services.AddScoped(sp => new HttpClient
{
    BaseAddress = new Uri(builder.HostEnvironment.BaseAddress)
});

await builder.Build().RunAsync();