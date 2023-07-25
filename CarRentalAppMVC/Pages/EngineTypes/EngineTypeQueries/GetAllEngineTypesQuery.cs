using CarRentalAppMVC.Contexts;
using CarRentalAppMVC.Pages.EngineTypes;
using CarRentalAppMVC.Pages.GearBoxes;
using LazyCache;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CarRentalAppMVC.Queries.EngineTypeQueries
{
    public record GetAllEngineTypesQuery() : IRequest<IEnumerable<EngineType>>;

	public class GetAllEngineTypesHandler : IRequestHandler<GetAllEngineTypesQuery, IEnumerable<EngineType>>
	{
		private readonly AppDbContext _context;
		private readonly IAppCache _cache;
		public GetAllEngineTypesHandler(AppDbContext context, IAppCache cache)
		{
			_cache = cache;
			_context = context;
		}

		public async Task<IEnumerable<EngineType>> Handle(GetAllEngineTypesQuery request, CancellationToken cancellationToken)
		{
			Func<Task<IEnumerable<EngineType>>> objFactory = () => GetItems();

			IEnumerable<EngineType> returnValue = await _cache.GetOrAddAsync("EngineTypes", objFactory, TimeSpan.FromDays(1));
			return returnValue;
		}

		private async Task<IEnumerable<EngineType>> GetItems()
		{
			return await _context.EngineTypes.ToListAsync();
		}
	}

}
