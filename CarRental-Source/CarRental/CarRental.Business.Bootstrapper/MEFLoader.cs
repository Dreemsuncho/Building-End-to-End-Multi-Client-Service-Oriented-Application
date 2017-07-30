using System.ComponentModel.Composition.Hosting;

using CarRental.Data;

namespace CarRental.Business.Bootstrapper
{
    public static class MEFLoader
    {
        public static CompositionContainer Init()
        {
            var catalog = new AggregateCatalog();

            catalog.Catalogs.Add(new AssemblyCatalog(typeof(AccountRepository).Assembly));

            return new CompositionContainer(catalog);
        }
    }
}
