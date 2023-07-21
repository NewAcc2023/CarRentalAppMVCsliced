using CarRentalAppMVC.Contexts;
using CarRentalAppMVC.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using DriveType = CarRentalAppMVC.Entities.DriveType;

namespace CarRentalAppMVC.Pages.DriveTypes.DriveTypeCommands
{
    public record UpdateDriveTypeCommand : IRequest<DriveType>
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class UpdateDriveTypeHandler : IRequestHandler<UpdateDriveTypeCommand, DriveType>
    {
        private readonly AppDbContext _context;
        public UpdateDriveTypeHandler(AppDbContext context)
        {
            _context = context;
        }

        public async Task<DriveType> Handle(UpdateDriveTypeCommand request, CancellationToken cancellationToken)
        {
            DriveType item = await _context.DriveTypes.FirstOrDefaultAsync(x => x.Id == request.Id);
            item.DriveTypeName = request.Name;
            await _context.SaveChangesAsync();
            return item;
        }
    }
}
