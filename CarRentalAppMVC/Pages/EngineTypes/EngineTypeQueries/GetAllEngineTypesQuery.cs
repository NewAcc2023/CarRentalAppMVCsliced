using CarRentalAppMVC.Contexts;
using CarRentalAppMVC.Entities;
using MediatR;

namespace CarRentalAppMVC.Queries.EngineTypeQueries
{
	public record GetAllEngineTypesQuery() : IRequest<IQueryable<EngineType>>;

	public class GetAllEngineTypesHandler : IRequestHandler<GetAllEngineTypesQuery, IQueryable<EngineType>>
	{
		private readonly AppDbContext _context;
		public GetAllEngineTypesHandler(AppDbContext context)
		{
			_context = context;
		}

		public Task<IQueryable<EngineType>> Handle(GetAllEngineTypesQuery request, CancellationToken cancellationToken)
		{
			return Task.FromResult(_context.EngineTypes.AsQueryable());
		}
	}

}
