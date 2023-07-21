using CarRentalAppMVC.Contexts;
using CarRentalAppMVC.Entities;
using MediatR;

namespace CarRentalAppMVC.Commands.EngineTypeCommands
{
	public record AddEngineTypeCommand : IRequest<EngineType>
	{
		public EngineType EngineType { get; set; }
	}

	public class AddEngineTypeHandler : IRequestHandler<AddEngineTypeCommand, EngineType>
	{
		private readonly AppDbContext _context;
		public AddEngineTypeHandler(AppDbContext context)
		{
			_context = context;
		}

		public async Task<EngineType> Handle(AddEngineTypeCommand request, CancellationToken cancellationToken)
		{
			await _context.EngineTypes.AddAsync(request.EngineType);
			await _context.SaveChangesAsync();
			return request.EngineType;
		}
	}
}
