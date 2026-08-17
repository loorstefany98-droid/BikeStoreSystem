using Microsoft.EntityFrameworkCore;
using BikeStore.Datos;

var builder = WebApplication.CreateBuilder(args);

// agregar servicios al contenedor

builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
});
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

// Tienes que agregar esta configuración para que la API despierte la base de datos
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
// (Asegúrate de que el nombre "DefaultConnection" coincida con el de tu archivo appsettings.json)

var app = builder.Build();



// configuramos HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
