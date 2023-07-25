using CarRentalAppMVC.Contexts;
using CarRentalAppMVC.Pages.EngineTypes;
using LazyCache;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CarRentalAppMVC.Commands.EngineTypeCommands
{
    public record AddEngineTypeCommand : IRequest<EngineType>
	{
		public EngineType EngineType { get; set; }
	}

	public class AddEngineTypeHandler : IRequestHandler<AddEngineTypeCommand, EngineType>
	{
		private readonly AppDbContext _context;
		private readonly IAppCache _cache;
		public AddEngineTypeHandler(AppDbContext context, IAppCache cache)
		{
			_context = context;
			_cache = cache;
		}

		public async Task<EngineType> Handle(AddEngineTypeCommand request, CancellationToken cancellationToken)
		{
			await _context.EngineTypes.AddAsync(request.EngineType);
			await _context.SaveChangesAsync();

			_cache.Remove("EngineTypes");
			Func<Task<IEnumerable<EngineType>>> objFactory = () => GetItems();
			await _cache.GetOrAddAsync("EngineTypes", objFactory, TimeSpan.FromDays(1));

			return request.EngineType;
		}

		private async Task<IEnumerable<EngineType>> GetItems()
		{
			return await _context.EngineTypes.ToListAsync();
		}
	}
}
