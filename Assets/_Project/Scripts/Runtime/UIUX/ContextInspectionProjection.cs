using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using VRMGames.CartridgeAndCloud.Application.Employees;
using VRMGames.CartridgeAndCloud.Application.GameSession;
using VRMGames.CartridgeAndCloud.Application.Store;
using VRMGames.CartridgeAndCloud.Application.UIUX;
using VRMGames.CartridgeAndCloud.Domain.Employees;
using VRMGames.CartridgeAndCloud.Domain.Persistence;
using VRMGames.CartridgeAndCloud.Domain.Products;
using VRMGames.CartridgeAndCloud.Domain.Store;
using VRMGames.CartridgeAndCloud.Presentation.Characters;
using VRMGames.CartridgeAndCloud.Presentation.Interaction;
using VRMGames.CartridgeAndCloud.Presentation.Products;
using VRMGames.CartridgeAndCloud.Presentation.Store.Doors;
using VRMGames.CartridgeAndCloud.Runtime.Characters;
using VRMGames.CartridgeAndCloud.Runtime.Placement;

namespace VRMGames.CartridgeAndCloud.Runtime.UIUX
{
    internal readonly struct ContextInspectionContent
    {
        public string Title { get; }
        public string Subtitle { get; }
        public string Body { get; }

        public ContextInspectionContent(
            string title,
            string subtitle,
            string body)
        {
            Title = string.IsNullOrWhiteSpace(title)
                ? "Inspection"
                : title;
            Subtitle = subtitle ?? string.Empty;
            Body = body ?? string.Empty;
        }
    }

    /// <summary>
    /// Projects existing authoritative state into read-only contextual rows.
    /// No state is owned or mutated here.
    /// </summary>
    internal sealed class ContextInspectionProjection
    {
        private readonly StoreOperationsFacade _store;
        private readonly IStoreContentCatalog _catalog;
        private readonly IActiveGameSession _activeSession;
        private readonly EmployeeHiringService _employeeHiring;
        private readonly EmployeeScheduleService _employeeSchedule;
        private readonly EmployeeStateService _employeeState;

        public ContextInspectionProjection(
            StoreOperationsFacade store,
            IStoreContentCatalog catalog,
            IActiveGameSession activeSession,
            EmployeeHiringService employeeHiring,
            EmployeeScheduleService employeeSchedule,
            EmployeeStateService employeeState)
        {
            _store = store ??
                throw new ArgumentNullException(nameof(store));
            _catalog = catalog ??
                throw new ArgumentNullException(nameof(catalog));
            _activeSession = activeSession ??
                throw new ArgumentNullException(nameof(activeSession));
            _employeeHiring = employeeHiring ??
                throw new ArgumentNullException(nameof(employeeHiring));
            _employeeSchedule = employeeSchedule ??
                throw new ArgumentNullException(nameof(employeeSchedule));
            _employeeState = employeeState ??
                throw new ArgumentNullException(nameof(employeeState));
        }

        public ContextInspectionContent Build(
            IWorldInteractionTarget target,
            float distance)
        {
            if (target == null)
            {
                throw new ArgumentNullException(nameof(target));
            }

            List<InspectionLine> lines =
                new List<InspectionLine>();

            string title = Humanize(target.InteractionKind);
            string subtitle =
                "ID  " + Safe(target.InteractionId) +
                "   ·   Distance " +
                Math.Max(0f, distance).ToString(
                    "0.0",
                    CultureInfo.InvariantCulture) +
                " m";

            if (target is PlacedFixtureVisual fixture)
            {
                title = AddFixtureInspection(
                    fixture,
                    target.InteractionKind,
                    lines);
            }
            else if (target is ProductVisualMarker product)
            {
                title = AddProductInspection(product, lines);
            }
            else if (target is CharacterPresence character)
            {
                title = AddCharacterInspection(character, lines);
            }
            else if (target is SupplierDeliveryView delivery)
            {
                title = AddDeliveryInspection(delivery, lines);
            }
            else if (target is AutomaticSlidingDoorController door)
            {
                title = "Store entrance";
                lines.Add(new InspectionLine(
                    "State",
                    door.IsOpen ? "Open" : "Closed"));
                lines.Add(new InspectionLine(
                    "Interaction",
                    "Entrance door"));
            }
            else
            {
                lines.Add(new InspectionLine(
                    "Type",
                    Humanize(target.InteractionKind)));
                lines.Add(new InspectionLine(
                    "Available",
                    target.IsInteractionAvailable ? "Yes" : "No"));
            }

            if (lines.Count == 0)
            {
                lines.Add(new InspectionLine(
                    "Status",
                    "No specialized inspection data"));
            }

            return new ContextInspectionContent(
                title,
                subtitle,
                FormatLines(lines));
        }

        private string AddFixtureInspection(
            PlacedFixtureVisual fixture,
            WorldInteractionKind interactionKind,
            List<InspectionLine> lines)
        {
            StoreFixtureDefinition definition;
            _catalog.TryGetFurniture(
                fixture.DefinitionId,
                out definition);

            string title = definition != null
                ? definition.DisplayName
                : Humanize(interactionKind);

            lines.Add(new InspectionLine(
                "Type",
                definition != null
                    ? definition.Kind.ToString()
                    : fixture.FixtureKind.ToString()));
            lines.Add(new InspectionLine(
                "Definition",
                fixture.DefinitionId));

            if (definition != null && definition.Capacity > 0)
            {
                lines.Add(new InspectionLine(
                    "Capacity",
                    definition.Capacity.ToString(
                        CultureInfo.InvariantCulture)));
            }

            switch (interactionKind)
            {
                case WorldInteractionKind.Display:
                    AddDisplayInspection(
                        fixture,
                        definition,
                        lines);
                    break;
                case WorldInteractionKind.Checkout:
                    AddCheckoutInspection(lines);
                    break;
                case WorldInteractionKind.Container:
                    AddContainerInspection(
                        fixture,
                        definition,
                        lines);
                    break;
            }

            return title;
        }

        private void AddDisplayInspection(
            PlacedFixtureVisual fixture,
            StoreFixtureDefinition definition,
            List<InspectionLine> lines)
        {
            int capacity = definition == null
                ? 0
                : definition.Capacity;

            if (string.IsNullOrWhiteSpace(
                    fixture.AssignedProductId))
            {
                lines.Add(new InspectionLine(
                    "Product",
                    "Empty / unassigned"));
                lines.Add(new InspectionLine(
                    "Stock",
                    capacity > 0
                        ? "0 / " + capacity
                        : "0"));
                return;
            }

            RetailProductDefinition product;
            bool found = _catalog.TryGetProduct(
                fixture.AssignedProductId,
                out product);

            lines.Add(new InspectionLine(
                "Product",
                found
                    ? product.DisplayName
                    : fixture.AssignedProductId));
            lines.Add(new InspectionLine(
                "Product ID",
                fixture.AssignedProductId));
            lines.Add(new InspectionLine(
                "Stock",
                capacity > 0
                    ? fixture.ProductQuantity + " / " + capacity
                    : fixture.ProductQuantity.ToString(
                        CultureInfo.InvariantCulture)));

            if (found)
            {
                lines.Add(new InspectionLine(
                    "Sale price",
                    FormatMoney(product.SalePriceCents)));
                lines.Add(new InspectionLine(
                    "Category",
                    product.Kind.ToString()));
            }
        }

        private void AddCheckoutInspection(
            List<InspectionLine> lines)
        {
            if (!_activeSession.HasActiveSession)
            {
                lines.Add(new InspectionLine(
                    "Station",
                    "No active session"));
                return;
            }

            IntegratedGameStateSnapshot snapshot =
                _activeSession.Snapshot;

            lines.Add(new InspectionLine(
                "Station",
                snapshot.CheckoutStation.State));
            lines.Add(new InspectionLine(
                "Queue",
                snapshot.QueueEntries.Count.ToString(
                    CultureInfo.InvariantCulture) +
                " customer(s)"));

            if (snapshot.QueueEntries.Count > 0)
            {
                CheckoutQueueEntrySaveRecord current =
                    snapshot.QueueEntries[0];
                lines.Add(new InspectionLine(
                    "First customer",
                    current.CustomerId));
                lines.Add(new InspectionLine(
                    "Queue state",
                    current.State));
            }
        }

        private void AddContainerInspection(
            PlacedFixtureVisual fixture,
            StoreFixtureDefinition definition,
            List<InspectionLine> lines)
        {
            if (fixture.FixtureKind !=
                StoreFixtureKind.BackroomStorage)
            {
                AddDisplayInspection(fixture, definition, lines);
                return;
            }

            InventoryContainerSaveRecord warehouse =
                FindInventory(StoreOperationsFacade.BackroomContainerId);

            if (warehouse == null)
            {
                lines.Add(new InspectionLine(
                    "Contents",
                    "Backroom inventory unavailable"));
                return;
            }

            int total = 0;
            foreach (ProductQuantitySaveRecord stock
                     in warehouse.Products)
            {
                total = checked(total + stock.Quantity);
            }

            lines.Add(new InspectionLine(
                "Stored units",
                total.ToString(CultureInfo.InvariantCulture) +
                " / " +
                warehouse.Capacity.ToString(
                    CultureInfo.InvariantCulture)));
            lines.Add(new InspectionLine(
                "Product lines",
                warehouse.Products.Count.ToString(
                    CultureInfo.InvariantCulture)));

            if (warehouse.Products.Count == 0)
            {
                lines.Add(new InspectionLine(
                    "Contents",
                    "Empty"));
                return;
            }

            int shown = 0;
            foreach (ProductQuantitySaveRecord stock
                     in warehouse.Products)
            {
                if (shown >= 8)
                {
                    lines.Add(new InspectionLine(
                        "More",
                        "+" + (warehouse.Products.Count - shown) +
                        " additional line(s)"));
                    break;
                }

                RetailProductDefinition product;
                string label = _catalog.TryGetProduct(
                        stock.ProductId,
                        out product)
                    ? product.DisplayName
                    : stock.ProductId;

                lines.Add(new InspectionLine(
                    label,
                    stock.Quantity.ToString(
                        CultureInfo.InvariantCulture)));
                shown++;
            }
        }

        private string AddProductInspection(
            ProductVisualMarker marker,
            List<InspectionLine> lines)
        {
            RetailProductDefinition product;
            if (!_catalog.TryGetProduct(
                    marker.ProductId,
                    out product))
            {
                lines.Add(new InspectionLine(
                    "Product ID",
                    marker.ProductId));
                lines.Add(new InspectionLine(
                    "Catalog",
                    "Definition not found"));
                return "Product";
            }

            lines.Add(new InspectionLine(
                "Product ID",
                product.ProductId));
            lines.Add(new InspectionLine(
                "Category",
                product.Kind.ToString()));
            lines.Add(new InspectionLine(
                "Sale price",
                FormatMoney(product.SalePriceCents)));
            lines.Add(new InspectionLine(
                "Wholesale",
                FormatMoney(product.WholesalePriceCents)));
            lines.Add(new InspectionLine(
                "Units / case",
                product.UnitsPerCase.ToString(
                    CultureInfo.InvariantCulture)));

            return product.DisplayName;
        }

        private string AddCharacterInspection(
            CharacterPresence character,
            List<InspectionLine> lines)
        {
            switch (character.InteractionKind)
            {
                case WorldInteractionKind.Employee:
                    return AddEmployeeInspection(
                        character,
                        lines);
                case WorldInteractionKind.Customer:
                    return AddCustomerInspection(
                        character,
                        lines);
                case WorldInteractionKind.Supplier:
                    lines.Add(new InspectionLine(
                        "Role",
                        "Supplier"));
                    lines.Add(new InspectionLine(
                        "Actor ID",
                        character.CharacterId));
                    return "Supplier";
                default:
                    lines.Add(new InspectionLine(
                        "Role",
                        character.Role.ToString()));
                    return Humanize(character.InteractionKind);
            }
        }

        private string AddEmployeeInspection(
            CharacterPresence character,
            List<InspectionLine> lines)
        {
            EmployeeId employeeId;
            if (!EmployeeId.TryParse(
                    character.CharacterId,
                    out employeeId))
            {
                lines.Add(new InspectionLine(
                    "Employee ID",
                    character.CharacterId));
                lines.Add(new InspectionLine(
                    "Roster",
                    "Legacy / non-persistent actor"));
                return "Employee";
            }

            HiredEmployee employee = FindEmployee(employeeId);
            if (employee == null)
            {
                lines.Add(new InspectionLine(
                    "Employee ID",
                    character.CharacterId));
                lines.Add(new InspectionLine(
                    "Roster",
                    "Employee not found"));
                return "Employee";
            }

            lines.Add(new InspectionLine(
                "Employee ID",
                employee.EmployeeId.ToString()));
            lines.Add(new InspectionLine(
                "Profile",
                employee.Profile + " · " + employee.Seniority));
            lines.Add(new InspectionLine(
                "Trait",
                employee.Trait));
            lines.Add(new InspectionLine(
                "Daily salary",
                FormatMoney(employee.ContractedDailySalaryCents)));
            lines.Add(new InspectionLine(
                "Start day",
                employee.StartDay.ToString(
                    CultureInfo.InvariantCulture)));

            EmployeeStateSnapshot state;
            if (_employeeState.TryGetState(
                    employee.EmployeeId,
                    out state))
            {
                lines.Add(new InspectionLine(
                    "State",
                    state.State.ToString()));
                lines.Add(new InspectionLine(
                    "Task-ready",
                    state.CanAcceptTasks ? "Yes" : "No"));
            }

            EmployeeShiftDefinition shift =
                _employeeSchedule.GetConfiguredShift(
                    employee.EmployeeId);
            lines.Add(new InspectionLine(
                "Shift",
                shift.DisplayName + " · " +
                FormatMinute(shift.StartMinute) +
                "–" + FormatMinute(shift.EndMinute)));

            if (shift.BreakDurationMinutes > 0)
            {
                lines.Add(new InspectionLine(
                    "Break",
                    FormatMinute(shift.BreakStartMinute) +
                    "–" + FormatMinute(shift.BreakEndMinute)));
            }

            return employee.DisplayName;
        }

        private string AddCustomerInspection(
            CharacterPresence character,
            List<InspectionLine> lines)
        {
            if (!_activeSession.HasActiveSession)
            {
                lines.Add(new InspectionLine(
                    "Customer ID",
                    character.CharacterId));
                return "Customer";
            }

            IntegratedGameStateSnapshot snapshot =
                _activeSession.Snapshot;
            CustomerSaveRecord customer = null;

            foreach (CustomerSaveRecord candidate
                     in snapshot.Customers)
            {
                if (string.Equals(
                        candidate.CustomerId,
                        character.CharacterId,
                        StringComparison.Ordinal))
                {
                    customer = candidate;
                    break;
                }
            }

            if (customer == null)
            {
                lines.Add(new InspectionLine(
                    "Customer ID",
                    character.CharacterId));
                lines.Add(new InspectionLine(
                    "State",
                    "Runtime customer"));
                return "Customer";
            }

            lines.Add(new InspectionLine(
                "Customer ID",
                customer.CustomerId));
            lines.Add(new InspectionLine(
                "Profile",
                customer.ProfileId));
            lines.Add(new InspectionLine(
                "State",
                customer.State));
            lines.Add(new InspectionLine(
                "Patience",
                customer.RemainingPatienceSeconds.ToString(
                    CultureInfo.InvariantCulture) + " s"));

            foreach (ShoppingSessionSaveRecord session
                     in snapshot.ShoppingSessions)
            {
                if (!string.Equals(
                        session.CustomerId,
                        customer.CustomerId,
                        StringComparison.Ordinal))
                {
                    continue;
                }

                lines.Add(new InspectionLine(
                    "Shopping",
                    session.State));
                lines.Add(new InspectionLine(
                    "Intent",
                    session.IntentId));
                break;
            }

            int reservedUnits = 0;
            foreach (ReservationSaveRecord reservation
                     in snapshot.Reservations)
            {
                if (string.Equals(
                        reservation.CustomerId,
                        customer.CustomerId,
                        StringComparison.Ordinal) &&
                    string.Equals(
                        reservation.State,
                        "Active",
                        StringComparison.Ordinal))
                {
                    reservedUnits = checked(
                        reservedUnits + reservation.Quantity);
                }
            }

            lines.Add(new InspectionLine(
                "Reserved units",
                reservedUnits.ToString(
                    CultureInfo.InvariantCulture)));

            return "Customer · " + customer.ProfileId;
        }

        private string AddDeliveryInspection(
            SupplierDeliveryView delivery,
            List<InspectionLine> lines)
        {
            StoreDeliveryRunRecord run = null;

            if (_store.State != null)
            {
                foreach (StoreDeliveryRunRecord candidate
                         in _store.State.DeliveryRuns)
                {
                    if (string.Equals(
                            candidate.DeliveryRunId,
                            delivery.InteractionId,
                            StringComparison.Ordinal))
                    {
                        run = candidate;
                        break;
                    }
                }
            }

            if (run == null)
            {
                lines.Add(new InspectionLine(
                    "Delivery ID",
                    delivery.InteractionId));
                lines.Add(new InspectionLine(
                    "Status",
                    "Runtime delivery"));
                return "Delivery";
            }

            lines.Add(new InspectionLine(
                "Delivery ID",
                run.DeliveryRunId));
            lines.Add(new InspectionLine(
                "Status",
                run.Status.ToString()));
            lines.Add(new InspectionLine(
                "Orders",
                run.OrderIds.Count.ToString(
                    CultureInfo.InvariantCulture)));
            lines.Add(new InspectionLine(
                "Total cost",
                FormatMoney(run.TotalCostCents)));

            return "Supplier delivery";
        }

        private InventoryContainerSaveRecord FindInventory(
            string containerId)
        {
            if (!_activeSession.HasActiveSession)
            {
                return null;
            }

            foreach (InventoryContainerSaveRecord inventory
                     in _activeSession.Snapshot.Inventories)
            {
                if (string.Equals(
                        inventory.ContainerId,
                        containerId,
                        StringComparison.Ordinal))
                {
                    return inventory;
                }
            }

            return null;
        }

        private HiredEmployee FindEmployee(
            EmployeeId employeeId)
        {
            foreach (HiredEmployee employee
                     in _employeeHiring.Employees)
            {
                if (employee.EmployeeId == employeeId)
                {
                    return employee;
                }
            }

            return null;
        }

        private string FormatMoney(long cents)
        {
            string currency =
                _activeSession.HasActiveSession
                    ? _activeSession.Snapshot.CurrencyCode
                    : "EUR";

            return StoreUiProjectionService.FormatMoney(
                cents,
                currency);
        }

        private static string FormatMinute(int minute)
        {
            int clamped = Math.Max(
                0,
                Math.Min(24 * 60, minute));
            int hour = clamped / 60;
            int minutes = clamped % 60;

            return hour.ToString(
                       "00",
                       CultureInfo.InvariantCulture) +
                   ":" +
                   minutes.ToString(
                       "00",
                       CultureInfo.InvariantCulture);
        }

        private static string FormatLines(
            IReadOnlyList<InspectionLine> lines)
        {
            StringBuilder builder = new StringBuilder();

            for (int index = 0; index < lines.Count; index++)
            {
                InspectionLine line = lines[index];
                builder.Append(line.Label);
                builder.Append("\n  ");
                builder.Append(line.Value);

                if (index < lines.Count - 1)
                {
                    builder.Append("\n\n");
                }
            }

            return builder.ToString();
        }

        private static string Humanize(
            WorldInteractionKind kind)
        {
            switch (kind)
            {
                case WorldInteractionKind.Checkout:
                    return "Checkout";
                case WorldInteractionKind.Container:
                    return "Storage";
                case WorldInteractionKind.Product:
                    return "Product";
                case WorldInteractionKind.Customer:
                    return "Customer";
                case WorldInteractionKind.Employee:
                    return "Employee";
                case WorldInteractionKind.Supplier:
                    return "Supplier";
                case WorldInteractionKind.Delivery:
                    return "Delivery";
                case WorldInteractionKind.Door:
                    return "Door";
                case WorldInteractionKind.Display:
                    return "Display";
                case WorldInteractionKind.Fixture:
                    return "Fixture";
                default:
                    return "Inspection";
            }
        }

        private static string Safe(string value)
        {
            return string.IsNullOrWhiteSpace(value)
                ? "—"
                : value;
        }

        private readonly struct InspectionLine
        {
            public string Label { get; }
            public string Value { get; }

            public InspectionLine(
                string label,
                string value)
            {
                Label = string.IsNullOrWhiteSpace(label)
                    ? "Info"
                    : label;
                Value = value ?? string.Empty;
            }
        }
    }
}
