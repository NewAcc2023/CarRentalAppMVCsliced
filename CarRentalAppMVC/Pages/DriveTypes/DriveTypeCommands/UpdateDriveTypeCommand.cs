using CarRentalAppMVC.Contexts;
using LazyCache;
using MediatR;
using Microsoft.EntityFrameworkCore;
using DriveType = CarRentalAppMVC.Pages.DriveTypes.DriveType;

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
        private readonly IAppCache _cache;
        public UpdateDriveTypeHandler(AppDbContext context,IAppCache cache)
		{
            _cache = cache;
			_context = context;
        }

        public async Task<DriveType> Handle(UpdateDriveTypeCommand request, CancellationToken cancellationToken)
        {
            DriveType item = await _context.DriveTypes.FirstOrDefaultAsync(x => x.Id == request.Id);
            item.DriveTypeName = request.Name;
            await _context.SaveChangesAsync();

			_cache.Remove("DriveTypes");
			Func<Task<IEnumerable<DriveType>>> objFactory = () => GetItems();
			await _cache.GetOrAddAsync("DriveTypes", objFactory, TimeSpan.FromDays(1));

			return item;
        }

		private async Task<IEnumerable<DriveType>> GetItems()
		{
			return await _context.DriveTypes.ToListAsync();
		}
	}
}
