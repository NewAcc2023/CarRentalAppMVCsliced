using CarRentalAppMVC.Contexts;
using CarRentalAppMVC.Pages.GearBoxes;
using LazyCache;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;

namespace CarRentalAppMVC.Commands.GearBoxCommands
{

    public record DeleteGearBoxCommand : IRequest<GearBox>
	{
		public int Id { get; set; }
	}

	public class DeleteGearBoxHandler : IRequestHandler<DeleteGearBoxCommand, GearBox>
	{
		private readonly AppDbContext _context;
		private readonly IAppCache _cache;
		public DeleteGearBoxHandler(AppDbContext context, IAppCache cache)
		{
			_cache = cache;
			_context = context;
		}

		public async Task<GearBox> Handle(DeleteGearBoxCommand request, CancellationToken cancellationToken)
		{
			GearBox gearBox = await _context.GearBoxes.FirstOrDefaultAsync(x => x.Id == request.Id);
			_context.GearBoxes.Remove(gearBox);
			await _context.SaveChangesAsync();

			_cache.Remove("GearBoxes");
			Func<Task<IEnumerable<GearBox>>> objFactory = () => GetItems();
			await _cache.GetOrAddAsync("GearBoxes", objFactory, TimeSpan.FromDays(1));


			return gearBox;
		}

		private async Task<IEnumerable<GearBox>> GetItems()
		{
			return await _context.GearBoxes.ToListAsync();
		}
	}
}
