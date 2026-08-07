using AppTecnici.Client;
using AppTecnici.Client.Services;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.AspNetCore.Components.Authorization;
using Radzen;
using Blazored.LocalStorage;
using Blazored.SessionStorage;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

// Configura i componenti radice collegando l'app Blazor al DOM dell'HTML
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

// Registra i servizi di archiviazione locale e di sessione per la gestione dei dati offline e del login
builder.Services.AddBlazoredLocalStorage();
builder.Services.AddBlazoredSessionStorage();

// Registra i servizi necessari per il sistema di autorizzazione e autenticazione di Blazor
builder.Services.AddOptions();
builder.Services.AddAuthorizationCore();
builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthenticationStateProvider>();

// Configura il client HTTP basato sull'indirizzo di origine dell'applicazione
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

// Registra i componenti UI di Radzen per l'uso dell'interfaccia grafica
builder.Services.AddRadzenComponents();

// Compila ed avvia l'applicazione Blazor WebAssembly
await builder.Build().RunAsync();