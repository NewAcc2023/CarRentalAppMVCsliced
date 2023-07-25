using CarRentalAppMVC.Contexts;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;

namespace CarRentalAppMVC.Pages.Cars.CarCommands
{
    public record UpdateCarPriceCommand : IRequest<Car>
    {
        public int Id { get; set; }
        public decimal Price { get; set; }
    }

    public class UpdateCarPriceHandler : IRequestHandler<UpdateCarPriceCommand, Car>
    {
        private readonly AppDbContext _context;
        public UpdateCarPriceHandler(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Car> Handle(UpdateCarPriceCommand request, CancellationToken cancellationToken)
        {
            Car? carInDatabase = await _context.Cars.FirstOrDefaultAsync(x => x.Id == request.Id);
            carInDatabase.Price = request.Price;
            _context.Cars.Update(carInDatabase);
            _context.SaveChanges();
            return carInDatabase;
        }
    }
}
