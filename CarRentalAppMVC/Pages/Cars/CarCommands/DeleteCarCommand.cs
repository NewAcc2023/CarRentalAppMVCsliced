using CarRentalAppMVC.Contexts;
using CarRentalAppMVC.Pages.Cars;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;

namespace CarRentalAppMVC.Commands.CarCommands
{
    public record DeleteCarCommand : IRequest<Car>
	{
		public int CarId { get; set; }
	}

	public class DeleteCarHandler : IRequestHandler<DeleteCarCommand, Car>
	{
		private readonly AppDbContext _context;
		public DeleteCarHandler(AppDbContext context)
		{
			_context = context;
		}

		public async Task<Car> Handle(DeleteCarCommand request, CancellationToken cancellationToken)
		{
			ImageManager imageManager = new();

			Car? carToDelete = await _context.Cars.FirstOrDefaultAsync(x => x.Id == request.CarId);
			imageManager.DeleteCarImage(carToDelete.ImagePath);
			var result = _context.Cars.Remove(carToDelete);
			await _context.SaveChangesAsync();
			return carToDelete;
		}
	}



	//public class DeleteCarCommand
	//{
	//	Car carToDelete = await _context.Cars.FirstOrDefaultAsync(x => x.Id == carId);
	//	DeleteCarImage(carToDelete.ImagePath);
	//	var result = _context.Cars.Remove(carToDelete);
	//	await _context.SaveChangesAsync();
	//}
}
