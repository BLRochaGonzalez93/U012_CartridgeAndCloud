using System;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using VRMGames.CartridgeAndCloud.Application.Employees;
using VRMGames.CartridgeAndCloud.Application.GameSession;
using VRMGames.CartridgeAndCloud.Application.Store;
using VRMGames.CartridgeAndCloud.Domain.Employees;
using VRMGames.CartridgeAndCloud.Domain.Persistence;
using VRMGames.CartridgeAndCloud.Domain.Products;
using VRMGames.CartridgeAndCloud.Domain.Store;
using VRMGames.CartridgeAndCloud.Presentation.Characters;
using VRMGames.CartridgeAndCloud.Presentation.Interaction;
using VRMGames.CartridgeAndCloud.Presentation.Products;
using VRMGames.CartridgeAndCloud.Runtime.Characters;
using VRMGames.CartridgeAndCloud.Runtime.Placement;

namespace VRMGames.CartridgeAndCloud.Runtime.UIUX
{
    public enum ContextualWorldFeedbackSeverity
    {
        Info = 0,
        Attention = 1,
        Warning = 2
    }

    public readonly struct ContextualWorldFeedbackItem
    {
        public string Key { get; }
        public Transform Anchor { get; }
        public string Text { get; }
        public ContextualWorldFeedbackSeverity Severity { get; }
        public float VerticalOffset { get; }

        public ContextualWorldFeedbackItem(
            string key,
            Transform anchor,
            string text,
            ContextualWorldFeedbackSeverity severity,
            float verticalOffset)
        {
            Key = key ?? string.Empty;
            Anchor = anchor;
            Text = text ?? string.Empty;
            Severity = severity;
            VerticalOffset = Mathf.Max(0f, verticalOffset);
        }
    }

    /// <summary>
    /// Read-only projection for contextual world feedback. It derives prompts
    /// and visible status markers from already-authoritative gameplay state.
    /// </summary>
    public sealed class ContextualWorldFeedbackProjection
    {
        private readonly StoreOperationsFacade _store;
        private readonly IStoreContentCatalog _catalog;
        private readonly IActiveGameSession _activeSession;
        private readonly EmployeeStateService _employeeState;

        public ContextualWorldFeedbackProjection(
            StoreOperationsFacade store,
            IStoreContentCatalog catalog,
            IActiveGameSession activeSession,
            EmployeeStateService employeeState)
        {
            _store = store ??
                throw new ArgumentNullException(nameof(store));
            _catalog = catalog ??
                throw new ArgumentNullException(nameof(catalog));
            _activeSession = activeSession ??
                throw new ArgumentNullException(nameof(activeSession));
            _employeeState = employeeState ??
                throw new ArgumentNullException(nameof(employeeState));
        }

        public string BuildTargetPrompt(
            IWorldInteractionTarget target,
            float distance)
        {
            if (!IsTargetAlive(target))
            {
                return string.Empty;
            }

            string label = ResolveTargetLabel(target);

            if (!target.IsInteractionAvailable)
            {
                return label + " · unavailable";
            }

            if (distance > target.InteractionRange)
            {
                return "Move closer · " + label +
                       " · " +
                       distance.ToString("0.0", CultureInfo.InvariantCulture) +
                       " m";
            }

            return "E · Inspect " + label;
        }

        public void CollectVisibleItems(
            List<ContextualWorldFeedbackItem> output)
        {
            if (output == null)
            {
                throw new ArgumentNullException(nameof(output));
            }

            output.Clear();

            if (!_activeSession.HasActiveSession)
            {
                return;
            }

            IntegratedGameStateSnapshot snapshot =
                _activeSession.Snapshot;

            CollectFixtureItems(snapshot, output);
            CollectCharacterItems(snapshot, output);
            CollectDeliveryItems(output);
        }

        private void CollectFixtureItems(
            IntegratedGameStateSnapshot snapshot,
            List<ContextualWorldFeedbackItem> output)
        {
            PlacedFixtureVisual[] fixtures =
                UnityEngine.Object.FindObjectsByType<PlacedFixtureVisual>(
                    FindObjectsInactive.Exclude,
                    FindObjectsSortMode.None);

            foreach (PlacedFixtureVisual fixture in fixtures)
            {
                if (fixture == null || !fixture.IsFixtureInteractive)
                {
                    continue;
                }

                if (fixture.InteractionKind == WorldInteractionKind.Display)
                {
                    if (string.IsNullOrWhiteSpace(
                            fixture.AssignedProductId))
                    {
                        output.Add(new ContextualWorldFeedbackItem(
                            "display-unassigned:" + fixture.InstanceId,
                            fixture.transform,
                            "Display without product",
                            ContextualWorldFeedbackSeverity.Attention,
                            1.9f));
                    }
                    else if (fixture.ProductQuantity == 0)
                    {
                        output.Add(new ContextualWorldFeedbackItem(
                            "display-empty:" + fixture.InstanceId,
                            fixture.transform,
                            "Empty display",
                            ContextualWorldFeedbackSeverity.Warning,
                            1.9f));
                    }

                    continue;
                }

                if (fixture.InteractionKind != WorldInteractionKind.Checkout)
                {
                    continue;
                }

                if (string.Equals(
                        snapshot.DayCycle.State,
                        "Open",
                        StringComparison.Ordinal) &&
                    string.Equals(
                        snapshot.CheckoutStation.State,
                        "Closed",
                        StringComparison.Ordinal))
                {
                    output.Add(new ContextualWorldFeedbackItem(
                        "checkout-closed:" + fixture.InstanceId,
                        fixture.transform,
                        "Checkout closed",
                        ContextualWorldFeedbackSeverity.Warning,
                        2.1f));
                }

                if (snapshot.QueueEntries.Count > 0)
                {
                    output.Add(new ContextualWorldFeedbackItem(
                        "checkout-queue:" + fixture.InstanceId,
                        fixture.transform,
                        "Queue · " + snapshot.QueueEntries.Count +
                        " customer(s)",
                        ContextualWorldFeedbackSeverity.Attention,
                        2.45f));
                }
            }
        }

        private void CollectCharacterItems(
            IntegratedGameStateSnapshot snapshot,
            List<ContextualWorldFeedbackItem> output)
        {
            CharacterPresence[] characters =
                UnityEngine.Object.FindObjectsByType<CharacterPresence>(
                    FindObjectsInactive.Exclude,
                    FindObjectsSortMode.None);

            foreach (CharacterPresence character in characters)
            {
                if (character == null ||
                    string.IsNullOrWhiteSpace(character.CharacterId))
                {
                    continue;
                }

                if (character.InteractionKind ==
                    WorldInteractionKind.Customer)
                {
                    AddCustomerQueueItem(
                        character,
                        snapshot,
                        output);
                    continue;
                }

                if (character.InteractionKind !=
                    WorldInteractionKind.Employee)
                {
                    continue;
                }

                if (!EmployeeId.TryParse(
                        character.CharacterId,
                        out EmployeeId employeeId) ||
                    !_employeeState.TryGetState(
                        employeeId,
                        out EmployeeStateSnapshot state) ||
                    !state.CanAcceptTasks)
                {
                    continue;
                }

                output.Add(new ContextualWorldFeedbackItem(
                    "employee-available:" + character.CharacterId,
                    character.transform,
                    "Employee available",
                    ContextualWorldFeedbackSeverity.Info,
                    2.25f));
            }
        }

        private static void AddCustomerQueueItem(
            CharacterPresence character,
            IntegratedGameStateSnapshot snapshot,
            List<ContextualWorldFeedbackItem> output)
        {
            CheckoutQueueEntrySaveRecord queueEntry = null;

            foreach (CheckoutQueueEntrySaveRecord candidate
                     in snapshot.QueueEntries)
            {
                if (!string.Equals(
                        candidate.CustomerId,
                        character.CharacterId,
                        StringComparison.Ordinal))
                {
                    continue;
                }

                queueEntry = candidate;
                break;
            }

            if (queueEntry == null)
            {
                return;
            }

            CustomerSaveRecord customer = null;
            foreach (CustomerSaveRecord candidate
                     in snapshot.Customers)
            {
                if (!string.Equals(
                        candidate.CustomerId,
                        character.CharacterId,
                        StringComparison.Ordinal))
                {
                    continue;
                }

                customer = candidate;
                break;
            }

            string text = "Waiting at checkout";
            if (customer != null)
            {
                text += " · " +
                    customer.RemainingPatienceSeconds.ToString(
                        CultureInfo.InvariantCulture) +
                    " s";
            }

            output.Add(new ContextualWorldFeedbackItem(
                "customer-queue:" + character.CharacterId,
                character.transform,
                text,
                ContextualWorldFeedbackSeverity.Attention,
                2.15f));
        }

        private void CollectDeliveryItems(
            List<ContextualWorldFeedbackItem> output)
        {
            SupplierDeliveryView[] deliveries =
                UnityEngine.Object.FindObjectsByType<SupplierDeliveryView>(
                    FindObjectsInactive.Exclude,
                    FindObjectsSortMode.None);

            foreach (SupplierDeliveryView delivery in deliveries)
            {
                if (delivery == null ||
                    string.IsNullOrWhiteSpace(delivery.InteractionId))
                {
                    continue;
                }

                StoreDeliveryRunRecord run = null;
                foreach (StoreDeliveryRunRecord candidate
                         in _store.State.DeliveryRuns)
                {
                    if (!string.Equals(
                            candidate.DeliveryRunId,
                            delivery.InteractionId,
                            StringComparison.Ordinal))
                    {
                        continue;
                    }

                    run = candidate;
                    break;
                }

                if (run == null ||
                    run.Status != StoreDeliveryRunStatus.InTransit)
                {
                    continue;
                }

                output.Add(new ContextualWorldFeedbackItem(
                    "delivery-pending:" + delivery.InteractionId,
                    delivery.InteractionTransform,
                    "Delivery pending",
                    ContextualWorldFeedbackSeverity.Attention,
                    1.65f));
            }
        }

        private string ResolveTargetLabel(
            IWorldInteractionTarget target)
        {
            if (target is PlacedFixtureVisual fixture)
            {
                StoreFixtureDefinition definition;
                if (_catalog.TryGetFurniture(
                        fixture.DefinitionId,
                        out definition) &&
                    definition != null)
                {
                    return definition.DisplayName;
                }
            }

            if (target is ProductVisualMarker productMarker)
            {
                RetailProductDefinition product;
                if (_catalog.TryGetProduct(
                        productMarker.ProductId,
                        out product) &&
                    product != null)
                {
                    return product.DisplayName;
                }
            }

            switch (target.InteractionKind)
            {
                case WorldInteractionKind.Display:
                    return "display";
                case WorldInteractionKind.Checkout:
                    return "checkout";
                case WorldInteractionKind.Container:
                    return "storage";
                case WorldInteractionKind.Product:
                    return "product";
                case WorldInteractionKind.Customer:
                    return "customer";
                case WorldInteractionKind.Employee:
                    return "employee";
                case WorldInteractionKind.Supplier:
                    return "supplier";
                case WorldInteractionKind.Delivery:
                    return "delivery";
                case WorldInteractionKind.Door:
                    return "door";
                case WorldInteractionKind.Fixture:
                    return "fixture";
                default:
                    return "object";
            }
        }

        private static bool IsTargetAlive(
            IWorldInteractionTarget target)
        {
            if (target == null)
            {
                return false;
            }

            if (target is Behaviour behaviour &&
                !behaviour.isActiveAndEnabled)
            {
                return false;
            }

            if (target is UnityEngine.Object unityObject)
            {
                return unityObject != null;
            }

            return true;
        }
    }
}
