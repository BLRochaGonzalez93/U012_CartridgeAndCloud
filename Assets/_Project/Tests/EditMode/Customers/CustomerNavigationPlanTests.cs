using System;
using NUnit.Framework;
using VRMGames.CartridgeAndCloud.Domain.Customers;

namespace VRMGames.CartridgeAndCloud.Tests.EditMode.Customers
{
    public sealed class CustomerNavigationPlanTests
    {
        [Test] public void Target_NegativeDwell_Throws() { Assert.Throws<ArgumentOutOfRangeException>(() => Target("browse", CustomerNavigationTargetType.Browse, -1)); }
        [Test] public void Target_EntryWithDwell_Throws() { Assert.Throws<ArgumentException>(() => Target("entry", CustomerNavigationTargetType.Entry, 1)); }
        [Test] public void Plan_FewerThanTwoTargets_Throws() { Assert.Throws<ArgumentException>(() => new CustomerNavigationPlan(new[] { Target("entry", CustomerNavigationTargetType.Entry, 0) })); }
        [Test] public void Plan_FirstNotEntry_Throws() { Assert.Throws<ArgumentException>(() => new CustomerNavigationPlan(new[] { Target("browse", CustomerNavigationTargetType.Browse, 0), Target("exit", CustomerNavigationTargetType.Exit, 0) })); }
        [Test] public void Plan_LastNotExit_Throws() { Assert.Throws<ArgumentException>(() => new CustomerNavigationPlan(new[] { Target("entry", CustomerNavigationTargetType.Entry, 0), Target("browse", CustomerNavigationTargetType.Browse, 0) })); }
        [Test] public void Plan_IntermediateNotBrowse_Throws() { Assert.Throws<ArgumentException>(() => new CustomerNavigationPlan(new[] { Target("entry", CustomerNavigationTargetType.Entry, 0), Target("entry2", CustomerNavigationTargetType.Entry, 0), Target("exit", CustomerNavigationTargetType.Exit, 0) })); }
        [Test] public void Plan_DuplicatePoint_Throws() { Assert.Throws<ArgumentException>(() => new CustomerNavigationPlan(new[] { Target("same", CustomerNavigationTargetType.Entry, 0), Target("same", CustomerNavigationTargetType.Exit, 0) })); }
        [Test] public void ValidPlan_ExposesEntryAndExit() { CustomerNavigationPlan p = Create(); Assert.That(p.EntryTarget.Type, Is.EqualTo(CustomerNavigationTargetType.Entry)); Assert.That(p.ExitTarget.Type, Is.EqualTo(CustomerNavigationTargetType.Exit)); }

        private static CustomerNavigationTarget Target(string id, CustomerNavigationTargetType type, int dwell) { return new CustomerNavigationTarget(new CustomerNavigationPointId(id), type, dwell); }
        private static CustomerNavigationPlan Create() { return new CustomerNavigationPlan(new[] { Target("entry", CustomerNavigationTargetType.Entry, 0), Target("browse", CustomerNavigationTargetType.Browse, 2), Target("exit", CustomerNavigationTargetType.Exit, 0) }); }
    }
}
