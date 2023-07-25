using CarRentalAppMVC.Contexts;
using CarRentalAppMVC.Pages.GearBoxes;
using LazyCache;
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
		private readonly IAppCache _cache;
		public UpdateGearBoxHandler(AppDbContext context, IAppCache cache)
		{
			_cache = cache;
			_context = context;
		}

		public async Task<GearBox> Handle(UpdateGearBoxCommand request, CancellationToken cancellationToken)
		{
			GearBox gearBox = await _context.GearBoxes.FirstOrDefaultAsync(x => x.Id == request.Id);
			gearBox.GearBoxName = request.Name;
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
