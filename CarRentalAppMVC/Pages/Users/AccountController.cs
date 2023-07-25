using CarRentalAppMVC.Contexts;
using CarRentalAppMVC.Pages.Cars;
using CarRentalAppMVC.Pages.Orders;
using CarRentalAppMVC.Pages.Orders.RentOrderQueries;
using CarRentalAppMVC.Pages.Orders.StatusQueries;
using CarRentalAppMVC.Pages.Orders.ViewModels;
using CarRentalAppMVC.Pages.Users.ViewModels;
using CarRentalAppMVC.Queries.CarQueries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Text.RegularExpressions;
namespace CarRentalAppMVC.Pages.Users
{
    public class AccountController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly AppDbContext _context;
        private readonly IMediator _mediator;
        public AccountController(IMediator mediator, UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, AppDbContext context)
        {
            _mediator = mediator;
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
        }
        //get register view
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        //post register data
        [HttpPost]
        public async Task<IActionResult> Register(RegisterModel model)
        {
            //if model is valid -> create user and sign him in
            if (ModelState.IsValid)
            {
                var user = new IdentityUser
                {
                    UserName = model.Email,
                    Email = model.Email,
                };

                await _userManager.CreateAsync(user, model.Password);
                await _signInManager.SignInAsync(user, isPersistent: false);
                return RedirectToAction("index", "Car");
            }
            return View(model);
        }

        //get login view
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        //post login data
        [HttpPost]
        public async Task<IActionResult> Login(LoginModel user)
        {
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(user.Email, user.Password, user.RememberMe, false);
                if (result.Succeeded)
                {
                    //get current user and search for his claim such as customer
                    var loggedInUser = await _userManager.FindByEmailAsync(user.Email);
                    var loggedUserId = loggedInUser.Id;

                    var allRoles = _context.Roles.ToList();
                    var userRoles = _context.UserRoles.ToList();

                    foreach (var item in userRoles)
                    {
                        if (item.UserId == loggedUserId)
                        {
                            //add role claim
                            User.Claims.Append(new Claim(ClaimTypes.Role, allRoles.FirstOrDefault(x => x.Id == item.RoleId).Name));
                        }
                    }
                    return RedirectToAction("Index", "Car");
                }
                ModelState.AddModelError(string.Empty, "Invalid Login Attempt");
            }
            return View(user);
        }

        [Authorize]
        public async Task<IActionResult> PersonalCabinet()
        {
            //get cars and rentOrders to get renter cars for current user
            var userId = User.Claims.FirstOrDefault(c => c.Type.Contains("nameidentifier"))?.Value;
            List<Car> cars = (await _mediator.Send(new GetAllCarsQuery())).ToList();
            List<RentOrder> rentOrders = (await _mediator.Send(new GetAllRentOrdersQuery())).Where(x => x.UserId == userId && x.ReturnDatetime > DateTime.Now && x.Status.StatusName != "Cancelled" && x.Status.StatusName != "Finished").ToList();
            List<Status> orderStatuses = (await _mediator.Send(new GetAllStatusesQuery())).ToList();


            //list of rented cars by user
            List<RentedCarModel> rentedCars = new List<RentedCarModel>();

            foreach (var rentOrder in rentOrders)
            {
                rentedCars.Add(new RentedCarModel
                {
                    Id = rentOrder.Id,
                    Brand = cars.FirstOrDefault(x => x.Id == rentOrder.CarId).Brand.BrandName,
                    ImagePath = cars.FirstOrDefault(x => x.Id == rentOrder.CarId).ImagePath,
                    Model = cars.FirstOrDefault(x => x.Id == rentOrder.CarId).ModelName,
                    Doors = cars.FirstOrDefault(x => x.Id == rentOrder.CarId).Doors,
                    Seats = cars.FirstOrDefault(x => x.Id == rentOrder.CarId).Seats,
                    Year = cars.FirstOrDefault(x => x.Id == rentOrder.CarId).Year,
                    OrderCreationDatetime = rentOrder.OrderCreationDatetime.ToString("dd/MM/yyyy HH:mm"),
                    RecieveDatetime = rentOrder.RecieveDatetime.ToString("dd/MM/yyyy HH:mm"),
                    ReturnDatetime = rentOrder.ReturnDatetime.ToString("dd/MM/yyyy HH:mm"),
                    TotalPrice = rentOrder.totalPrice,
                });
            }
            return View(rentedCars);
        }

        //add phone number to get Customer status and be able to rent cars
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> AddPhoneNumber(string phoneNumber)
        {
            string pattern = @"^(06|07)\d{7}$"; //Orange or Moldcell phone number 
            Regex regex = new Regex(pattern);

            if (!regex.IsMatch(phoneNumber))
            {
                TempData["WrongPhoneNumber"] = "Please provide a valid phone number";
                return RedirectToAction("PersonalCabinet");
            }

            //adding number to user in database
            var user = await _userManager.GetUserAsync(User);
            user.PhoneNumber = phoneNumber;
            _context.Users.Update(user);

            // Add the new claim to the user
            await _userManager.AddClaimAsync(user, new Claim(ClaimTypes.Role, "Customer"));

            await _signInManager.RefreshSignInAsync(user);
            TempData["AddedPhoneNumber"] = "Phone number added successfully!";
            return RedirectToAction("PersonalCabinet");
        }
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();

            return RedirectToAction("Login");
        }
    }
}
