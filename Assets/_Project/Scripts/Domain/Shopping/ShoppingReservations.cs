using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using VRMGames.CartridgeAndCloud.Domain.Customers;
using VRMGames.CartridgeAndCloud.Domain.Displays;
using VRMGames.CartridgeAndCloud.Domain.Inventory;
using VRMGames.CartridgeAndCloud.Domain.Products;

namespace VRMGames.CartridgeAndCloud.Domain.Shopping
{
    public enum ShoppingReservationState
    {
        Active = 0,
        Released = 1,
        Consumed = 2
    }

    public sealed class ShoppingReservation
    {
        public ShoppingReservationId Id { get; }
        public CustomerInstanceId CustomerId { get; }
        public DisplayInstanceId DisplayId { get; }
        public ProductDefinitionId ProductId { get; }
        public Quantity Quantity { get; }
        public ShoppingReservationState State { get; private set; }
        public bool IsActive => State == ShoppingReservationState.Active;

        public ShoppingReservation(
            ShoppingReservationId id,
            CustomerInstanceId customerId,
            DisplayInstanceId displayId,
            ProductDefinitionId productId,
            Quantity quantity)
        {
            if (string.IsNullOrWhiteSpace(customerId.Value))
                throw new ArgumentException("Customer ID must be initialized.", nameof(customerId));
            if (string.IsNullOrWhiteSpace(displayId.Value))
                throw new ArgumentException("Display ID must be initialized.", nameof(displayId));
            if (string.IsNullOrWhiteSpace(productId.Value))
                throw new ArgumentException("Product ID must be initialized.", nameof(productId));
            if (quantity.IsZero)
                throw new ArgumentOutOfRangeException(nameof(quantity));
            Id = id;
            CustomerId = customerId;
            DisplayId = displayId;
            ProductId = productId;
            Quantity = quantity;
            State = ShoppingReservationState.Active;
        }

        public bool TryRelease()
        {
            if (!IsActive) return false;
            State = ShoppingReservationState.Released;
            return true;
        }

        public bool TryConsume()
        {
            if (!IsActive) return false;
            State = ShoppingReservationState.Consumed;
            return true;
        }

        public bool TryRollbackConsumption()
        {
            if (State != ShoppingReservationState.Consumed)
                return false;
            State = ShoppingReservationState.Active;
            return true;
        }
    }

    public sealed class ShoppingReservationRegistry
    {
        private readonly Dictionary<ShoppingReservationId, ShoppingReservation> _byId =
            new Dictionary<ShoppingReservationId, ShoppingReservation>();

        public int Count => _byId.Count;

        public bool TryRegister(ShoppingReservation reservation)
        {
            if (reservation == null) throw new ArgumentNullException(nameof(reservation));
            if (_byId.ContainsKey(reservation.Id)) return false;
            _byId.Add(reservation.Id, reservation);
            return true;
        }

        public bool Contains(ShoppingReservationId id) => _byId.ContainsKey(id);

        public bool TryGet(ShoppingReservationId id, out ShoppingReservation reservation) =>
            _byId.TryGetValue(id, out reservation);

        public ShoppingReservation Get(ShoppingReservationId id)
        {
            if (!_byId.TryGetValue(id, out ShoppingReservation reservation))
                throw new KeyNotFoundException($"Shopping reservation {id} was not found.");
            return reservation;
        }

        public Quantity GetActiveReservedQuantity(DisplayInstanceId displayId, ProductDefinitionId productId)
        {
            int total = 0;
            foreach (ShoppingReservation reservation in _byId.Values)
            {
                if (reservation.IsActive &&
                    reservation.DisplayId == displayId &&
                    reservation.ProductId == productId)
                {
                    total = checked(total + reservation.Quantity.Value);
                }
            }
            return new Quantity(total);
        }

        public IReadOnlyList<ShoppingReservation> GetForCustomer(CustomerInstanceId customerId, bool activeOnly)
        {
            List<ShoppingReservation> matches = new List<ShoppingReservation>();
            foreach (ShoppingReservation reservation in _byId.Values)
            {
                if (reservation.CustomerId == customerId && (!activeOnly || reservation.IsActive))
                    matches.Add(reservation);
            }
            matches.Sort((left, right) =>
                StringComparer.Ordinal.Compare(left.Id.Value, right.Id.Value));
            return new ReadOnlyCollection<ShoppingReservation>(matches);
        }
    }
}
