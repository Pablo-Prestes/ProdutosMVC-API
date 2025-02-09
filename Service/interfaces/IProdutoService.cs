using ProdutosMvc.Models;

namespace ProdutosMvc.Service.interfaces
{
	public interface IProdutoService
	{
		Task<IEnumerable<Produto>> GetAllAsync();
		Task<Produto> GetByIdAsync(int id);
		Task PostAsync(Produto produto);
		Task PutAsync(Produto produto);
		Task DeleteAsync(int id);
	}
}
