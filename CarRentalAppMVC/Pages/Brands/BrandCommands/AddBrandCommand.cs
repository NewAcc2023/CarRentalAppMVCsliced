
using CarRentalAppMVC.Contexts;
using CarRentalAppMVC.Pages.Brands;
using LazyCache;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CarRentalAppMVC.Commands.BrandCommands
{
    public record AddBrandCommand(Brand Brand) : IRequest<Brand>;

	public class AddBrandHandler : IRequestHandler<AddBrandCommand, Brand>
	{
		private readonly AppDbContext _context;
		private readonly IAppCache _cache;
		public AddBrandHandler(AppDbContext context, IAppCache cache)
		{
			_context = context;
			_cache = cache;
		}

		public async Task<Brand> Handle(AddBrandCommand request, CancellationToken cancellationToken)
		{
			await _context.Brands.AddAsync(request.Brand);
			await _context.SaveChangesAsync();
			_cache.Remove("Brands");
			Func<Task<IEnumerable<Brand>>> objFactory = () => GetItems();
			await _cache.GetOrAddAsync("Brands", objFactory, TimeSpan.FromHours(1));
			return request.Brand;
		}

		private async Task<IEnumerable<Brand>> GetItems()
		{
			return await _context.Brands.ToListAsync();
		}
	}
}
