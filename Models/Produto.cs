using System.ComponentModel.DataAnnotations;

namespace ProdutosMvc.Models
{
	public class Produto
	{
		public int Id { get; set; }

		[Required(ErrorMessage = "Campo nome não pode ser vazio ou ser somente espaços em branco!"), MaxLength(100)]
		public string Nome { get; set; }

		[MaxLength(200)]
		public string? Descricao { get; set; }

		[Required(ErrorMessage = "Campo preço não pode ser vazio!")]
		[Range(0.01, double.MaxValue, ErrorMessage = "Campo preço deve ser maior que 0.0 !")]
		public decimal Preco { get; set; }

		public DateTime DataCadastro { get; set; } = DateTime.UtcNow;
	}
}
