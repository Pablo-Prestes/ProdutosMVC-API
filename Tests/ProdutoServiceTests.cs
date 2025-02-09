using Moq;
using ProdutosMvc.Models;
using ProdutosMvc.Repositories.Interfaces;
using ProdutosMvc.Service.Services;
using Xunit;

namespace ProdutosMvc.Tests.Services
{
	public class ProdutoServiceTests
	{
		private readonly Mock<IProdutosRepository> _repositoryMock;
		private readonly ProdutoService _produtoService;

		public ProdutoServiceTests()
		{
			_repositoryMock = new Mock<IProdutosRepository>();
			_produtoService = new ProdutoService(_repositoryMock.Object);
		}

		[Fact]
		public async Task GetAllAsync_DeveRetornarTodosOsProdutos()
		{
			var produtos = new List<Produto>
			{
				new Produto { Id = 1, Nome = "Produto 1", Preco = 10.0M },
				new Produto { Id = 2, Nome = "Produto 2", Preco = 20.0M }
			};
			_repositoryMock.Setup(r => r.GetAllAsync()).ReturnsAsync(produtos);

			var resultado = await _produtoService.GetAllAsync();

			Assert.NotNull(resultado);
			Assert.Equal(2, resultado.Count());
			_repositoryMock.Verify(r => r.GetAllAsync(), Times.Once);
		}

		[Fact]
		public async Task GetByIdAsync_ProdutoExiste_DeveRetornarProduto()
		{
			var produto = new Produto { Id = 1, Nome = "Produto 1", Preco = 10.0M };
			_repositoryMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(produto);

			var resultado = await _produtoService.GetByIdAsync(1);

			Assert.NotNull(resultado);
			Assert.Equal(1, resultado.Id);
			_repositoryMock.Verify(r => r.GetByIdAsync(1), Times.Once);
		}

		[Fact]
		public async Task PostAsync_DeveChamarMetodoDoRepositorio()
		{
			var produto = new Produto { Nome = "Produto Novo", Preco = 15.0M };

			await _produtoService.PostAsync(produto);

			_repositoryMock.Verify(r => r.PostAsync(produto), Times.Once);
		}

		[Fact]
		public async Task PutAsync_DeveChamarMetodoDoRepositorio()
		{
			var produto = new Produto { Id = 1, Nome = "Produto Atualizado", Preco = 20.0M };

			await _produtoService.PutAsync(produto);

			_repositoryMock.Verify(r => r.PutAsync(produto), Times.Once);
		}

		[Fact]
		public async Task DeleteAsync_DeveChamarMetodoDoRepositorio()
		{
			int id = 1;

			await _produtoService.DeleteAsync(id);

			_repositoryMock.Verify(r => r.DeleteAsync(id), Times.Once);
		}
	}
}
