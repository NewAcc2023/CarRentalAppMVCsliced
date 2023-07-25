using CarRentalAppMVC.Commands.CarBodyTypeCommands;
using CarRentalAppMVC.Queries.CarTypeQueries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace CarRentalAppMVC.Pages.CarType
{
	[Authorize(Roles = "Admin")]
	public class CarTypeController : Controller
	{
		private readonly IMediator _mediator;
		public CarTypeController(IMediator mediator)
		{
			_mediator = mediator;
		}


		public async Task<IActionResult> EditCarBodyTypes()
		{
			return View((await _mediator.Send(new GetAllCarTypesQuery())).ToList());
		}

		// CRUD for CarBodyType
		[HttpPost]
		public async Task<IActionResult> AddCarBodyType(CarBodyType carBodyType)
		{
			await _mediator.Send(new AddCarBodyTypeCommand()
			{
				CarBodyType = carBodyType
			});
			return RedirectToAction("EditCarBodyTypes");
		}

		[HttpPost]
		public async Task<IActionResult> UpdateCarBodyType(int id, string name)
		{
			await _mediator.Send(new UpdateCarBodyTypeCommand()
			{
				Id = id,
				Name = name
			});
			return RedirectToAction("EditCarBodyTypes");
		}

		public async Task<IActionResult> DeleteCarBodyType(int id)
		{
			await _mediator.Send(new DeleteCarBodyTypeCommand()
			{
				Id = id,
			});
			return RedirectToAction("EditCarBodyTypes");
		}
	}
}
