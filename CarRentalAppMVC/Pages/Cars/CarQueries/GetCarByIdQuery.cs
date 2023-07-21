using CarRentalAppMVC.Contexts;
using CarRentalAppMVC.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CarRentalAppMVC.Queries.CarQueries
{
	public record GetCarByIdQuery(int Id) : IRequest<Car>;

	public class GetCarByIdQueryHandler : IRequestHandler<GetCarByIdQuery, Car>
	{
		private readonly AppDbContext _context;
		public GetCarByIdQueryHandler(AppDbContext context)
		{
			_context = context;
		}
		public  Task<Car?> Handle(GetCarByIdQuery request, CancellationToken cancellationToken)
		{
			return Task.FromResult(_context.Cars
				.Include(x => x.CarBodyType)
				.Include(x => x.GearBox)
				.Include(x => x.EngineType)
				.Include(x => x.DriveType)
				.Include(x => x.Brand).FirstOrDefault(x => x.Id == request.Id));
		}
	}
}
