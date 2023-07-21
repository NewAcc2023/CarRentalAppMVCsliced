using CarRentalAppMVC.Contexts;
using CarRentalAppMVC.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;
using static System.Net.Mime.MediaTypeNames;

namespace CarRentalAppMVC.Pages.Cars.CarCommands
{
    public record UpdateCarCommand : IRequest<Car>
    {
        public IFormFile? Image { get; set; }
        public Car Car { get; set; }
    }

    public class UpdateCarHandler : IRequestHandler<UpdateCarCommand, Car>
    {
        private readonly AppDbContext _context;
        public UpdateCarHandler(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Car> Handle(UpdateCarCommand request, CancellationToken cancellationToken)
        {
            ImageManager imageManager = new();

            Car? carInDatabase = await _context.Cars.FirstOrDefaultAsync(x => x.Id == request.Car.Id);

            if (request.Image == null)
            {
                carInDatabase.BrandId = request.Car.BrandId;
                carInDatabase.ModelName = request.Car.ModelName;
                carInDatabase.GearBoxId = request.Car.GearBoxId;
                carInDatabase.EngineTypeId = request.Car.EngineTypeId;
                carInDatabase.EngineCapacity = request.Car.EngineCapacity;
                carInDatabase.CarBodyTypeId = request.Car.CarBodyTypeId;
                carInDatabase.Doors = request.Car.Doors;
                carInDatabase.Seats = request.Car.Seats;
                carInDatabase.Price = request.Car.Price;
                carInDatabase.Year = request.Car.Year;
                carInDatabase.Description = request.Car.Description;
                carInDatabase.DriveTypeId = request.Car.DriveTypeId;

                _context.Cars.Update(carInDatabase);
                _context.SaveChanges();
            }
            else
            {
                carInDatabase.BrandId = request.Car.BrandId;
                carInDatabase.ModelName = request.Car.ModelName;
                carInDatabase.GearBoxId = request.Car.GearBoxId;
                carInDatabase.EngineTypeId = request.Car.EngineTypeId;
                carInDatabase.EngineCapacity = request.Car.EngineCapacity;
                carInDatabase.CarBodyTypeId = request.Car.CarBodyTypeId;
                carInDatabase.Doors = request.Car.Doors;
                carInDatabase.Seats = request.Car.Seats;
                carInDatabase.Price = request.Car.Price;
                carInDatabase.Year = request.Car.Year;
                carInDatabase.Description = request.Car.Description;
                carInDatabase.DriveTypeId = request.Car.DriveTypeId;

                string uniqueFileName = DateTime.Now.Millisecond.ToString()
                + Regex.Replace(request.Image.FileName, "[^a-zA-Z0-9.]", "-").Replace(" ", "-");

                await imageManager.DeleteCarImage(carInDatabase.ImagePath);
                await imageManager.SaveCarImage(request.Image, uniqueFileName);

                carInDatabase.ImagePath = uniqueFileName;

                _context.Cars.Update(carInDatabase);
                _context.SaveChanges();
            }
            return carInDatabase;
        }
    }
}
