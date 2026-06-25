using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace VRMGames.CartridgeAndCloud.Domain.Customers
{
    public sealed class CustomerNavigationPlan
    {
        private readonly ReadOnlyCollection<CustomerNavigationTarget>
            _targets;

        public IReadOnlyList<CustomerNavigationTarget> Targets => _targets;

        public int Count => _targets.Count;

        public CustomerNavigationTarget EntryTarget => _targets[0];

        public CustomerNavigationTarget ExitTarget =>
            _targets[_targets.Count - 1];

        public CustomerNavigationPlan(
            IEnumerable<CustomerNavigationTarget> targets)
        {
            if (targets == null)
            {
                throw new ArgumentNullException(nameof(targets));
            }

            List<CustomerNavigationTarget> list =
                new List<CustomerNavigationTarget>();
            HashSet<CustomerNavigationPointId> unique =
                new HashSet<CustomerNavigationPointId>();

            foreach (CustomerNavigationTarget target in targets)
            {
                if (target == null)
                {
                    throw new ArgumentException(
                        "Navigation targets cannot contain null.",
                        nameof(targets));
                }

                if (!unique.Add(target.PointId))
                {
                    throw new ArgumentException(
                        $"Navigation point {target.PointId} is duplicated.",
                        nameof(targets));
                }

                list.Add(target);
            }

            if (list.Count < 2)
            {
                throw new ArgumentException(
                    "A navigation plan requires entry and exit targets.",
                    nameof(targets));
            }

            if (list[0].Type != CustomerNavigationTargetType.Entry)
            {
                throw new ArgumentException(
                    "The first navigation target must be Entry.",
                    nameof(targets));
            }

            if (list[list.Count - 1].Type !=
                CustomerNavigationTargetType.Exit)
            {
                throw new ArgumentException(
                    "The last navigation target must be Exit.",
                    nameof(targets));
            }

            for (int i = 1; i < list.Count - 1; i++)
            {
                if (list[i].Type != CustomerNavigationTargetType.Browse)
                {
                    throw new ArgumentException(
                        "Intermediate navigation targets must be Browse.",
                        nameof(targets));
                }
            }

            _targets =
                new ReadOnlyCollection<CustomerNavigationTarget>(list);
        }
    }
}
