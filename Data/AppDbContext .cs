using Microsoft.EntityFrameworkCore;
using ProdutosMvc.Models;

namespace ProdutosMvc.Data
{
	public class AppDbContext : DbContext
	{
		public AppDbContext(DbContextOptions<AppDbContext> options)
			: base(options) { }

		public DbSet<Produto> Produtos { get; set; }
	}
}
