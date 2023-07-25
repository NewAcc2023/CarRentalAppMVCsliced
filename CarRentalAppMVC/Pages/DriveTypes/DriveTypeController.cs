using CarRentalAppMVC.Commands.DriveTypeCommands;
using CarRentalAppMVC.Pages.DriveTypes.DriveTypeCommands;
using CarRentalAppMVC.Pages.DriveTypes.DriveTypeQueries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace CarRentalAppMVC.Pages.DriveTypes
{

	[Authorize(Roles = "Admin")]
	public class DriveTypeController : Controller
	{

		private readonly IMediator _mediator;
		public DriveTypeController(IMediator mediator)
		{
			_mediator = mediator;
		}

		public async Task<IActionResult> EditDriveTypes()
		{
			return View((await _mediator.Send(new GetAllDriveTypesQuery())).ToList());
		}

		// CRUD for DRIVETYPE
		[HttpPost]
		public async Task<IActionResult> AddDriveType(DriveType driveType)
		{
			await _mediator.Send(new AddDriveTypeCommand()
			{
				DriveType = driveType
			});
			return RedirectToAction("EditDriveTypes");
		}

		[HttpPost]
		public async Task<IActionResult> UpdateDriveType(int id, string name)
		{
			await _mediator.Send(new UpdateDriveTypeCommand()
			{
				Id = id,
				Name = name
			});
			return RedirectToAction("EditDriveTypes");
		}

		public async Task<IActionResult> DeleteDriveType(int id)
		{
			await _mediator.Send(new DeleteDriveTypeCommand()
			{
				Id = id,
			});
			return RedirectToAction("EditDriveTypes");
		}
	}
}
