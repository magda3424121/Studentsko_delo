using Microsoft.EntityFrameworkCore;
using Studentski_servis.Data;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// 1. Povezava z bazo
builder.Services.AddDbContext<DataContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// 2. Dodajanje vseh potrebnih servisov
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer(); // Nujno za prepoznavo poti
builder.Services.AddOpenApi();              // Za .NET 9/10 dokumentacijo

var app = builder.Build();

// 3. Omogoči Scalar in OpenAPI (brez "if" pogoja, da bo 100% delalo)
app.MapOpenApi();
app.MapScalarApiReference();

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

// 4. Testna pot, da takoj vidiš, če API sploh "živi"
app.MapGet("/", () => "API deluje! Pojdi na /scalar/v1 za vmesnik.");

app.Run();