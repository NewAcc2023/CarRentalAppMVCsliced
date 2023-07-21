using CarRentalAppMVC.Contexts;
using CarRentalAppMVC.Entities;
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
		public DeleteEngineTypeHandler(AppDbContext context)
		{
			_context = context;
		}

		public async Task<EngineType> Handle(DeleteEngineTypeCommand request, CancellationToken cancellationToken)
		{
			EngineType engineTypeToDelete = await _context.EngineTypes.FirstOrDefaultAsync(x => x.Id == request.Id);
			_context.EngineTypes.Remove(engineTypeToDelete);
			await _context.SaveChangesAsync();
			return engineTypeToDelete;
		}
	}
}
