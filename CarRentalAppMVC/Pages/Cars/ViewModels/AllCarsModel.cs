using CarRentalAppMVC.Pages.Brands;
using CarRentalAppMVC.Pages.CarType;
using CarRentalAppMVC.Pages.EngineTypes;
using CarRentalAppMVC.Pages.GearBoxes;
using System.ComponentModel.DataAnnotations;
using DriveType = CarRentalAppMVC.Pages.DriveTypes.DriveType;

namespace CarRentalAppMVC.Pages.Cars.ViewModels
{
    public class AllCarsModel
    {
        public List<Car> Cars { get; set; }
        public IEnumerable<Brand> Brands { get; set; }
        public List<GearBox> GearBoxes { get; set; }
        public List<EngineType> EngineTypes { get; set; }
        public List<CarBodyType> CarBodyTypes { get; set; }
        public List<DriveType> DriveTypes { get; set; }


    }
}
