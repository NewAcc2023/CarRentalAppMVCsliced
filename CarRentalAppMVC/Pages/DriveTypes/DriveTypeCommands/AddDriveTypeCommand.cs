using CarRentalAppMVC.Contexts;
using LazyCache;
using MediatR;
using Microsoft.EntityFrameworkCore;
using DriveType = CarRentalAppMVC.Pages.DriveTypes.DriveType;

namespace CarRentalAppMVC.Commands.DriveTypeCommands
{

    public record AddDriveTypeCommand : IRequest<DriveType>
	{
		public DriveType DriveType { get; set; }
	}

	public class AddDriveTypeHandler : IRequestHandler<AddDriveTypeCommand, DriveType>
	{
		private readonly AppDbContext _context;
		private readonly IAppCache _cache;
		public AddDriveTypeHandler(AppDbContext context, IAppCache cache)
		{
			_cache = cache;
			_context = context;
		}

		public async Task<DriveType> Handle(AddDriveTypeCommand request, CancellationToken cancellationToken)
		{
			await _context.DriveTypes.AddAsync(request.DriveType);
			await _context.SaveChangesAsync();

			_cache.Remove("DriveTypes");
			Func<Task<IEnumerable<DriveType>>> objFactory = () => GetItems();
			await _cache.GetOrAddAsync("DriveTypes", objFactory, TimeSpan.FromDays(1));

			return request.DriveType;
		}

		private async Task<IEnumerable<DriveType>> GetItems()
		{
			return await _context.DriveTypes.ToListAsync();
		}
	}
}
