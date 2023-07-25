using CarRentalAppMVC.Commands.EngineTypeCommands;
using CarRentalAppMVC.Queries.EngineTypeQueries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace CarRentalAppMVC.Pages.EngineTypes
{

	[Authorize(Roles = "Admin")]
	public class EngineTypeController : Controller
	{
		private readonly IMediator _mediator;
		public EngineTypeController(IMediator mediator)
		{
			_mediator = mediator;
		}

		public async Task<IActionResult> EditEngineTypes()
		{
			return View((await _mediator.Send(new GetAllEngineTypesQuery())).ToList());
		}

		// CRUD for ENGINE TYPES
		[HttpPost]
		public async Task<IActionResult> AddEngineType(EngineType engineType)
		{
			await _mediator.Send(new AddEngineTypeCommand()
			{
				EngineType = engineType
			});
			return RedirectToAction("EditEngineTypes");
		}

		[HttpPost]
		public async Task<IActionResult> UpdateEngineType(int id, string name)
		{
			await _mediator.Send(new UpdateEngineTypeCommand()
			{
				Id = id,
				Name = name
			});
			return RedirectToAction("EditEngineTypes");
		}

		public async Task<IActionResult> DeleteEngineType(int id)
		{
			await _mediator.Send(new DeleteEngineTypeCommand()
			{
				Id = id
			});
			return RedirectToAction("EditEngineTypes");
		}
	}
}
