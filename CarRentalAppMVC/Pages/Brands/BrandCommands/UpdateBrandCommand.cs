using CarRentalAppMVC.Contexts;
using CarRentalAppMVC.Pages.Brands;
using LazyCache;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CarRentalAppMVC.Commands.BrandCommands
{
    public record UpdateBrandCommand : IRequest<Brand>
	{
		public int Id { get; set; }

		public string Name { get; set; }	
	}

	public class UpdateBrandHandler : IRequestHandler<UpdateBrandCommand, Brand>
	{
		private readonly AppDbContext _context;
		private readonly IAppCache _cache;
		public UpdateBrandHandler(AppDbContext context, IAppCache cache)
		{
			_context = context;
			_cache = cache;

		}

		public async Task<Brand> Handle(UpdateBrandCommand request, CancellationToken cancellationToken)
		{
			Brand brand = await _context.Brands.FirstOrDefaultAsync(x => x.Id == request.Id);
			brand.BrandName = request.Name;
			await _context.SaveChangesAsync();

			_cache.Remove("Brands");
			Func<Task<IEnumerable<Brand>>> objFactory = () => GetItems();
			await _cache.GetOrAddAsync("Brands", objFactory, TimeSpan.FromHours(1));


			return brand;
		}

		private async Task<IEnumerable<Brand>> GetItems()
		{
			return await _context.Brands.ToListAsync();
		}
	}
}
