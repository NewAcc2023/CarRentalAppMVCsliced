using CarRentalAppMVC.Contexts;
using CarRentalAppMVC.Entities;
using MediatR;
using DriveType = CarRentalAppMVC.Entities.DriveType;

namespace CarRentalAppMVC.Commands.DriveTypeCommands
{

	public record AddDriveTypeCommand : IRequest<DriveType>
	{
		public DriveType DriveType { get; set; }
	}

	public class AddDriveTypeHandler : IRequestHandler<AddDriveTypeCommand, DriveType>
	{
		private readonly AppDbContext _context;
		public AddDriveTypeHandler(AppDbContext context)
		{
			_context = context;
		}

		public async Task<DriveType> Handle(AddDriveTypeCommand request, CancellationToken cancellationToken)
		{
			await _context.DriveTypes.AddAsync(request.DriveType);
			await _context.SaveChangesAsync();
			return request.DriveType;
		}
	}
}
