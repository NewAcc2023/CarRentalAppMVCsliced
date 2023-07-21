using CarRentalAppMVC.Commands.BrandCommands;
using CarRentalAppMVC.Contexts;
using CarRentalAppMVC.Entities;
using MediatR;

namespace CarRentalAppMVC.Commands.RentOrderCommands
{
	public class AddRentOrderCommand : IRequest<RentOrder>
	{
		public RentOrder RentOrder { get; set; }
	}

	public class AddRentOrderHandler : IRequestHandler<AddRentOrderCommand, RentOrder>
	{
		private readonly AppDbContext _context;
		public AddRentOrderHandler(AppDbContext context)
		{
			_context = context;
		}

		public async Task<RentOrder> Handle(AddRentOrderCommand request, CancellationToken cancellationToken)
		{
			await _context.RentOrders.AddAsync(request.RentOrder);
			await _context.SaveChangesAsync();
			return request.RentOrder;
		}
	}
}
