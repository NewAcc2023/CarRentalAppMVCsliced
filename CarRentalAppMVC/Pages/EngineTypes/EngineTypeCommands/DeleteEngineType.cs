using CarRentalAppMVC.Contexts;
using CarRentalAppMVC.Pages.EngineTypes;
using LazyCache;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CarRentalAppMVC.Commands.EngineTypeCommands
{

    public record DeleteEngineTypeCommand : IRequest<EngineType>
	{
		public int Id { get; set; }
	}

	public class DeleteEngineTypeHandler : IRequestHandler<DeleteEngineTypeCommand, EngineType>
	{
		private readonly AppDbContext _context;
		private readonly IAppCache _cache;
		public DeleteEngineTypeHandler(AppDbContext context, IAppCache cache)
		{
			_context = context;
			_cache = cache;
		}

		public async Task<EngineType> Handle(DeleteEngineTypeCommand request, CancellationToken cancellationToken)
		{
			EngineType engineTypeToDelete = await _context.EngineTypes.FirstOrDefaultAsync(x => x.Id == request.Id);
			_context.EngineTypes.Remove(engineTypeToDelete);
			await _context.SaveChangesAsync();

			_cache.Remove("EngineTypes");
			Func<Task<IEnumerable<EngineType>>> objFactory = () => GetItems();
			await _cache.GetOrAddAsync("EngineTypes", objFactory, TimeSpan.FromDays(1));

			return engineTypeToDelete;
		}

		private async Task<IEnumerable<EngineType>> GetItems()
		{
			return await _context.EngineTypes.ToListAsync();
		}
	}
}
