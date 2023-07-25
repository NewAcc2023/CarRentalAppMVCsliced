using CarRentalAppMVC.Contexts;
using CarRentalAppMVC.Pages.CarType;
using LazyCache;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CarRentalAppMVC.Commands.CarBodyTypeCommands
{


    public record UpdateCarBodyTypeCommand : IRequest<CarBodyType>
	{
		public int Id { get; set; }
		public string Name { get; set; }
	}

	public class UpdateCarBodyTypeHandler : IRequestHandler<UpdateCarBodyTypeCommand, CarBodyType>
	{
		private readonly AppDbContext _context;
		private readonly IAppCache _cache;
		public UpdateCarBodyTypeHandler(AppDbContext context, IAppCache cache)
		{
			_cache = cache;
			_context = context;
		}

		public async Task<CarBodyType> Handle(UpdateCarBodyTypeCommand request, CancellationToken cancellationToken)
		{
			CarBodyType carBodyType = await _context.CarBodyTypes.FirstOrDefaultAsync(x => x.Id == request.Id);
			carBodyType.CarBodyTypeName = request.Name;
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
