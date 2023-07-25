using CarRentalAppMVC.Contexts;
using MediatR;

namespace CarRentalAppMVC.Pages.Orders.RentOrderQueries
{
    public record GetAllRentOrdersQuery() : IRequest<IQueryable<RentOrder>>;

    public class GetAllRentOrdersHandler : IRequestHandler<GetAllRentOrdersQuery, IQueryable<RentOrder>>
    {
        private readonly AppDbContext _context;
        public GetAllRentOrdersHandler(AppDbContext context)
        {
            _context = context;
        }

        public Task<IQueryable<RentOrder>> Handle(GetAllRentOrdersQuery request, CancellationToken cancellationToken)
        {
            return Task.FromResult(_context.RentOrders.AsQueryable());
        }
    }
}
