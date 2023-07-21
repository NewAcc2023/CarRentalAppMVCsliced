using CarRentalAppMVC.Contexts;
using CarRentalAppMVC.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using DriveType = CarRentalAppMVC.Entities.DriveType;

namespace CarRentalAppMVC.Pages.DriveTypes.DriveTypeCommands
{

    public record DeleteDriveTypeCommand : IRequest<DriveType>
    {
        public int Id { get; set; }
    }

    public class DeleteDriveTypeHandler : IRequestHandler<DeleteDriveTypeCommand, DriveType>
    {
        private readonly AppDbContext _context;
        public DeleteDriveTypeHandler(AppDbContext context)
        {
            _context = context;
        }

        public async Task<DriveType> Handle(DeleteDriveTypeCommand request, CancellationToken cancellationToken)
        {
            DriveType driveTypeToDelete = await _context.DriveTypes.FirstOrDefaultAsync(x => x.Id == request.Id);
            _context.DriveTypes.Remove(driveTypeToDelete);
            await _context.SaveChangesAsync();
            return driveTypeToDelete;
        }
    }
}
