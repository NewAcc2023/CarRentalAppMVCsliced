using CarRentalAppMVC.Contexts;
using CarRentalAppMVC.Pages.Cars;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CarRentalAppMVC.Queries.CarQueries
{
    public record GetAllCarsQuery() : IRequest<IQueryable<Car>>;

    public class GetAllCarsHandler : IRequestHandler<GetAllCarsQuery, IQueryable<Car>>
    {
        private readonly AppDbContext _context;
        public GetAllCarsHandler(AppDbContext context)
        {
            _context = context;
        }
        public Task<IQueryable<Car>> Handle(GetAllCarsQuery request, CancellationToken cancellationToken)
        {
            return Task.FromResult(_context.Cars
                .Include(x => x.CarBodyType)
                .Include(x => x.GearBox)
                .Include(x => x.EngineType)
                .Include(x => x.DriveType)
                .Include(x => x.Brand)
                .AsQueryable());
        }
    }
}
