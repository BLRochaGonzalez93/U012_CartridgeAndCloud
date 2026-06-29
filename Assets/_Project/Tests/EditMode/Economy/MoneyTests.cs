using NUnit.Framework;
using VRMGames.CartridgeAndCloud.Domain.Economy;

namespace VRMGames.CartridgeAndCloud.Tests.EditMode.Economy
{
    public sealed class MoneyTests
    {
        [Test] public void Currency_RejectsEmpty() =>
            Assert.Throws<System.ArgumentException>(
                () => new CurrencyCode(" "));

        [Test] public void Currency_RequiresThreeCharacters() =>
            Assert.Throws<System.ArgumentException>(
                () => new CurrencyCode("EU"));

        [Test] public void Currency_RejectsDigits() =>
            Assert.Throws<System.ArgumentException>(
                () => new CurrencyCode("E1R"));

        [Test] public void Currency_NormalizesCase()
        {
            Assert.That(
                new CurrencyCode("eur").Value,
                Is.EqualTo("EUR"));
        }

        [Test] public void Currency_TrimsWhitespace()
        {
            Assert.That(
                new CurrencyCode(" eur ").Value,
                Is.EqualTo("EUR"));
        }

        [Test] public void Currency_UsesOrdinalEquality()
        {
            Assert.That(
                new CurrencyCode("EUR") ==
                new CurrencyCode("eur"),
                Is.True);
        }

        [Test] public void Money_StoresSignedMinorUnits()
        {
            Assert.That(
                EconomyTestFactory.EurMoney(-100)
                    .MinorUnits,
                Is.EqualTo(-100));
        }

        [Test] public void Money_ZeroUsesCurrency()
        {
            Money zero = Money.Zero(
                EconomyTestFactory.Eur);
            Assert.That(zero.IsZero, Is.True);
            Assert.That(
                zero.Currency,
                Is.EqualTo(EconomyTestFactory.Eur));
        }

        [Test] public void Money_AddsExactly()
        {
            Money result =
                EconomyTestFactory.EurMoney(125)
                    .Add(
                        EconomyTestFactory.EurMoney(75));
            Assert.That(
                result.MinorUnits,
                Is.EqualTo(200));
        }

        [Test] public void Money_SubtractsExactly()
        {
            Money result =
                EconomyTestFactory.EurMoney(125)
                    .Subtract(
                        EconomyTestFactory.EurMoney(200));
            Assert.That(
                result.MinorUnits,
                Is.EqualTo(-75));
        }

        [Test] public void Money_MultipliesExactly()
        {
            Money result =
                EconomyTestFactory.EurMoney(1299)
                    .Multiply(6);
            Assert.That(
                result.MinorUnits,
                Is.EqualTo(7794));
        }

        [Test] public void Money_RejectsNegativeMultiplier() =>
            Assert.Throws<
                System.ArgumentOutOfRangeException>(
                () => EconomyTestFactory.EurMoney(100)
                    .Multiply(-1));

        [Test] public void Money_AddRejectsCurrencyMismatch() =>
            Assert.Throws<
                System.InvalidOperationException>(
                () => EconomyTestFactory.EurMoney(100)
                    .Add(
                        new Money(
                            100,
                            new CurrencyCode("USD"))));

        [Test] public void Money_CompareRejectsCurrencyMismatch() =>
            Assert.Throws<
                System.InvalidOperationException>(
                () => EconomyTestFactory.EurMoney(100)
                    .CompareTo(
                        new Money(
                            100,
                            new CurrencyCode("USD"))));

        [Test] public void Money_EqualityIncludesCurrency()
        {
            Assert.That(
                EconomyTestFactory.EurMoney(100) ==
                new Money(
                    100,
                    new CurrencyCode("USD")),
                Is.False);
        }

        [Test] public void Money_ComparisonOperatorsWork()
        {
            Assert.That(
                EconomyTestFactory.EurMoney(101) >
                EconomyTestFactory.EurMoney(100),
                Is.True);
        }

        [Test] public void Money_PositiveAndNegativeFlagsWork()
        {
            Assert.That(
                EconomyTestFactory.EurMoney(1)
                    .IsPositive,
                Is.True);
            Assert.That(
                EconomyTestFactory.EurMoney(-1)
                    .IsNegative,
                Is.True);
        }

        [Test] public void Money_ToStringContainsCurrency()
        {
            Assert.That(
                EconomyTestFactory.EurMoney(150)
                    .ToString(),
                Does.Contain("EUR"));
        }
    }
}
