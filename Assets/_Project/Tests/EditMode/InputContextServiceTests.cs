using NUnit.Framework;
using VRMGames.CartridgeAndCloud.Application.InputContexts;

namespace VRMGames.CartridgeAndCloud.Tests.EditMode
{
    public sealed class InputContextServiceTests
    {
        [Test]
        public void NewService_StartsWithNoneContext()
        {
            InputContextService service = new InputContextService();

            Assert.That(
                service.CurrentContext,
                Is.EqualTo(InputContextId.None));
        }

        [Test]
        public void SetContext_ToDifferentContext_ChangesAndRaisesEvent()
        {
            InputContextService service = new InputContextService();
            InputContextId observed = InputContextId.None;
            int eventCount = 0;

            service.ContextChanged += context =>
            {
                observed = context;
                eventCount++;
            };

            InputContextChangeResult result =
                service.SetContext(InputContextId.UI);

            Assert.That(
                result,
                Is.EqualTo(InputContextChangeResult.Changed));
            Assert.That(
                service.CurrentContext,
                Is.EqualTo(InputContextId.UI));
            Assert.That(observed, Is.EqualTo(InputContextId.UI));
            Assert.That(eventCount, Is.EqualTo(1));
        }

        [Test]
        public void SetContext_ToCurrentContext_DoesNotRaiseEvent()
        {
            InputContextService service = new InputContextService();
            int eventCount = 0;

            service.ContextChanged += _ => eventCount++;

            service.SetContext(InputContextId.Gameplay);
            InputContextChangeResult result =
                service.SetContext(InputContextId.Gameplay);

            Assert.That(
                result,
                Is.EqualTo(InputContextChangeResult.AlreadyActive));
            Assert.That(eventCount, Is.EqualTo(1));
        }

        [Test]
        public void SetContext_ToUndefinedValue_IsRejected()
        {
            InputContextService service = new InputContextService();

            InputContextChangeResult result =
                service.SetContext((InputContextId)99);

            Assert.That(
                result,
                Is.EqualTo(InputContextChangeResult.InvalidContext));
            Assert.That(
                service.CurrentContext,
                Is.EqualTo(InputContextId.None));
        }
    }
}
