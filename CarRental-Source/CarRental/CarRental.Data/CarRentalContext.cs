using System.Data.Entity;
using System.Runtime.Serialization;
using System.Data.Entity.ModelConfiguration.Conventions;

using Core.Common.Contracts;
using CarRental.Business.Entities;

namespace CarRental.Data
{
    public class CarRentalContext : DbContext
    {
        public CarRentalContext()
            : base("CarRental")
        {
            Database.SetInitializer<CarRentalContext>(new CreateDatabaseIfNotExists<CarRentalContext>());
        }

        public DbSet<Account> AccountSet { get; set; }

        public DbSet<Car> CarSet { get; set; }

        public DbSet<Rental> RentalSet { get; set; }

        public DbSet<Reservation> ReservationSet { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

            modelBuilder.Ignore<ExtensionDataObject>();
            modelBuilder.Ignore<IIdentifiableEntity>();

            modelBuilder.Configurations.AddFromAssembly(typeof(CarRentalContext).Assembly);
        }
    }
}
