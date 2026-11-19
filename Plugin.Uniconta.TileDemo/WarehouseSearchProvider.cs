using global::Uniconta.ClientTools.Page;
using global::Uniconta.ClientTools.InfoLog;
using global::Uniconta.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Uniconta.ClientTools.Page;
using Uniconta.DataModel;
using UnicontaClient.Controls;

namespace Plugin.Uniconta.TileDemo
{
    public sealed class WarehouseSearchProvider : INavigationSearchProvider
    {
        public Task<NavigationSearchProviderResult> SearchAsync(
            string searchText,
            Company company,
            BasePage currentPage,
            CancellationToken cancellationToken)
        {
            var warehouses = new[]
            {
                new { Code = "WH001", Name = "Main Warehouse", City = "Copenhagen" },
                new { Code = "WH002", Name = "Spare Parts Warehouse", City = "Aarhus" },
                new { Code = "WH003", Name = "Distribution Center", City = "Odense" }
            };

            var results = warehouses
                .Where(w =>
                    w.Code.IndexOf(searchText, StringComparison.OrdinalIgnoreCase) >= 0 ||
                    w.Name.IndexOf(searchText, StringComparison.OrdinalIgnoreCase) >= 0 ||
                    w.City.IndexOf(searchText, StringComparison.OrdinalIgnoreCase) >= 0)
                .Select(w => new CachedSearchResult
                {
                    Key = w.Code,
                    Name = w.Name,
                    TableName = "Partner Warehouses",
                    MatchDescription = w.City,

                    // Custom navigation when the result is selected.
                    OpenAction = () =>
                    {
                        Log.Info($"Opening warehouse {w.Code}: {w.Name}");
                    }
                })
                .ToList();

            return Task.FromResult(new NavigationSearchProviderResult
            {
                DataResults = results
            });
        }

        private static readonly WarehouseSearchProvider warehouseSearchProvider = new WarehouseSearchProvider();

        public static void Initialize()
        {
            NavigationSearchExtensions.Register(warehouseSearchProvider);
        }

        public static void Dispose()
        {
            NavigationSearchExtensions.Unregister(warehouseSearchProvider);
        }
    }
}
