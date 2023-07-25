using CarRentalAppMVC.Commands.BrandCommands;
using CarRentalAppMVC.Queries.BrandQueries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace CarRentalAppMVC.Pages.Brands
{
	[Authorize(Roles = "Admin")]
	public class BrandController : Controller
	{
		private readonly IMediator _mediator;
		public BrandController(IMediator mediator)
		{
			_mediator = mediator;
		}

		public async Task<IActionResult> EditBrands()
		{
			return View((await _mediator.Send(new GetAllBrandsQuery())).ToList());
		}


		// CRUD for BRANDS
		[HttpPost]
		public async Task<IActionResult> AddBrand(Brand brand)
		{
			await _mediator.Send(new AddBrandCommand(brand));
			return RedirectToAction("EditBrands");
		}

		[HttpPost]
		public async Task<IActionResult> UpdateBrand(int id, string name)
		{

			await _mediator.Send(new UpdateBrandCommand()
			{
				Id = id,
				Name = name
			});
			return RedirectToAction("EditBrands");
		}

		public async Task<IActionResult> DeleteBrand(int id)
		{
			await _mediator.Send(new DeleteBrandCommand()
			{
				Id = id,
			});
			return RedirectToAction("EditBrands");
		}

	}
}
