using CarRentalAppMVC.Pages.CarType;
using CarRentalAppMVC.Pages.EngineTypes;
using CarRentalAppMVC.Pages.GearBoxes;
using Brand = CarRentalAppMVC.Pages.Brands.Brand;
using DriveType = CarRentalAppMVC.Pages.DriveTypes.DriveType;

namespace CarRentalAppMVC.Pages.Cars.ViewModels
{
    public class AddCarModel
    {
        public List<Brand> Brands { get; set; }

        public string ModelName { get; set; }

        public List<GearBox> GearBoxes { get; set; }

        public List<EngineType> EngineTypes { get; set; }

        public string EngineCapacity { get; set; }

        public List<CarBodyType> CarBodyTypes { get; set; }

        public int Doors { get; set; }

        public int Seats { get; set; }

        public int Year { get; set; }

        public decimal Price { get; set; }

        public List<DriveType> DriveTypes { get; set; }

        public string ImagePath { get; set; }
    }
}
