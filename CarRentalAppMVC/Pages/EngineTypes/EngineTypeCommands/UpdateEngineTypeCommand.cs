using CarRentalAppMVC.Contexts;
using CarRentalAppMVC.Entities;
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
		public UpdateEngineTypeHandler(AppDbContext context)
		{
			_context = context;
		}

		public async Task<EngineType> Handle(UpdateEngineTypeCommand request, CancellationToken cancellationToken)
		{
			EngineType engineType = await _context.EngineTypes.FirstOrDefaultAsync(x => x.Id == request.Id);
			engineType.EngineTypeName = request.Name;
			await _context.SaveChangesAsync();
			return engineType;
		}
	}
}
