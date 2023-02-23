using Microsoft.EntityFrameworkCore;
using TechCompilerCo.Models;
using TechCompilerCo.Repositorys;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddScoped<DbSession>();
builder.Services.AddTransient<BaseRepository, BaseRepository>();
builder.Services.AddTransient<LoginRepository, LoginRepository>();
builder.Services.AddTransient<FuncionariosRepository, FuncionariosRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Login}/{action=Index}/{id?}");

app.Run();