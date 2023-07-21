using CarRentalAppMVC.Contexts;
using CarRentalAppMVC.Entities;
using CarRentalAppMVC.Queries;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;

namespace CarRentalAppMVC.Commands.CarCommands
{
    public record AddCarCommand : IRequest<Car>
    {
        public IFormFile Image { get; set; }
        public Car Car { get; set; }
    }

    public class AddCarHandler : IRequestHandler<AddCarCommand, Car>
    {
        private readonly AppDbContext _context;
        public AddCarHandler(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Car> Handle(AddCarCommand request, CancellationToken cancellationToken)
        {
            ImageManager imageManager = new();

            string uniqueFileName = DateTime.Now.Millisecond.ToString()
            + Regex.Replace(request.Image.FileName, "[^a-zA-Z0-9.]", "-").Replace(" ", "-");
            await imageManager.SaveCarImage(request.Image, uniqueFileName);
            Car newCar = request.Car;
            newCar.ImagePath = uniqueFileName;
            await _context.Cars.AddAsync(newCar);
            await _context.SaveChangesAsync();
            return newCar;
        }
    }
}
