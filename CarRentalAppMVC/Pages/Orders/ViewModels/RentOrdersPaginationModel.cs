using CarRentalAppMVC.Pages.Cars;
using CarRentalAppMVC.Pages.Orders;
using CarRentalAppMVC.Pages.Orders.StatusQueries;
using Microsoft.AspNetCore.Identity;

namespace CarRentalAppMVC.Pages.Orders.ViewModels
{
    public class RentOrdersPaginationModel
    {
        public IEnumerable<RentOrder> RentOrders { get; set; }
        public IEnumerable<IdentityUser> Users { get; set; }
        public IEnumerable<Car> Cars { get; set; }
        public IEnumerable<Status> Statuses { get; set; }
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
        public int TotalPages { get; set; }
    }
}
