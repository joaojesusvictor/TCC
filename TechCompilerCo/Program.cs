using Microsoft.EntityFrameworkCore;
using TechCompilerCo.Helper;
using TechCompilerCo.Models;
using TechCompilerCo.Repositorys;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

builder.Services.AddScoped<DbSession>();
builder.Services.AddScoped<ISessao, Sessao>();
builder.Services.AddScoped<IEmail, Email>();
builder.Services.AddTransient<BaseRepository, BaseRepository>();
builder.Services.AddTransient<LoginRepository, LoginRepository>();
builder.Services.AddTransient<FuncionariosRepository, FuncionariosRepository>();
builder.Services.AddTransient<ClientesRepository, ClientesRepository>();
builder.Services.AddTransient<FornecedoresRepository, FornecedoresRepository>();
builder.Services.AddTransient<UsuariosRepository, UsuariosRepository>();
builder.Services.AddTransient<ProdutosRepository, ProdutosRepository>();
builder.Services.AddTransient<VendasRepository, VendasRepository>();
builder.Services.AddTransient<ContasPagarRepository, ContasPagarRepository>();
builder.Services.AddTransient<ContasReceberRepository, ContasReceberRepository>();
builder.Services.AddTransient<GerarOsRepository, GerarOsRepository>();
builder.Services.AddTransient<AniversariantesRepository, AniversariantesRepository>();
builder.Services.AddTransient<ControlarCaixaRepository, ControlarCaixaRepository>();

builder.Services.AddSession(o =>
{
    o.Cookie.HttpOnly = true;
    o.Cookie.IsEssential = true;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.UseSession();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Login}/{action=Index}/{id?}");

app.Run();