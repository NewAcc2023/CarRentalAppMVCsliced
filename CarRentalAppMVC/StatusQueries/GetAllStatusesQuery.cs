using CarRentalAppMVC.Contexts;
using CarRentalAppMVC.Entities;
using MediatR;

namespace CarRentalAppMVC.StatusQueries
{

    public record GetAllStatusesQuery() : IRequest<IQueryable<Status>>;

    public class GetAllStatusesHandler : IRequestHandler<GetAllStatusesQuery, IQueryable<Status>>
    {
        private readonly AppDbContext _context;
        public GetAllStatusesHandler(AppDbContext context)
        {
            _context = context;
        }

        public Task<IQueryable<Status>> Handle(GetAllStatusesQuery request, CancellationToken cancellationToken)
        {
            return Task.FromResult(_context.Statuses.AsQueryable());
        }
    }

}
