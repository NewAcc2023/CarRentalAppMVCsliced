using CarRentalAppMVC.Commands.CarCommands;
using CarRentalAppMVC.Pages.Cars.CarCommands;
using CarRentalAppMVC.Pages.Cars.ViewModels;
using CarRentalAppMVC.Pages.DriveTypes.DriveTypeQueries;
using CarRentalAppMVC.Pages.GearBoxes.GearBoxQueries;
using CarRentalAppMVC.Pages.Shared;
using CarRentalAppMVC.Queries.BrandQueries;
using CarRentalAppMVC.Queries.CarQueries;
using CarRentalAppMVC.Queries.CarTypeQueries;
using CarRentalAppMVC.Queries.EngineTypeQueries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace CarRentalAppMVC.Pages.Cars
{
	public class CarController : Controller
	{
		private readonly IMediator _mediator;
		public CarController(IMediator mediator)
		{
			_mediator = mediator;
		}

		public async Task<IActionResult> Index()
		{
			//List<Car> cars = _carRepo.GetAll().ToList();
			List<Car> cars = (await _mediator.Send(new GetAllCarsQuery())).ToList();

			IEnumerable<IndexModel> indexModels = cars.Select(car => new IndexModel
			{
				Id = car.Id,
				Brand = car.Brand.BrandName,
				ImagePath = car.ImagePath,
				Model = car.ModelName,
				StartPrice = car.Price,
			});

			return View(indexModels);
		}

		public async Task<IActionResult> SearchCars(string search)
		{
			if (search != null && search.Count() > 0)
			{
				JsonSerializerOptions options = new JsonSerializerOptions
				{
					ReferenceHandler = ReferenceHandler.Preserve,
					MaxDepth = 32 // Set the maximum allowed depth as needed
				};

				List<Car> cars = (await _mediator.Send(new GetAllCarsQuery())).Where(x => x.Brand.BrandName.ToLower().Contains(search.ToLower()) ||
													x.ModelName.ToLower().Contains(search.ToLower())).ToList();

				List<SearchResult> searchResults = new List<SearchResult>();

				foreach (var car in cars)
				{
					SearchResult searchResult = new SearchResult();
					searchResult.Id = car.Id;
					searchResult.BrandName = car.Brand.BrandName;
					searchResult.ModelName = car.ModelName;
					searchResult.Price = car.Price;
					searchResults.Add(searchResult);
				}
				return Json(searchResults, options);
			}
			return RedirectToAction("Index");
		}

		public async Task<IActionResult> AllCars(List<string> selectedGearBoxes, List<string> selectedEngineTypes, List<string> selectedBrands, string? sorting = "")
		{
			ViewBag.selectedEngineTypes = selectedEngineTypes;
			ViewBag.selectedGearBoxes = selectedGearBoxes;
			ViewBag.selectedBrands = selectedBrands;
			ViewBag.sorting = sorting;
			TempData["Sorted"] = sorting;
			List<Car> cars = (await _mediator.Send(new GetAllCarsQuery())).ToList();
			//filter
			if (selectedGearBoxes.Count > 0)
				cars = cars.Where(x => selectedGearBoxes.Contains(x.GearBox.GearBoxName)).ToList();
			if (selectedEngineTypes.Count > 0)
				cars = cars.Where(x => selectedEngineTypes.Contains(x.EngineType.EngineTypeName)).ToList();
			if (selectedBrands.Count > 0)
				cars = cars.Where(x => selectedBrands.Contains(x.Brand.BrandName)).ToList();

			AllCarsModel allCarsModel = new AllCarsModel()
			{
				Cars = cars,
				//BRANDS
				//Brands = _brandRepo.GetAll().ToList(),
				Brands = await _mediator.Send(new GetAllBrandsQuery()),
				GearBoxes = (await _mediator.Send(new GetAllGearBoxesQuery())).ToList(),
				EngineTypes = (await _mediator.Send(new GetAllEngineTypesQuery())).ToList(),
			};

			//sort
			if (sorting == "cheapest")
			{
				AllCarsModel allCarsModelSorted = allCarsModel;
				allCarsModelSorted.Cars = allCarsModel.Cars.OrderBy(x => x.Price).ToList();
				return View(allCarsModelSorted);
			}
			else if (sorting == "expensive")
			{
				AllCarsModel allCarsModelSorted = allCarsModel;
				allCarsModelSorted.Cars = allCarsModel.Cars.OrderByDescending(x => x.Price).ToList();
				return View(allCarsModelSorted);
			}
			else if (sorting == "newest")
			{
				AllCarsModel allCarsModelSorted = allCarsModel;
				allCarsModelSorted.Cars = allCarsModel.Cars.OrderByDescending(x => x.Year).ToList();
				return View(allCarsModelSorted);
			}
			//if anything else
			return View(allCarsModel);
		}

		public async Task<IActionResult> SomethingWentWrong()
		{
			return View();
		}


		//ADMIN VIEWS 
		[Authorize(Roles = "Admin")]
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


		[Authorize(Roles = "Admin")]
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
		[Authorize(Roles = "Admin")]
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

		[Authorize(Roles = "Admin")]
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

		//CRUD for CAR
		[HttpPost]
		[Authorize(Roles = "Admin")]
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
		[Authorize(Roles = "Admin")]
		public async Task<IActionResult> AddCar(IFormFile image, Car car)
		{
			//await _carRepo.AddCar(image, car);
			await _mediator.Send(new AddCarCommand { Image = image, Car = car });

			return RedirectToAction("EditRoom");
		}

		[Authorize(Roles = "Admin")]
		public async Task<IActionResult> DeleteCar(int carId)
		{
			//await _carRepo.DeleteCar(carId);
			await _mediator.Send(new DeleteCarCommand { CarId = carId });
			return RedirectToAction("EditRoom");
		}
	}
}
