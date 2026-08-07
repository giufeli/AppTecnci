using AppTecnici.Server.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// 1. Configurazione DbContext con strategia di resilienza per Azure SQL (retry in caso di disconnessione)
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("MaintenanceDB"),
        sqlServerOptions => sqlServerOptions.EnableRetryOnFailure(
            maxRetryCount: 5,
            maxRetryDelay: TimeSpan.FromSeconds(30),
            errorNumbersToAdd: null)
    ));

// 2. Configurazione CORS per consentire le chiamate HTTP da parte del client Blazor
builder.Services.AddCors(options => {
    options.AddPolicy("AllowClient", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

// Aggiunta dei controller API con aumento del limite di profondità per la serializzazione JSON
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.MaxDepth = 64;
    });

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.UseHttpsRedirection();
app.UseBlazorFrameworkFiles();
app.UseStaticFiles();
app.UseRouting(); // Gestione dell'instradamento delle richieste
app.UseCors("AllowClient"); // Applicazione della policy CORS definita in precedenza
app.UseAuthorization();
app.MapControllers(); // Mappatura delle rotte per i Controller API
app.MapFallbackToFile("index.html"); // Fallback per l'architettura Single Page Application (SPA): reindirizza al file index.html di Blazor
app.Run(); // Avvio del web server