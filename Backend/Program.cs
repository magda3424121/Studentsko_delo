using Microsoft.EntityFrameworkCore;
using Studentski_servis.Data;

var builder = WebApplication.CreateBuilder(args);

// Dodajanje kontrolerjev in baze
builder.Services.AddControllers();
builder.Services.AddDbContext<DataContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddCors(options => {
    options.AddPolicy("AllowAll", p => p.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
});

var app = builder.Build();

app.UseCors("AllowAll");

// NASTAVITEV ZA STATIČNE DATOTEKE
app.UseDefaultFiles(); // To poskrbi za index.html
app.UseStaticFiles();  // To poskrbi za vse ostale .html datoteke

app.UseRouting();
app.UseAuthorization();

app.MapControllers();

app.Run();
