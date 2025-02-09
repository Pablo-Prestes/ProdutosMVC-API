using FluentMigrator.Runner;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using ProdutosMvc.Data;
using ProdutosMvc.Migrations;
using ProdutosMvc.Models;
using ProdutosMvc.Repositories.Interfaces;
using ProdutosMvc.Repositories.Repositorys;
using ProdutosMvc.Service.interfaces;
using ProdutosMvc.Service.Services;
using ProdutosMvc.Validator;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

#region Conexão com o banco de dados && Configurações do Assembly de migração com o banco de dados
builder.Services.AddDbContext<AppDbContext>(options =>
	options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

//
builder.Services.AddFluentMigratorCore()
	.ConfigureRunner(rb => rb
		.AddPostgres()
		.WithGlobalConnectionString(builder.Configuration.GetConnectionString("DefaultConnection"))
		.ScanIn(typeof(Migration_070202025_Inicial).Assembly).For.Migrations())
	.AddLogging(lb => lb.AddFluentMigratorConsole());

#endregion

#region Services
builder.Services.AddScoped<IProdutoService, ProdutoService>();
#endregion

#region Repositories
builder.Services.AddScoped<IProdutosRepository, ProdutosRepository>();
#endregion

#region Validators
builder.Services.AddTransient<IValidator<Produto>, ProdutoValidator>();
#endregion

var app = builder.Build();

CreateDatabase(app);

#region Aplicar migrações no banco de dados
using (var scope = app.Services.CreateScope())
{
	var runner = scope.ServiceProvider.GetRequiredService<IMigrationRunner>();
	runner.MigrateUp(); 
}
#endregion

// Configuração do pipeline HTTP
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
	pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();

#region Criar Database
static void CreateDatabase(WebApplication app)
{
	var serviceScope = app.Services.CreateScope();
	var dataContext = serviceScope.ServiceProvider.GetService<AppDbContext>();
	dataContext?.Database.Migrate();
}
#endregion
