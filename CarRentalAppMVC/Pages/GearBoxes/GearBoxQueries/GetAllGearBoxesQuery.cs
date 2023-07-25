using CarRentalAppMVC.Contexts;
using LazyCache;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CarRentalAppMVC.Pages.GearBoxes.GearBoxQueries
{
    public record GetAllGearBoxesQuery() : IRequest<IEnumerable<GearBox>>;

    public class GetAllGearBoxesHandler : IRequestHandler<GetAllGearBoxesQuery, IEnumerable<GearBox>>
    {
        private readonly AppDbContext _context;
        private readonly IAppCache _cache;
        public GetAllGearBoxesHandler(AppDbContext context, IAppCache cache)
        {
            _cache = cache;
            _context = context;
        }

        public async Task<IEnumerable<GearBox>> Handle(GetAllGearBoxesQuery request, CancellationToken cancellationToken)
        {
			Func<Task<IEnumerable<GearBox>>> objFactory = () => GetItems();

			IEnumerable<GearBox> returnValue = await _cache.GetOrAddAsync("GearBoxes", objFactory, TimeSpan.FromDays(1));
			return returnValue;
		}
		private async Task<IEnumerable<GearBox>> GetItems()
		{
			return await _context.GearBoxes.ToListAsync();
		}
	}
}
