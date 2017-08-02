using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SM = System.ServiceModel;

using CarRental.Business.Managers;

namespace CarRental.ServiceHost
{
    class Startup
    {
        static void Main()
        {
            Console.WriteLine("Starting up services...");
            Console.WriteLine();

            SM.ServiceHost hostInventoryManager = new SM.ServiceHost(typeof(InventoryManager));
            SM.ServiceHost hostRentalManager= new SM.ServiceHost(typeof(RentalManager));
            SM.ServiceHost hostAccountManager = new SM.ServiceHost(typeof(AccountManager));

            StartService(hostInventoryManager, "InventoryManager");
            StartService(hostRentalManager, "RentalManager");
            StartService(hostAccountManager, "AccountManager");

            Console.WriteLine();
            Console.WriteLine("Press [Enter] to exit.");
            Console.ReadLine();

            StopService(hostInventoryManager, "InventoryManager");
            StopService(hostRentalManager, "RentalManager");
            StopService(hostAccountManager, "AccountManager");
        }

        static void StartService(SM.ServiceHost host, string description)
        {
            host.Open();
            Console.WriteLine("Service {0} started.", description);

            foreach (var endopint in host.Description.Endpoints)
            {
                Console.WriteLine("Listing on endpoints:");
                Console.WriteLine("Address: {0}", endopint.Address.Uri);
                Console.WriteLine("Bindings: {0}", endopint.Binding.Name);
                Console.WriteLine("Contracts: {0}", endopint.Contract.ConfigurationName);
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
