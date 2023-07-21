
using CarRentalAppMVC.Entities;
using CarRentalAppMVC.Models;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json.Serialization;
using System.Text.Json;
using MediatR;
using CarRentalAppMVC.Queries.CarQueries;
using CarRentalAppMVC.Queries.BrandQueries;
using CarRentalAppMVC.Pages.GearBoxes.GearBoxQueries;
using CarRentalAppMVC.Queries.EngineTypeQueries;

namespace CarRentalAppMVC.Controllers
{
	public class HomeController : Controller
	{
		private readonly ILogger<HomeController> _logger;

		private readonly IMediator _mediator;


		public HomeController(IMediator mediator,ILogger<HomeController> logger)
		{
			_logger = logger;
			_mediator = mediator;
		}

		public async Task<IActionResult> Index()
		{
			//List<Car> cars = _carRepo.GetAll().ToList();
			List<Car> cars = (await _mediator.Send(new GetAllCarsQuery())).ToList();
			
			IEnumerable<IndexModel> indexModels = cars.Select(car => new IndexModel {
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
			if(search != null && search.Count() > 0)
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

		public async Task<IActionResult> AllCars( List<string> selectedGearBoxes, List<string> selectedEngineTypes, List<string> selectedBrands, string? sorting = "")
		{
			ViewBag.selectedEngineTypes = selectedEngineTypes;
			ViewBag.selectedGearBoxes = selectedGearBoxes;
			ViewBag.selectedBrands =selectedBrands;
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
	}
}