using CarRentalAppMVC.Contexts;
using CarRentalAppMVC.Pages.CarType;
using LazyCache;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CarRentalAppMVC.Commands.CarBodyTypeCommands
{
    public record AddCarBodyTypeCommand : IRequest<CarBodyType>
	{
		public CarBodyType CarBodyType { get; set; }
	}

	public class AddCarBodyTypeHandler : IRequestHandler<AddCarBodyTypeCommand, CarBodyType>
	{
		private readonly AppDbContext _context;
		private readonly IAppCache _cache;
		public AddCarBodyTypeHandler(AppDbContext context, IAppCache cache)
		{
			_cache = cache;
			_context = context;
		}

		public async Task<CarBodyType> Handle(AddCarBodyTypeCommand request, CancellationToken cancellationToken)
		{
			await _context.CarBodyTypes.AddAsync(request.CarBodyType);
			await _context.SaveChangesAsync();
			_cache.Remove("CarBodyTypes");
			Func<Task<IEnumerable<CarBodyType>>> objFactory = () => GetItems();
			await _cache.GetOrAddAsync("CarBodyTypes", objFactory, TimeSpan.FromHours(1));
			return request.CarBodyType;
		}

		private async Task<IEnumerable<CarBodyType>> GetItems()
		{
			return await _context.CarBodyTypes.ToListAsync();
		}
	}
}
