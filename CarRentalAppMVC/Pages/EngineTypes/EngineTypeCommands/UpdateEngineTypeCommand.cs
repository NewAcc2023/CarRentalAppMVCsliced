using CarRentalAppMVC.Contexts;
using CarRentalAppMVC.Pages.EngineTypes;
using LazyCache;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CarRentalAppMVC.Commands.EngineTypeCommands
{

    public record UpdateEngineTypeCommand : IRequest<EngineType>
	{
		public int Id { get; set; }
		public string Name { get; set; }
	}

	public class UpdateEngineTypeHandler : IRequestHandler<UpdateEngineTypeCommand, EngineType>
	{
		private readonly AppDbContext _context;
		private readonly IAppCache _cache;
		public UpdateEngineTypeHandler(AppDbContext context, IAppCache cache)
		{
			_cache = cache;
			_context = context;
		}

		public async Task<EngineType> Handle(UpdateEngineTypeCommand request, CancellationToken cancellationToken)
		{
			EngineType engineType = await _context.EngineTypes.FirstOrDefaultAsync(x => x.Id == request.Id);
			engineType.EngineTypeName = request.Name;
			await _context.SaveChangesAsync();

			_cache.Remove("EngineTypes");
			Func<Task<IEnumerable<EngineType>>> objFactory = () => GetItems();
			await _cache.GetOrAddAsync("EngineTypes", objFactory, TimeSpan.FromDays(1));

			return engineType;
		}

		private async Task<IEnumerable<EngineType>> GetItems()
		{
			return await _context.EngineTypes.ToListAsync();
		}
	}
}
