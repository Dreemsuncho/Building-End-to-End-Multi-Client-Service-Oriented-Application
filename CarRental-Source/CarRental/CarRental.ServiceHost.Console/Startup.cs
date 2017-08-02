using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Transactions;
using SM = System.ServiceModel;

using Core.Common.Core;
using CarRental.Common;
using CarRental.Business.Managers;
using CarRental.Business.Bootstrapper;

namespace CarRental.ServiceHost
{
    class Startup
    {
        static void Main()
        {
            Security.AddGenericPrincipal();
            ObjectBase.Container = MEFLoader.Init();

            ConsoleColor defaultColor = Console.ForegroundColor;

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Starting up services...");
            Console.ForegroundColor = defaultColor;
            Console.WriteLine();


            SM.ServiceHost hostInventoryManager = new SM.ServiceHost(typeof(InventoryManager));
            SM.ServiceHost hostRentalManager = new SM.ServiceHost(typeof(RentalManager));
            SM.ServiceHost hostAccountManager = new SM.ServiceHost(typeof(AccountManager));

            StartService(hostInventoryManager, "InventoryManager");
            StartService(hostRentalManager, "RentalManager");
            StartService(hostAccountManager, "AccountManager");

            var timer = new Timer(10000);
            timer.Elapsed += OnTimerElapsed;
            timer.Start();
            Console.WriteLine("Reservation monitor started.");

            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Press [Enter] to exit.");
            Console.ForegroundColor = defaultColor;
            Console.ReadLine();

            timer.Stop();
            Console.WriteLine("Reservation monitor stopped.");
            Console.WriteLine();

            StopService(hostInventoryManager, "InventoryManager");
            StopService(hostRentalManager, "RentalManager");
            StopService(hostAccountManager, "AccountManager");
        }

        static void OnTimerElapsed(object sender, ElapsedEventArgs e)
        {
            Console.WriteLine("Looking for dead reservations at {0}", DateTime.Now.ToString());
            var rentalManager = new RentalManager();

            var reservations = rentalManager.GetDeadReservations();
            if (reservations != null)
            {
                foreach (var reservation in reservations)
                {
                    using (var scope = new TransactionScope())
                    {
                        try
                        {
                            rentalManager.CancelReservation(reservation.ReservationId);
                            Console.WriteLine("Cancel reservation '{0}'", reservation.ReservationId);

                            scope.Complete();
                        }
                        catch (Exception)
                        {
                            Console.WriteLine(
                                "There was an excetion when attmpting to cancel reservation '{0}'.", reservation.ReservationId);
                        }
                    }
                }
            }
        }

        static void StartService(SM.ServiceHost host, string description)
        {
            host.Open();
            Console.WriteLine("Service {0} started.", description);

            foreach (var endopint in host.Description.Endpoints)
            {
                Console.WriteLine("Listing on endpoints:");
                Console.WriteLine("    Address: {0}", endopint.Address.Uri);
                Console.WriteLine("    Bindings: {0}", endopint.Binding.Name);
                Console.WriteLine("    Contracts: {0}", endopint.Contract.ConfigurationName);
            }
            Console.WriteLine();
        }

        static void StopService(SM.ServiceHost host, string description)
        {
            host.Close();
            Console.WriteLine("Service {0} stopped.", description);
        }
    }
}
