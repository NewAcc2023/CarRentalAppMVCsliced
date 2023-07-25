using CarRentalAppMVC.Contexts;
using CarRentalAppMVC.Pages.Brands;
using LazyCache;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CarRentalAppMVC.Queries.BrandQueries
{
    public record GetAllBrandsQuery() : IRequest<IEnumerable<Brand>>;

	public class GetAllBrandsHandler : IRequestHandler<GetAllBrandsQuery, IEnumerable<Brand>>
	{
		private readonly AppDbContext _context;
		private readonly IAppCache _cache;
		public GetAllBrandsHandler(AppDbContext context, IAppCache cache)
		{
			_cache = cache;
			_context = context;


		}
		public async Task<IEnumerable<Brand>> Handle(GetAllBrandsQuery request, CancellationToken cancellationToken)
		{
			Func<Task<IEnumerable<Brand>>> objFactory = () => GetItems();

			IEnumerable<Brand> returnValue = await _cache.GetOrAddAsync("Brands", objFactory, TimeSpan.FromHours(1));

			return returnValue;
		}

		private async Task<IEnumerable<Brand>> GetItems()
		{
			return await _context.Brands.ToListAsync();
		}
	}
}
