using Microsoft.EntityFrameworkCore;
using ProdutosMvc.Data; 
using ProdutosMvc.Models;
using ProdutosMvc.Repositories.Repositorys; 
using Xunit;

namespace ProdutosMvc.Tests.Repositories
{
	public class ProdutosRepositoryTests : IDisposable
	{
		private readonly AppDbContext _context;
		private readonly ProdutosRepository _repository;

		public ProdutosRepositoryTests()
		{
			var options = new DbContextOptionsBuilder<AppDbContext>()
				.UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
				.Options;
			_context = new AppDbContext(options);
			_repository = new ProdutosRepository(_context);
		}

		[Fact]
		public async Task GetAllAsync_DeveRetornarTodosOsProdutos()
		{
			_context.Produtos.AddRange(
				new Produto { Nome = "Produto 1", Preco = 10.0M },
				new Produto { Nome = "Produto 2", Preco = 20.0M }
			);
			await _context.SaveChangesAsync();

			var produtos = await _repository.GetAllAsync();

			Assert.NotNull(produtos);
			Assert.Equal(2, produtos.Count());
		}

		[Fact]
		public async Task GetByIdAsync_ProdutoExiste_DeveRetornarProduto()
		{
			var produto = new Produto { Nome = "Produto Teste", Preco = 50.0M };
			_context.Produtos.Add(produto);
			await _context.SaveChangesAsync();

			var resultado = await _repository.GetByIdAsync(produto.Id);

			Assert.NotNull(resultado);
			Assert.Equal(produto.Id, resultado.Id);
			Assert.Equal(produto.Nome, resultado.Nome);
		}

		[Fact]
		public async Task PostAsync_DeveAdicionarProdutoNoBanco()
		{
			var produto = new Produto { Nome = "Produto Novo", Preco = 30.0M };

			await _repository.PostAsync(produto);

			var produtoInserido = await _context.Produtos.FirstOrDefaultAsync(p => p.Id == produto.Id);
			Assert.NotNull(produtoInserido);
			Assert.Equal("Produto Novo", produtoInserido.Nome);
		}

		[Fact]
		public async Task PutAsync_DeveAtualizarProduto()
		{
			var produto = new Produto { Nome = "Produto Original", Preco = 40.0M };
			_context.Produtos.Add(produto);
			await _context.SaveChangesAsync();

			produto.Nome = "Produto Atualizado";
			produto.Preco = 45.0M;

			await _repository.PutAsync(produto);

			var produtoAtualizado = await _context.Produtos.FirstOrDefaultAsync(p => p.Id == produto.Id);
			Assert.Equal("Produto Atualizado", produtoAtualizado.Nome);
			Assert.Equal(45.0M, produtoAtualizado.Preco);
		}

		[Fact]
		public async Task DeleteAsync_DeveRemoverProdutoDoBanco()
		{
			var produto = new Produto { Nome = "Produto para Deletar", Preco = 60.0M };
			_context.Produtos.Add(produto);
			await _context.SaveChangesAsync();

			await _repository.DeleteAsync(produto.Id);

			var produtoRemovido = await _context.Produtos.FindAsync(produto.Id);
			Assert.Null(produtoRemovido);
		}

		[Fact]
		public async Task ValidateExistProdutoNameAsync_DeveRetornarTrueSeProdutoExiste()
		{
			var produto = new Produto { Nome = "Produto Existente", Preco = 70.0M };
			_context.Produtos.Add(produto);
			await _context.SaveChangesAsync();

			var existe = await _repository.ValidateExistProdutoNameAsync(produto.Nome);

			Assert.True(existe);
		}

		[Fact]
		public async Task ValidateExistProdutoNameAsync_DeveRetornarFalseSeProdutoNaoExiste()
		{
			var existe = await _repository.ValidateExistProdutoNameAsync("Produto Inexistente");

			Assert.False(existe);
		}

		public void Dispose()
		{
			_context.Database.EnsureDeleted();
			_context.Dispose();
		}
	}
}
