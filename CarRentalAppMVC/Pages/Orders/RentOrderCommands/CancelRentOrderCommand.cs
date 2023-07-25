using CarRentalAppMVC.Contexts;
using CarRentalAppMVC.Pages.Orders;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CarRentalAppMVC.Commands.RentOrderCommands
{

    public record CancelRentOrderCommand : IRequest<RentOrder>
	{
		public int Id { get; set; }
	}

	public class CancelRentOrderHandler : IRequestHandler<CancelRentOrderCommand, RentOrder>
	{
		private readonly AppDbContext _context;
		public CancelRentOrderHandler(AppDbContext context)
		{
			_context = context;
		}

		public async Task<RentOrder> Handle(CancelRentOrderCommand request, CancellationToken cancellationToken)
		{
			RentOrder rentOrder = await _context.RentOrders.FirstAsync(x => x.Id == request.Id);
			rentOrder.Status = await _context.Statuses.FirstAsync(x => x.StatusName == "Cancelled");
			_context.RentOrders.Update(rentOrder);
			await _context.SaveChangesAsync();
			return rentOrder;
		}
	}

}
