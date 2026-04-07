using Microsoft.EntityFrameworkCore;
using Studentski_servis.Data;

var builder = WebApplication.CreateBuilder(args);

// 1. REGISTRACIJA BAZE (To je manjkalo!)
builder.Services.AddDbContext<DataContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// Dodajanje osnovnih servisov
builder.Services.AddControllers(); // Pomembno za kasnejše API kontrolerje
builder.Services.AddOpenApi();

var app = builder.Build();

// 2. KONFIGURACIJA CEVOVODA (Pipeline)
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.UseAuthorization(); // Vedno dobro imeti, tudi če še nimaš prijave

// Mapiranje kontrolerjev (to boš potrebovala za CRUD)
app.MapControllers();

// Spodaj pustiva tvoj testni WeatherForecast primer
var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/weatherforecast", () =>
{
    var forecast = Enumerable.Range(1, 5).Select(index =>
        new WeatherForecast
        (
            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            Random.Shared.Next(-20, 55),
            summaries[Random.Shared.Next(summaries.Length)]
        ))
        .ToArray();
    return forecast;
})
.WithName("GetWeatherForecast");

app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}