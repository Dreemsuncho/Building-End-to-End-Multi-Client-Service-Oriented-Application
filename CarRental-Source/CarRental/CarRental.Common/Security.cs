using System.Security.Principal;
using System.Threading;

namespace CarRental.Common
{
    public static class Security
    {
        public const string Car_Rental_User = "CarRentalUser";
        public const string Car_Rental_Admin_Role = "CarRentalAdmin";

        public static void AddGenericPrincipal()
        {
            var identity = new GenericIdentity("Elvis");
            var principal = new GenericPrincipal(identity, new[] { Car_Rental_Admin_Role });
            Thread.CurrentPrincipal = principal;
        }
    }
}
