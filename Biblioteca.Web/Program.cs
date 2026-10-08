using Biblioteca.Web.Repositorios;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

// Repositorios (inyección de dependencias)
builder.Services.AddScoped<LibroRepositorio>();
builder.Services.AddScoped<SocioRepositorio>();
builder.Services.AddScoped<PrestamoRepositorio>();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Libros}/{action=Index}/{id?}");

app.Run();