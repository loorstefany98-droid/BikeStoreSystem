using BikeStore.Web.Services;

var builder = WebApplication.CreateBuilder(args);

// Agregar servicios al contenedor de inyección de dependencias
builder.Services.AddControllersWithViews();

// Servicio para consumir la API de bicicletas
builder.Services.AddHttpClient<BicicletaApiService>(client =>
{
    var baseUrl = builder.Configuration["ApiSettings:BaseUrl"];
    client.BaseAddress = new Uri(baseUrl!);
});

// Servicio para consumir la API de clientes
builder.Services.AddHttpClient<ClienteApiService>(client =>
{
    var baseUrl = builder.Configuration["ApiSettings:BaseUrl"];
    client.BaseAddress = new Uri(baseUrl!);
});

// Servicio para consumir la API de ventas
builder.Services.AddHttpClient<VentaApiService>(client =>
{
    var baseUrl = builder.Configuration["ApiSettings:BaseUrl"];
    client.BaseAddress = new Uri(baseUrl!);
});

// --- AGREGA ESTE BLOQUE AQUÍ ---
// Servicio para consumir la API de categorías
builder.Services.AddHttpClient<CategoriaApiService>(client =>
{
    var baseUrl = builder.Configuration["ApiSettings:BaseUrl"];
    client.BaseAddress = new Uri(baseUrl!);
});

var app = builder.Build();

// Configurar el flujo de peticiones HTTP
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthorization();
app.MapStaticAssets();

// Ruta por defecto: entra directo al listado de bicicletas
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Bicicletas}/{action=Index}/{id?}")
    .WithStaticAssets();

app.Run();