using System;

namespace VRMGames.CartridgeAndCloud.Application.PlayerAgency
{
    /// <summary>
    /// Session-scoped authority for the physical load carried by the player.
    /// Capacity is expressed in abstract carry units until product/box weight
    /// data exists. Any carried load blocks sprint by design.
    /// </summary>
    public sealed class PlayerCarryLoadService
    {
        public const int DefaultBaseCapacityUnits = 1;

        private int _baseCapacityUnits;
        private int _skillCapacityBonusUnits;
        private int _carriedUnits;

        public PlayerCarryLoadService(
            int baseCapacityUnits = DefaultBaseCapacityUnits)
        {
            if (baseCapacityUnits <= 0)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(baseCapacityUnits));
            }

            _baseCapacityUnits = baseCapacityUnits;
        }

        public int BaseCapacityUnits => _baseCapacityUnits;

        public int SkillCapacityBonusUnits =>
            _skillCapacityBonusUnits;

        public int CapacityUnits =>
            checked(_baseCapacityUnits + _skillCapacityBonusUnits);

        public int CarriedUnits => _carriedUnits;

        public int AvailableCapacityUnits =>
            CapacityUnits - _carriedUnits;

        public bool HasLoad => _carriedUnits > 0;

        public bool BlocksSprint => HasLoad;

        public event Action LoadChanged;

        public bool CanCarry(int additionalUnits)
        {
            return additionalUnits >= 0 &&
                   additionalUnits <= AvailableCapacityUnits;
        }

        public bool TryAdd(int units, out string reason)
        {
            if (units <= 0)
            {
                reason = "Carry units must be positive.";
                return false;
            }

            if (!CanCarry(units))
            {
                reason =
                    "Player carry capacity would be exceeded.";
                return false;
            }

            _carriedUnits = checked(_carriedUnits + units);
            reason = string.Empty;
            LoadChanged?.Invoke();
            return true;
        }

        public bool TryRemove(int units, out string reason)
        {
            if (units <= 0)
            {
                reason = "Carry units must be positive.";
                return false;
            }

            if (units > _carriedUnits)
            {
                reason =
                    "Cannot remove more carry units than are held.";
                return false;
            }

            _carriedUnits -= units;
            reason = string.Empty;
            LoadChanged?.Invoke();
            return true;
        }

        public bool TrySetSkillCapacityBonus(
            int bonusUnits,
            out string reason)
        {
            if (bonusUnits < 0)
            {
                reason =
                    "Carry capacity bonus cannot be negative.";
                return false;
            }

            int resultingCapacity = checked(
                _baseCapacityUnits + bonusUnits);

            if (_carriedUnits > resultingCapacity)
            {
                reason =
                    "Carry capacity cannot be reduced below the current load.";
                return false;
            }

            if (_skillCapacityBonusUnits == bonusUnits)
            {
                reason = string.Empty;
                return true;
            }

            _skillCapacityBonusUnits = bonusUnits;
            reason = string.Empty;
            LoadChanged?.Invoke();
            return true;
        }

        public void Clear()
        {
            if (_carriedUnits == 0)
            {
                return;
            }

            _carriedUnits = 0;
            LoadChanged?.Invoke();
        }
    }
}
