using ProdutosMvc.Models;
using ProdutosMvc.Repositories.Interfaces;
using ProdutosMvc.Service.interfaces;

namespace ProdutosMvc.Service.Services
{
	public class ProdutoService : IProdutoService
	{
		private readonly IProdutosRepository _repository;

		public ProdutoService(IProdutosRepository repository)
		{
			_repository = repository;
		}

		public async Task<IEnumerable<Produto>> GetAllAsync()
		{
			return await _repository.GetAllAsync();
		}

		public async Task<Produto> GetByIdAsync(int id)
		{
			return await _repository.GetByIdAsync(id);
		}

		public async Task PostAsync(Produto produto)
		{
			await _repository.PostAsync(produto);
		}

		public async Task PutAsync(Produto produto)
		{
			await _repository.PutAsync(produto);
		}

		public async Task DeleteAsync(int id)
		{
			await _repository.DeleteAsync(id);
		}
	}
}
