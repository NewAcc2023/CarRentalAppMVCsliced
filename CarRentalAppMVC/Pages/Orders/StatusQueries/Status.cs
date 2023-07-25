using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using CarRentalAppMVC.Pages.Orders;

namespace CarRentalAppMVC.Pages.Orders.StatusQueries
{
    public class Status
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public byte Id { get; set; }

        [Required, Column(TypeName = "varchar(31)")]
        public string StatusName { get; set; }

        public virtual ICollection<RentOrder> RentOrders { get; set; }
    }
}
