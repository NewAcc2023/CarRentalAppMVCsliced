using CarRentalAppMVC.Contexts;
using CarRentalAppMVC.Entities;
using MediatR;

namespace CarRentalAppMVC.Pages.GearBoxes.GearBoxQueries
{
    public record GetAllGearBoxesQuery() : IRequest<IQueryable<GearBox>>;

    public class GetAllGearBoxesHandler : IRequestHandler<GetAllGearBoxesQuery, IQueryable<GearBox>>
    {
        private readonly AppDbContext _context;
        public GetAllGearBoxesHandler(AppDbContext context)
        {
            _context = context;
        }

        public Task<IQueryable<GearBox>> Handle(GetAllGearBoxesQuery request, CancellationToken cancellationToken)
        {
            return Task.FromResult(_context.GearBoxes.AsQueryable());
        }
    }
}
