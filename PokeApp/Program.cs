using InjectedServices.Interfaces;
using InjectedServices.Interfaces.Refit;
using InjectedServices.Services;
using Refit;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

string apiConnecttion = builder.Configuration["PokeAPI"] ?? "";
builder.Services.AddControllersWithViews();

// Refit
builder.Services.AddRefitClient<IPokeAPI>()
    .ConfigureHttpClient(c =>
    {
        c.BaseAddress = new Uri(apiConnecttion);
    });

builder.Services.AddScoped<IPokeService, PokeService>();
// Cache de memoria
builder.Services.AddMemoryCache();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Pokemon}/{action=Index}/{id?}");

app.Run();
