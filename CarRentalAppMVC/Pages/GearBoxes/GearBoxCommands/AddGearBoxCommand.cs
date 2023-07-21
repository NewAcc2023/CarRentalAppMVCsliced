using CarRentalAppMVC.Contexts;
using CarRentalAppMVC.Entities;
using MediatR;

namespace CarRentalAppMVC.Commands.GearBoxCommands
{
	public record AddGearBoxCommand : IRequest<GearBox>
	{
		public GearBox GearBox { get; set; }
	}

	public class AddGearBoxHandler : IRequestHandler<AddGearBoxCommand, GearBox>
	{
		private readonly AppDbContext _context;
		public AddGearBoxHandler(AppDbContext context)
		{
			_context = context;
		}

		public async Task<GearBox> Handle(AddGearBoxCommand request, CancellationToken cancellationToken)
		{
			await _context.GearBoxes.AddAsync(request.GearBox);
			await _context.SaveChangesAsync();
			return request.GearBox;
		}
	}

}
