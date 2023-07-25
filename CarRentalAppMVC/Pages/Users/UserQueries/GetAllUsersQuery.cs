using CarRentalAppMVC.Contexts;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace CarRentalAppMVC.Pages.Users.UserQueries
{
    public record GetAllUsersQuery() : IRequest<IQueryable<IdentityUser>>;

    public class GetAllUsersHandler : IRequestHandler<GetAllUsersQuery, IQueryable<IdentityUser>>
    {
        private readonly AppDbContext _context;
        public GetAllUsersHandler(AppDbContext context)
        {
            _context = context;
        }

        public Task<IQueryable<IdentityUser>> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
        {
            return Task.FromResult(_context.Users.AsQueryable());
        }
    }
}
