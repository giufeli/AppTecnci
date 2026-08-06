using AppTecnici.Server.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// 1. Configurazione DbContext con resilienza
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("MaintenanceDB"),
        sqlServerOptions => sqlServerOptions.EnableRetryOnFailure(
            maxRetryCount: 5,
            maxRetryDelay: TimeSpan.FromSeconds(30),
            errorNumbersToAdd: null)
    ));

// 2. Aggiunta CORS (abilita la comunicazione Client-Server)
builder.Services.AddCors(options => {
    options.AddPolicy("AllowClient", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

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

// --- INIZIO AGGIUNTE PER BLAZOR HOSTED ---
app.UseBlazorFrameworkFiles();
app.UseStaticFiles();
app.UseRouting();
// --- FINE AGGIUNTE PER BLAZOR HOSTED ---

app.UseCors("AllowClient");
app.UseAuthorization();

app.MapControllers();

// --- ISTRUZIONE FALLBACK PER BLAZOR ---
app.MapFallbackToFile("index.html");

app.Run();