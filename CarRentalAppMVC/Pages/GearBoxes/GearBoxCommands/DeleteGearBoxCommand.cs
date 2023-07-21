using CarRentalAppMVC.Contexts;
using CarRentalAppMVC.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CarRentalAppMVC.Commands.GearBoxCommands
{

	public record DeleteGearBoxCommand : IRequest<GearBox>
	{
		public int Id { get; set; }
	}

	public class DeleteGearBoxHandler : IRequestHandler<DeleteGearBoxCommand, GearBox>
	{
		private readonly AppDbContext _context;
		public DeleteGearBoxHandler(AppDbContext context)
		{
			_context = context;
		}

		public async Task<GearBox> Handle(DeleteGearBoxCommand request, CancellationToken cancellationToken)
		{
			GearBox gearBox = await _context.GearBoxes.FirstOrDefaultAsync(x => x.Id == request.Id);
			_context.GearBoxes.Remove(gearBox);
			await _context.SaveChangesAsync();
			return gearBox;
		}
	}
}
