using GAMMAFEST.Data;
using GAMMAFEST.Repositorio;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System.Data.Common;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ContextoDb>(options => 
                            options.UseSqlServer(
                                builder.Configuration.GetConnectionString("Connexion")));

builder.Services.AddDbContext<ContextoDb>(ServiceLifetime.Transient);

//builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
//    .AddEntityFrameworkStores<ContextoDb>();

builder.Services.AddIdentity<IdentityUser, IdentityRole>().AddDefaultTokenProviders().AddDefaultUI().AddEntityFrameworkStores<ContextoDb>();

// Add services to the container.
builder.Services.AddControllersWithViews();

//Añadir servicios para las interfaces
builder.Services.AddTransient<IEntradaRepositorio, EntradaRepositorio>();
builder.Services.AddTransient<IEventoRepositorio, EventoRepositorio>();
builder.Services.AddTransient<IPromotorRepositorio, PromotorRepositorio>();
builder.Services.AddTransient<IRegistroAsistenciaRepositorio, RegistroAsistenciaRepositorio>();


//Autenticación
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(50);
});
builder.Services.AddAuthentication(options =>
{
    options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
}).AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, config =>
{
    config.AccessDeniedPath = "/Manage/ErrorAcceso";
});

//Necesitamos indicar un provedor de almacenamiento para tempdata
builder.Services.AddSingleton<ITempDataProvider, CookieTempDataProvider>();

builder.Services.AddControllersWithViews(options => options.EnableEndpointRouting = false).AddSessionStateTempDataProvider();

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("ADMINISTRADORES", policy => policy.RequireRole("ADMIN"));
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

//app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();;

app.UseAuthorization();

app.MapRazorPages();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.Run();
