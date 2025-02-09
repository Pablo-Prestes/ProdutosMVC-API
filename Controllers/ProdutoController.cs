using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using ProdutosMvc.Models;
using ProdutosMvc.Service.interfaces;

namespace ProdutosMvc.Controllers
{
	public class ProdutoController : Controller
	{
		private readonly IProdutoService _service;
		private readonly IValidator<Produto> _validator;

		public ProdutoController(IProdutoService service, IValidator<Produto> validator)
		{
			_service = service;
			_validator = validator;
		}

		public async Task<IActionResult> Index()
		{
			var produtos = await _service.GetAllAsync();
			return View(produtos);
		}

		public IActionResult Create() => View();

		[HttpPost]
		public async Task<IActionResult> Create(Produto produto)
		{
			var validationResult = await _validator.ValidateAsync(produto);
			if (!validationResult.IsValid)
			{
				foreach (var error in validationResult.Errors)
					ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
				return View(produto);
			}

			await _service.PostAsync(produto);
			return RedirectToAction("Index");
		}

		public async Task<IActionResult> Edit(int id)
		{
			var produto = await _service.GetByIdAsync(id);
			if (produto == null)
				return NotFound();

			return View(produto);
		}

		[HttpPost]
		public async Task<IActionResult> Edit(Produto produto)
		{
			var validationResult = await _validator.ValidateAsync(produto);
			if (!validationResult.IsValid)
			{
				foreach (var error in validationResult.Errors)
					ModelState.AddModelError("", error.ErrorMessage);
				return View(produto);
			}

			await _service.PutAsync(produto);
			return RedirectToAction("Index");
		}

		public async Task<IActionResult> Delete(int id)
		{
			var produto = await _service.GetByIdAsync(id);
			if (produto == null)
				return NotFound();

			return View(produto);
		}

		[HttpPost, ActionName("Delete")]
		public async Task<IActionResult> DeleteConfirmed(int id)
		{
			await _service.DeleteAsync(id);
			return RedirectToAction("Index");
		}

		public async Task<IActionResult> Details(int id)
		{
			var produto = await _service.GetByIdAsync(id);
			if (produto == null)
				return NotFound();

			return View(produto);
		}
	}
}
