using CarRentalAppMVC.Contexts;
using CarRentalAppMVC.Pages.Brands;
using LazyCache;
using MediatR;
using Microsoft.EntityFrameworkCore;
using DriveType = CarRentalAppMVC.Pages.DriveTypes.DriveType;

namespace CarRentalAppMVC.Pages.DriveTypes.DriveTypeQueries
{
    public record GetAllDriveTypesQuery() : IRequest<IEnumerable<DriveType>>;

    public class GetAllDriveTypesHandler : IRequestHandler<GetAllDriveTypesQuery, IEnumerable<DriveType>>
    {
        private readonly AppDbContext _context;
        private readonly IAppCache _cache;
        public GetAllDriveTypesHandler(AppDbContext context, IAppCache cache)
        {
            _context = context;
            _cache = cache;
        }

        public async Task<IEnumerable<DriveType>> Handle(GetAllDriveTypesQuery request, CancellationToken cancellationToken)
        {
			Func<Task<IEnumerable<DriveType>>> objFactory = () => GetItems();

			IEnumerable<DriveType> returnValue = await _cache.GetOrAddAsync("DriveTypes", objFactory, TimeSpan.FromDays(1));

			return returnValue;
        }

		private async Task<IEnumerable<DriveType>> GetItems()
		{
			return await _context.DriveTypes.ToListAsync();
		}
	}
}
