using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using CarRentalAppMVC.Pages.Cars;

namespace CarRentalAppMVC.Pages.GearBoxes
{
    public class GearBox
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public byte Id { get; set; }

        [Required, Column(TypeName = "varchar(31)")]
        public string GearBoxName { get; set; }

        public virtual ICollection<Car> Cars { get; set; }
    }
}
