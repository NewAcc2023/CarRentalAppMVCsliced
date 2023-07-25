using CarRentalAppMVC.Commands.GearBoxCommands;
using CarRentalAppMVC.Pages.GearBoxes.GearBoxQueries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace CarRentalAppMVC.Pages.GearBoxes
{
	[Authorize(Roles = "Admin")]
	public class GearBoxController : Controller
	{

		private readonly IMediator _mediator;

		public GearBoxController(IMediator mediator)
		{
			_mediator = mediator;
		}

		public async Task<IActionResult> EditGearBoxes()
		{
			return View((await _mediator.Send(new GetAllGearBoxesQuery())).ToList());
		}

		// CRUD for GEARBOX
		[HttpPost]
		public async Task<IActionResult> AddGearBox(GearBox gearBox)
		{
			await _mediator.Send(new AddGearBoxCommand()
			{
				GearBox = gearBox
			});
			return RedirectToAction("EditGearBoxes");
		}

		[HttpPost]
		public async Task<IActionResult> UpdateGearBox(int id, string name)
		{
			await _mediator.Send(new UpdateGearBoxCommand()
			{
				Id = id,
				Name = name
			});
			return RedirectToAction("EditGearBoxes");
		}

		public async Task<IActionResult> DeleteGearBox(int id)
		{
			await _mediator.Send(new DeleteGearBoxCommand()
			{
				Id = id,
			});
			return RedirectToAction("EditGearBoxes");
		}
	}
}
