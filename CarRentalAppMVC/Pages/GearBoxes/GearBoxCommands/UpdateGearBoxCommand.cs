using CarRentalAppMVC.Contexts;
using CarRentalAppMVC.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CarRentalAppMVC.Commands.GearBoxCommands
{

	public record UpdateGearBoxCommand : IRequest<GearBox>
	{
		public int Id { get; set; }
		public string Name { get; set; }
	}

	public class UpdateGearBoxHandler : IRequestHandler<UpdateGearBoxCommand, GearBox>
	{
		private readonly AppDbContext _context;
		public UpdateGearBoxHandler(AppDbContext context)
		{
			_context = context;
		}

		public async Task<GearBox> Handle(UpdateGearBoxCommand request, CancellationToken cancellationToken)
		{
			GearBox gearBox = await _context.GearBoxes.FirstOrDefaultAsync(x => x.Id == request.Id);
			gearBox.GearBoxName = request.Name;
			await _context.SaveChangesAsync();
			return gearBox;
		}
	}
}
