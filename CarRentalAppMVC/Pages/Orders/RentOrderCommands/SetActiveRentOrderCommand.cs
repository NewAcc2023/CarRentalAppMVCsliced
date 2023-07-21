using CarRentalAppMVC.Contexts;
using CarRentalAppMVC.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CarRentalAppMVC.Commands.RentOrderCommands
{
	public record SetActiveRentOrderCommand : IRequest<RentOrder>
	{
		public int Id { get; set; }
	}

	public class SetActiveRentOrderHandler : IRequestHandler<SetActiveRentOrderCommand, RentOrder>
	{
		private readonly AppDbContext _context;
		public SetActiveRentOrderHandler(AppDbContext context)
		{
			_context = context;
		}

		public async Task<RentOrder> Handle(SetActiveRentOrderCommand request, CancellationToken cancellationToken)
		{
			RentOrder rentOrder = await _context.RentOrders.FirstAsync(x => x.Id == request.Id);
			rentOrder.Status = await _context.Statuses.FirstAsync(x => x.StatusName == "Reserved");
			_context.RentOrders.Update(rentOrder);
			await _context.SaveChangesAsync();
			return rentOrder;
		}
	}
}
