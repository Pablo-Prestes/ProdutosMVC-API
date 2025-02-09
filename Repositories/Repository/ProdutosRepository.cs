using Microsoft.EntityFrameworkCore;
using ProdutosMvc.Data;
using ProdutosMvc.Models;
using ProdutosMvc.Repositories.Interfaces;

namespace ProdutosMvc.Repositories.Repositorys
{
	public class ProdutosRepository : IProdutosRepository
	{
		private readonly AppDbContext _context;

		public ProdutosRepository(AppDbContext context)
		{
			_context = context;
		}

		public async Task<IEnumerable<Produto>> GetAllAsync() =>
			await _context.Produtos.ToListAsync();

		public async Task<Produto> GetByIdAsync(int id) =>
			await _context.Produtos.FindAsync(id);

		public async Task PostAsync(Produto produto)
		{
			await _context.Produtos.AddAsync(produto);
			await _context.SaveChangesAsync();
		}

		public async Task PutAsync(Produto produto)
		{
			_context.Produtos.Update(produto);
			await _context.SaveChangesAsync();
		}

		public async Task DeleteAsync(int id)
		{
			var produto = await GetByIdAsync(id);
			if (produto != null)
			{
				_context.Produtos.Remove(produto);
				await _context.SaveChangesAsync();
			}
		}

		public async Task<bool> ValidateExistProdutoNameAsync(string nome)
		{
			return await _context.Produtos
				.AnyAsync(p => p.Nome.ToUpper() == nome.ToUpper());
		}
	}
}
