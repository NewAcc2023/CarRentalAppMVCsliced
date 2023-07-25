using CarRentalAppMVC.Contexts;
using LazyCache;
using MediatR;
using Microsoft.EntityFrameworkCore;
using DriveType = CarRentalAppMVC.Pages.DriveTypes.DriveType;

namespace CarRentalAppMVC.Pages.DriveTypes.DriveTypeCommands
{

    public record DeleteDriveTypeCommand : IRequest<DriveType>
    {
        public int Id { get; set; }
    }

    public class DeleteDriveTypeHandler : IRequestHandler<DeleteDriveTypeCommand, DriveType>
    {
        private readonly AppDbContext _context;
		private readonly IAppCache _cache;
		public DeleteDriveTypeHandler(AppDbContext context, IAppCache cache)
        {
            _cache = cache;
            _context = context;
        }

        public async Task<DriveType> Handle(DeleteDriveTypeCommand request, CancellationToken cancellationToken)
        {
            DriveType driveTypeToDelete = await _context.DriveTypes.FirstOrDefaultAsync(x => x.Id == request.Id);
            _context.DriveTypes.Remove(driveTypeToDelete);
            await _context.SaveChangesAsync();

			_cache.Remove("DriveTypes");
			Func<Task<IEnumerable<DriveType>>> objFactory = () => GetItems();
			await _cache.GetOrAddAsync("DriveTypes", objFactory, TimeSpan.FromDays(1));

			return driveTypeToDelete;
        }

		private async Task<IEnumerable<DriveType>> GetItems()
		{
			return await _context.DriveTypes.ToListAsync();
		}
	}
}
