using System;
using System.Collections.Generic;
using System.ComponentModel.Composition.Hosting;
using System.ComponentModel.Composition.Primitives;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CarRental.Client.Proxies;

namespace CarRental.Client.Bootstrapper
{
    public static class MEFLoader
    {
        public static CompositionContainer Init()
        {
            return Init(null);
        }

        public static CompositionContainer Init(IEnumerable<ComposablePartCatalog> catalogParts)
        {
            var catalog = new AggregateCatalog();
            catalog.Catalogs.Add(new AssemblyCatalog(typeof(InventoryClient).Assembly));

            if (catalogParts != null)
            {
                foreach (var part in catalogParts)
                    catalog.Catalogs.Add(part);
            }

            var container = new CompositionContainer(catalog);
            return container;
        }
    }
}
