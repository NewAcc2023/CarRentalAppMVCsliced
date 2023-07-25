using CarRentalAppMVC.Contexts;
using CarRentalAppMVC.Pages.CarType;
using LazyCache;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CarRentalAppMVC.Queries.CarTypeQueries
{
    public class GetAllCarTypesQuery : IRequest<IEnumerable<CarBodyType>>
	{

	}

	public class GetAllCarTypesHandler : IRequestHandler<GetAllCarTypesQuery, IEnumerable<CarBodyType>>
	{
		private readonly AppDbContext _context;
		private readonly IAppCache _cache;
		public GetAllCarTypesHandler(AppDbContext context, IAppCache cache)
		{
			_cache = cache;
			_context = context;
        }

        public async Task<IEnumerable<CarBodyType>> Handle(GetAllCarTypesQuery request, CancellationToken cancellationToken)
		{
			Func<Task<IEnumerable<CarBodyType>>> objFactory = () => GetItems();

			IEnumerable<CarBodyType> returnValue = await _cache.GetOrAddAsync("CarBodyTypes", objFactory, TimeSpan.FromHours(1));

			return returnValue;
		}

		private async Task<IEnumerable<CarBodyType>> GetItems()
		{
			return await _context.CarBodyTypes.ToListAsync();
		}
	}
}
