using CarRentalAppMVC.Contexts;
using CarRentalAppMVC.Pages.Orders.ViewModels;
using MediatR;

namespace CarRentalAppMVC.Pages.Orders.RentOrderQueries
{
    public class GetPaginatedOrdersQuery : IRequest<RentOrdersPaginationModel>
    {
        public int PageSize { get; set; }
        public int PageNumber { get; set; }
    }

    public class GetPaginatedOrdersHandler : IRequestHandler<GetPaginatedOrdersQuery, RentOrdersPaginationModel>
    {
        private readonly AppDbContext _context;
        public GetPaginatedOrdersHandler(AppDbContext context)
        {
            _context = context;
        }

        public Task<RentOrdersPaginationModel> Handle(GetPaginatedOrdersQuery request, CancellationToken cancellationToken)
        {
            // Fetch the total count of cars from the database
            int totalCount = _context.RentOrders.Count();

            // Calculate the total number of pages based on the page size and total count
            int totalPages = (int)Math.Ceiling((double)totalCount / request.PageSize);

            // Fetch the cars for the specified page number
            IEnumerable<RentOrder> rentOrders = _context.RentOrders
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize)
                .ToList();

            return Task.FromResult(new RentOrdersPaginationModel
            {
                RentOrders = rentOrders,
                CurrentPage = request.PageNumber,
                PageSize = request.PageSize,
                TotalPages = totalPages
            });
        }
    }
}
