using CarRentalAppMVC.Contexts;
using CarRentalAppMVC.Entities;
using LazyCache;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CarRentalAppMVC.Commands.CarBodyTypeCommands
{
	public record DeleteCarBodyTypeCommand : IRequest<CarBodyType>
	{
		public int Id { get; set; }
	}

	public class DeleteCarBodyTypeHandler : IRequestHandler<DeleteCarBodyTypeCommand, CarBodyType>
	{
		private readonly AppDbContext _context;
		private readonly IAppCache _cache;
		public DeleteCarBodyTypeHandler(AppDbContext context, IAppCache cache)
		{
			_cache = cache;
			_context = context;
		}

		public async Task<CarBodyType> Handle(DeleteCarBodyTypeCommand request, CancellationToken cancellationToken)
		{
			CarBodyType carBodyType = await _context.CarBodyTypes.FirstOrDefaultAsync(x => x.Id == request.Id);
			_context.CarBodyTypes.Remove(carBodyType);
			await _context.SaveChangesAsync();
			_cache.Remove("CarBodyTypes");
			Func<Task<IEnumerable<CarBodyType>>> objFactory = () => GetItems();
			await _cache.GetOrAddAsync("CarBodyTypes", objFactory, TimeSpan.FromHours(1));
			return carBodyType;
		}

		private async Task<IEnumerable<CarBodyType>> GetItems()
		{
			return await _context.CarBodyTypes.ToListAsync();
		}
	}
}
