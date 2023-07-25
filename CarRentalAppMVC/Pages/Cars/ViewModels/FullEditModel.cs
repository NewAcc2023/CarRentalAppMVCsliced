using CarRentalAppMVC.Pages.Brands;
using CarRentalAppMVC.Pages.CarType;
using CarRentalAppMVC.Pages.EngineTypes;
using CarRentalAppMVC.Pages.GearBoxes;
using DriveType = CarRentalAppMVC.Pages.DriveTypes.DriveType;

namespace CarRentalAppMVC.Pages.Cars.ViewModels
{
    public class FullEditModel
    {
        public Car Car { get; set; }
        public List<Brand> Brands { get; set; }
        public List<GearBox> GearBoxes { get; set; }
        public List<EngineType> EngineTypes { get; set; }
        public List<CarBodyType> CarBodyTypes { get; set; }
        public List<DriveType> DriveTypes { get; set; }


    }
}
