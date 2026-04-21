using Microsoft.EntityFrameworkCore;
using Studentski_servis.Data;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<DataContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer(); 
builder.Services.AddOpenApi();              

var app = builder.Build();

app.UseDefaultFiles();
app.UseStaticFiles();

app.MapOpenApi();
app.MapScalarApiReference();

app.UseHttpsRedirection();
app.UseAuthorization();

app.MapControllers();

app.Run();