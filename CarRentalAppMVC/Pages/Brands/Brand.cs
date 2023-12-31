﻿using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using CarRentalAppMVC.Pages.Cars;

namespace CarRentalAppMVC.Pages.Brands
{
    public class Brand
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public short Id { get; set; }

        [Required, Column(TypeName = "varchar(32)")]
        public string BrandName { get; set; }
        public virtual ICollection<Car> Cars { get; set; }
    }
}
