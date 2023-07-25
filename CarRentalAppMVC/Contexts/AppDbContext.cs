using CarRentalAppMVC.Pages.Brands;
using CarRentalAppMVC.Pages.Cars;
using CarRentalAppMVC.Pages.CarType;
using CarRentalAppMVC.Pages.EngineTypes;
using CarRentalAppMVC.Pages.GearBoxes;
using CarRentalAppMVC.Pages.Orders;
using CarRentalAppMVC.Pages.Orders.StatusQueries;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using DriveType = CarRentalAppMVC.Pages.DriveTypes.DriveType;

namespace CarRentalAppMVC.Contexts
{
    public class AppDbContext : IdentityDbContext
	{
        private readonly DbContextOptions _options;

		public DbSet<Car> Cars { get; set; }
		public DbSet<Brand> Brands { get; set; }

		public DbSet<CarBodyType> CarBodyTypes { get; set; }

		public DbSet<DriveType> DriveTypes { get; set; }

		public DbSet<EngineType> EngineTypes { get; set; }

		public DbSet<GearBox> GearBoxes { get; set; }

		public DbSet<RentOrder> RentOrders { get; set; }
		public DbSet<Status> Statuses { get; set; }
		public AppDbContext(DbContextOptions options) : base(options)
		{
			_options = options;
		}

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);
		}
	}
}
