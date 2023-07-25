using CarRentalAppMVC.Contexts;
using CarRentalAppMVC.Pages.GearBoxes;
using LazyCache;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CarRentalAppMVC.Commands.GearBoxCommands
{
    public record AddGearBoxCommand : IRequest<GearBox>
	{
		public GearBox GearBox { get; set; }
	}

	public class AddGearBoxHandler : IRequestHandler<AddGearBoxCommand, GearBox>
	{
		private readonly AppDbContext _context;
		private readonly IAppCache _cache;
		public AddGearBoxHandler(AppDbContext context, IAppCache cache)
		{
			_cache = cache;
			_context = context;
		}

		public async Task<GearBox> Handle(AddGearBoxCommand request, CancellationToken cancellationToken)
		{
			await _context.GearBoxes.AddAsync(request.GearBox);
			await _context.SaveChangesAsync();

			_cache.Remove("GearBoxes");
			Func<Task<IEnumerable<GearBox>>> objFactory = () => GetItems();
			await _cache.GetOrAddAsync("GearBoxes", objFactory, TimeSpan.FromDays(1));
			return request.GearBox;
		}

		private async Task<IEnumerable<GearBox>> GetItems()
		{
			return await _context.GearBoxes.ToListAsync();
		}
	}

}
