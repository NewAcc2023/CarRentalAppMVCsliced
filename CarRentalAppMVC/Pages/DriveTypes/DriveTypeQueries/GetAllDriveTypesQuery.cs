using CarRentalAppMVC.Contexts;
using CarRentalAppMVC.Entities;
using MediatR;
using DriveType = CarRentalAppMVC.Entities.DriveType;

namespace CarRentalAppMVC.Pages.DriveTypes.DriveTypeQueries
{
    public record GetAllDriveTypesQuery() : IRequest<IQueryable<DriveType>>;

    public class GetAllDriveTypesHandler : IRequestHandler<GetAllDriveTypesQuery, IQueryable<DriveType>>
    {
        private readonly AppDbContext _context;
        public GetAllDriveTypesHandler(AppDbContext context)
        {
            _context = context;
        }

        public Task<IQueryable<DriveType>> Handle(GetAllDriveTypesQuery request, CancellationToken cancellationToken)
        {
            return Task.FromResult(_context.DriveTypes.AsQueryable());
        }
    }
}
