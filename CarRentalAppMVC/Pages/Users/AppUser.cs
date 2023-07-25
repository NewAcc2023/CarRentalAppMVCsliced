using CarRentalAppMVC.Pages.Orders;
using Microsoft.AspNetCore.Identity;

namespace CarRentalAppMVC.Pages.Users
{
    public class AppUser : IdentityUser
    {
        public virtual ICollection<RentOrder> RentOrders { get; set; }
    }
}
