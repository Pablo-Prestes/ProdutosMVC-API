using ProdutosMvc.Models;

namespace ProdutosMvc.Repositories.Interfaces
{
	public interface IProdutosRepository
	{
		Task<IEnumerable<Produto>> GetAllAsync();
		Task<Produto> GetByIdAsync(int id);
		Task PostAsync(Produto produto);
		Task PutAsync(Produto produto);
		Task DeleteAsync(int id);
		Task<bool> ValidateExistProdutoNameAsync(string nome);
	}
}
