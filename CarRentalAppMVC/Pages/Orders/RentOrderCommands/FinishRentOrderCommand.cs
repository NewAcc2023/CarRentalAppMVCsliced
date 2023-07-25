using CarRentalAppMVC.Contexts;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CarRentalAppMVC.Pages.Orders.RentOrderCommands
{

    public record FinishRentOrderCommand : IRequest<RentOrder>
    {
        public int Id { get; set; }
    }

    public class FinishRentOrderHandler : IRequestHandler<FinishRentOrderCommand, RentOrder>
    {
        private readonly AppDbContext _context;
        public FinishRentOrderHandler(AppDbContext context)
        {
            _context = context;
        }

        public async Task<RentOrder> Handle(FinishRentOrderCommand request, CancellationToken cancellationToken)
        {
            RentOrder rentOrder = await _context.RentOrders.FirstAsync(x => x.Id == request.Id);
            rentOrder.Status = await _context.Statuses.FirstAsync(x => x.StatusName == "Finished");
            _context.RentOrders.Update(rentOrder);
            await _context.SaveChangesAsync();
            return rentOrder;
        }
    }
}
