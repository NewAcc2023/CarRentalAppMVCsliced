using CarRentalAppMVC.Commands.BrandCommands;
using CarRentalAppMVC.Commands.CarBodyTypeCommands;
using CarRentalAppMVC.Commands.CarCommands;
using CarRentalAppMVC.Commands.DriveTypeCommands;
using CarRentalAppMVC.Commands.EngineTypeCommands;
using CarRentalAppMVC.Commands.GearBoxCommands;
using CarRentalAppMVC.Entities;
using CarRentalAppMVC.Models;
using CarRentalAppMVC.Queries.BrandQueries;
using CarRentalAppMVC.Queries.CarQueries;
using CarRentalAppMVC.Queries.CarTypeQueries;
using CarRentalAppMVC.Pages.DriveTypes.DriveTypeCommands;
using CarRentalAppMVC.Pages.DriveTypes.DriveTypeQueries;
using CarRentalAppMVC.Queries.EngineTypeQueries;
using CarRentalAppMVC.Pages.GearBoxes.GearBoxQueries;
using CarRentalAppMVC.Pages.Orders.RentOrderQueries;
using CarRentalAppMVC.StatusQueries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DriveType = CarRentalAppMVC.Entities.DriveType;
using CarRentalAppMVC.Pages.Cars.CarCommands;

namespace CarRentalAppMVC.Controllers
{
	[Authorize(Roles = "Admin")]
	public class AdminController : Controller
	{
		private readonly UserManager<IdentityUser> _userManager;
		private readonly SignInManager<IdentityUser> _signInManager;

		private readonly IMediator _mediator;

		public AdminController(IMediator mediator, UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
		{
			_mediator = mediator;
			_userManager = userManager;
			_signInManager = signInManager;
		}
		//ADMIN VIEWS 
		public async Task<IActionResult> EditRoom()
		{
			AllCarsModel allCarsModel = new AllCarsModel();
			allCarsModel.Cars = (await _mediator.Send(new GetAllCarsQuery())).ToList();
			allCarsModel.Brands = (await _mediator.Send(new GetAllBrandsQuery())).ToList();
			allCarsModel.EngineTypes = (await _mediator.Send(new GetAllEngineTypesQuery())).ToList();
			allCarsModel.GearBoxes = (await _mediator.Send(new GetAllGearBoxesQuery())).ToList();
			allCarsModel.CarBodyTypes = (await _mediator.Send(new GetAllCarTypesQuery())).ToList();
			allCarsModel.DriveTypes = (await _mediator.Send(new GetAllDriveTypesQuery())).ToList();
			return View(allCarsModel);
		}

		public async Task<IActionResult> FullEdit(int carId)
		{
			Car car = (await _mediator.Send(new GetCarByIdQuery(carId)));
			if (car == null)
			{
				return RedirectToAction("SomethingWentWrong", "Home");
			}

			FullEditModel fullEdit = new FullEditModel()
			{
				Car = car,
				Brands = (await _mediator.Send(new GetAllBrandsQuery())).ToList(),
				GearBoxes = (await _mediator.Send(new GetAllGearBoxesQuery())).ToList(),
				EngineTypes = (await _mediator.Send(new GetAllEngineTypesQuery())).ToList(),
				CarBodyTypes = (await _mediator.Send(new GetAllCarTypesQuery())).ToList(),
				DriveTypes = (await _mediator.Send(new GetAllDriveTypesQuery())).ToList(),
			};

			return View(fullEdit);
		}


		[HttpPost]
		public async Task<IActionResult> FullEdit(IFormFile image, Car car)
		{
			//await _carRepo.UpdateCar(image, car);
			await _mediator.Send(new UpdateCarCommand()
			{
				Image = image,
				Car = car
			});
			return RedirectToAction("EditRoom");
		}

		public async Task<IActionResult> EditBrands()
		{
			return View((await _mediator.Send(new GetAllBrandsQuery())).ToList());
		}

		public async Task<IActionResult> ManageOrders(int page = 1, int pageSize = 3)
		{
			RentOrdersPaginationModel rentOrdersPaginationModel = (await _mediator.Send(new GetPaginatedOrdersQuery()
			{
				PageNumber = page,
				PageSize = pageSize
			}));
			rentOrdersPaginationModel.Users = _userManager.Users;
			rentOrdersPaginationModel.Cars = (await _mediator.Send(new GetAllCarsQuery())).ToList();
			rentOrdersPaginationModel.Statuses = (await _mediator.Send(new GetAllStatusesQuery()));
			return View(rentOrdersPaginationModel);
		}

		public async Task<IActionResult> AddCar(AddCarModel model)
		{
			AddCarModel addCarModel = new AddCarModel();
			addCarModel.Brands = (await _mediator.Send(new GetAllBrandsQuery())).ToList();
			addCarModel.GearBoxes = (await _mediator.Send(new GetAllGearBoxesQuery())).ToList();
			addCarModel.EngineTypes = (await _mediator.Send(new GetAllEngineTypesQuery())).ToList();
			addCarModel.CarBodyTypes = (await _mediator.Send(new GetAllCarTypesQuery())).ToList();
			addCarModel.DriveTypes = (await _mediator.Send(new GetAllDriveTypesQuery())).ToList();

			return View(addCarModel);
		}

		public async Task<IActionResult> EditCarBodyTypes()
		{
			return View((await _mediator.Send(new GetAllCarTypesQuery())).ToList());
		}

		public async Task<IActionResult> EditDriveTypes()
		{
			return View((await _mediator.Send(new GetAllDriveTypesQuery())).ToList());
		}

		public async Task<IActionResult> EditGearBoxes()
		{
			return View((await _mediator.Send(new GetAllGearBoxesQuery())).ToList());
		}

		public async Task<IActionResult> EditEngineTypes()
		{
			return View((await _mediator.Send(new GetAllEngineTypesQuery())).ToList());
		}
		//CRUD for CAR
		[HttpPost]
		public async Task<IActionResult> UpdateCarPrice(int id, decimal price)
		{
			await _mediator.Send(new UpdateCarPriceCommand()
			{
				Id = id,
				Price = price
			});
			return RedirectToAction("EditRoom");
		}

		[HttpPost]
		public async Task<IActionResult> AddCar(IFormFile image, Car car)
		{
			//await _carRepo.AddCar(image, car);
			await _mediator.Send(new AddCarCommand { Image = image, Car = car });

			return RedirectToAction("EditRoom");
		}

		public async Task<IActionResult> DeleteCar(int carId)
		{
			//await _carRepo.DeleteCar(carId);
			await _mediator.Send(new DeleteCarCommand { CarId = carId });
			return RedirectToAction("EditRoom");
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

		// CRUD for ENGINE TYPES
		[HttpPost]
		public async Task<IActionResult> AddEngineType(EngineType engineType)
		{
			await _mediator.Send(new AddEngineTypeCommand()
			{
				EngineType  =engineType
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
