using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor.Services;
using ShowMoBud;
using ShowMoBud.Services;
using ShowMoBud.Services.Interfaces;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddBlazoredLocalStorage();
builder.Services.AddMudServices();
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("https://localhost:7249/") });
builder.Services.AddScoped<ShowMoBud.Services.DispensaryService>();
builder.Services.AddScoped<INewsletterClient, NewsletterClient>();

await builder.Build().RunAsync();
