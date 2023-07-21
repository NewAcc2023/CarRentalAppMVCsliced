using CarRentalAppMVC.Contexts;
using CarRentalAppMVC.Entities;
using LazyCache;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CarRentalAppMVC.Commands.BrandCommands
{
	public record DeleteBrandCommand : IRequest<Brand>
	{
		public int Id { get; set; }
	}

	public class DeleteBrandHandler : IRequestHandler<DeleteBrandCommand, Brand>
	{
		private readonly AppDbContext _context;
		private readonly IAppCache _cache;
		public DeleteBrandHandler(AppDbContext context, IAppCache cache)
		{
			_context = context;
			_cache = cache;

		}

		public async Task<Brand> Handle(DeleteBrandCommand request, CancellationToken cancellationToken)
		{
			Brand Brand = await _context.Brands.FirstOrDefaultAsync(x => x.Id == request.Id);
			_context.Brands.Remove(Brand);
			await _context.SaveChangesAsync();

			_cache.Remove("Brands");
			Func<Task<IEnumerable<Brand>>> objFactory = () => GetItems();
			await _cache.GetOrAddAsync("Brands", objFactory, TimeSpan.FromHours(1));

			return Brand;
		}

		private async Task<IEnumerable<Brand>> GetItems()
		{
			return await _context.Brands.ToListAsync();
		}
	}
}
