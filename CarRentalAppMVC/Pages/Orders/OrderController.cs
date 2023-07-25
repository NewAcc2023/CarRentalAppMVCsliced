using CarRentalAppMVC.Contexts;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CarRentalAppMVC.Queries;
using CarRentalAppMVC.Queries.CarQueries;
using CarRentalAppMVC.Commands;
using static System.Net.Mime.MediaTypeNames;
using CarRentalAppMVC.Pages.GearBoxes.GearBoxQueries;
using CarRentalAppMVC.Pages.Orders.RentOrderQueries;
using CarRentalAppMVC.Commands.RentOrderCommands;
using CarRentalAppMVC.Pages.Orders.RentOrderCommands;
using CarRentalAppMVC.Pages.Cars;
using CarRentalAppMVC.Pages.Orders.StatusQueries;
using CarRentalAppMVC.Pages.Orders.ViewModels;

namespace CarRentalAppMVC.Pages.Orders
{
    public class OrderController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;

        private readonly IMediator _mediator;
        public OrderController(IMediator mediator, UserManager<IdentityUser> userManager)
        {
            _mediator = mediator;
            _userManager = userManager;
        }
		[Authorize(Roles = "Admin")]
		public async Task<IActionResult> ManageOrders(int page = 1, int pageSize = 3)
        {
            if (page < 1)
                return RedirectToAction("SomethingWentWrong", "Car");

            RentOrdersPaginationModel rentOrdersPaginationModel = await _mediator.Send(new GetPaginatedOrdersQuery()
            {
                PageNumber = page,
                PageSize = pageSize
            });
            rentOrdersPaginationModel.Users = _userManager.Users;
            rentOrdersPaginationModel.Cars = (await _mediator.Send(new GetAllCarsQuery())).ToList();
            rentOrdersPaginationModel.Statuses = await _mediator.Send(new GetAllStatusesQuery());
            return View(rentOrdersPaginationModel);
        }


        public async Task<IActionResult> CarInfo(int carId)
        {
            //Car car = await _carRepo.GetAll().FirstOrDefaultAsync(x => x.Id == carId);
            Car car = await _mediator.Send(new GetCarByIdQuery(carId));
            if (car == null)
            {
                return RedirectToAction("SomethingWentWrong", "Home");
            }
            return View(car);
        }

        public async Task<IActionResult> Order(DateTime recieveDate, DateTime returnDate, int carId, decimal carPrice)
        {
            if (!User.Identity.IsAuthenticated)
            {
                TempData["message"] = " - Not authorized!";
                return Redirect($"CarInfo?carId={carId}");
            }
            else if (!User.IsInRole("Customer"))
            {
                TempData["message"] = " - Please add your phone number in the personal cabinet!";
                return Redirect($"CarInfo?carId={carId}");
            }

            List<RentOrder> rentOrders = (await _mediator.Send(new GetAllRentOrdersQuery())).ToList();
            List<Status> orderStatuses = (await _mediator.Send(new GetAllStatusesQuery())).ToList();
            List<Car> cars = (await _mediator.Send(new GetAllCarsQuery())).ToList();
            List<RentOrder> lockedCars = rentOrders.Where(x => x.ReturnDatetime > DateTime.Now && x.StatusId != orderStatuses.FirstOrDefault(x => x.StatusName == "Cancelled").Id && x.StatusId != orderStatuses.FirstOrDefault(x => x.StatusName == "Finished").Id).ToList();

            if (lockedCars.Find(x => x.CarId == carId) != null)
            {
                TempData["message"] = $" - The car is already in use until {lockedCars.FirstOrDefault(x => x.CarId == carId).ReturnDatetime}!"; //date

                return Redirect($"CarInfo?carId={carId}");
            }


            var user = await _userManager.GetUserAsync(User);

            if ((returnDate - recieveDate).TotalHours < 1)
            {
                TempData["message"] = " - Order should be at least for 1 hour!";
                return Redirect($"CarInfo?carId={carId}");
            }

            if (returnDate < recieveDate)
            {
                TempData["message"] = " - Return date should be bigger than recieve date!";
                return Redirect($"CarInfo?carId={carId}");
            }

            decimal total;
            if ((returnDate - recieveDate).Days < 1)
            {
                TempData["message"] = " - Order was less than for a day, so you'll pay for the 1 whole day!";
                total = 1 * carPrice;
            }
            else
            {
                total = (returnDate - recieveDate).Days * carPrice;
            }
            //add to orders
            RentOrder rentOrder = new RentOrder()
            {
                UserId = user.Id,
                CarId = carId,
                OrderCreationDatetime = DateTime.Now,
                RecieveDatetime = recieveDate,
                ReturnDatetime = returnDate,
                StatusId = orderStatuses.First(x => x.StatusName == "Reserved").Id,
                totalPrice = total,
            };

            await _mediator.Send(new AddRentOrderCommand()
            {
                RentOrder = rentOrder,
            });

            return View("OrderSuccess");
        }


        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Finish(int id, int page)
        {
            await _mediator.Send(new FinishRentOrderCommand()
            {
                Id = id
            });
            return Redirect($"/Order/ManageOrders?page={page}");
        }
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Cancel(int id, int page)
        {
            await _mediator.Send(new CancelRentOrderCommand()
            {
                Id = id
            });
            return Redirect($"/Order/ManageOrders?page={page}");
        }
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> SetActive(int id, int page)
        {
            await _mediator.Send(new SetActiveRentOrderCommand()
            {
                Id = id
            });
            return Redirect($"/Order/ManageOrders?page={page}");
        }
    }
}
