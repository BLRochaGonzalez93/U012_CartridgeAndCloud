using System;
using VRMGames.CartridgeAndCloud.Application.Employees;
using VRMGames.CartridgeAndCloud.Application.Store;
using VRMGames.CartridgeAndCloud.Domain.Store;

namespace VRMGames.CartridgeAndCloud.Runtime.Employees
{
    internal sealed class StoreEmployeeHiringAccess :
        IEmployeeHiringStoreAccess,
        IEmployeePayrollStoreAccess
    {
        private readonly StoreOperationsFacade _store;
        private readonly IStoreContentCatalog _catalog;

        public long AvailableCashCents =>
            _store.AvailableCashCents;

        public bool HasValidWorkArea
        {
            get
            {
                if (_store.State == null)
                {
                    return false;
                }

                foreach (PlacedStoreFixtureRecord fixture
                         in _store.State.Fixtures)
                {
                    if (!_catalog.TryGetFurniture(
                            fixture.DefinitionId,
                            out StoreFixtureDefinition definition))
                    {
                        continue;
                    }

                    if (definition.Kind == StoreFixtureKind.CheckoutCounter)
                    {
                        return true;
                    }
                }

                return false;
            }
        }

        public StoreEmployeeHiringAccess(
            StoreOperationsFacade store,
            IStoreContentCatalog catalog)
        {
            _store = store ??
                throw new ArgumentNullException(nameof(store));
            _catalog = catalog ??
                throw new ArgumentNullException(nameof(catalog));
        }
    }
}
