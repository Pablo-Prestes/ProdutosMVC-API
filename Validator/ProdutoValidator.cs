using ProdutosMvc.Models;
using FluentValidation;
using ProdutosMvc.Repositories.Interfaces;

namespace ProdutosMvc.Validator
{
	public class ProdutoValidator : AbstractValidator<Produto>
	{
		public ProdutoValidator(IProdutosRepository repository)
		{
			RuleFor(p => p.Nome)
				.NotEmpty().WithMessage("O nome é obrigatório !")
				.MaximumLength(100).WithMessage("O nome deve ter no máximo 100 caracteres !")
				.MustAsync(async (produto, nome, cancellation) =>
					!await repository.ValidateExistProdutoNameAsync(nome))
				.WithMessage("Já existe um produto com esse nome !");

			RuleFor(p => p.Preco)
				.NotNull().WithMessage("O preço é obrigatório !")
				.GreaterThan(0).WithMessage("O preço deve ser maior que zero !");
		}
	}
}
