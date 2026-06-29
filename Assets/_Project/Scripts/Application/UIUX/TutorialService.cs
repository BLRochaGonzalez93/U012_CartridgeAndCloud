using System;
using System.Collections.Generic;
using VRMGames.CartridgeAndCloud.Domain.Identifiers;
using VRMGames.CartridgeAndCloud.Domain.UIUX;

namespace VRMGames.CartridgeAndCloud.Application.UIUX
{
    public sealed class TutorialService
    {
        private readonly ITutorialProgressRepository
            _repository;

        private readonly Dictionary<
            TutorialStepId,
            TutorialBubble> _bubbles;

        public TutorialService(
            ITutorialProgressRepository repository)
        {
            _repository = repository ??
                throw new ArgumentNullException(
                    nameof(repository));
            _bubbles = CreateDefaultBubbles();
        }

        public TutorialProgress GetProgress(
            SaveSlotId slotId)
        {
            return _repository.Load(slotId) ??
                TutorialProgress.New();
        }

        public TutorialBubble GetBubble(
            TutorialProgress progress)
        {
            if (progress == null)
            {
                throw new ArgumentNullException(
                    nameof(progress));
            }

            return _bubbles.TryGetValue(
                progress.CurrentStep,
                out TutorialBubble bubble)
                ? bubble
                : null;
        }

        public TutorialProgress Start(
            SaveSlotId slotId)
        {
            TutorialProgress progress =
                GetProgress(slotId).Start();
            Save(slotId, progress);
            return progress;
        }

        public TutorialProgress Advance(
            SaveSlotId slotId)
        {
            TutorialProgress progress =
                GetProgress(slotId);

            if (progress.State ==
                TutorialProgressState.NotStarted)
            {
                progress = progress.Start();
            }

            progress = progress.Advance();
            Save(slotId, progress);
            return progress;
        }

        public TutorialProgress Skip(
            SaveSlotId slotId)
        {
            TutorialProgress progress =
                GetProgress(slotId).Skip();
            Save(slotId, progress);
            return progress;
        }

        public TutorialProgress Restart(
            SaveSlotId slotId)
        {
            TutorialProgress progress =
                GetProgress(slotId).Restart();
            Save(slotId, progress);
            return progress;
        }

        public bool Delete(SaveSlotId slotId)
        {
            return _repository.Delete(slotId);
        }

        private void Save(
            SaveSlotId slotId,
            TutorialProgress progress)
        {
            _repository.Save(slotId, progress);
        }

        private static Dictionary<
            TutorialStepId,
            TutorialBubble> CreateDefaultBubbles()
        {
            return new Dictionary<
                TutorialStepId,
                TutorialBubble>
            {
                {
                    TutorialStepId.Welcome,
                    Bubble(
                        TutorialStepId.Welcome,
                        "Welcome to Cartridge & Cloud",
                        "Run the store one day at a time. " +
                        "The HUD keeps the important state visible.",
                        "Press Next to continue.",
                        "hud-root")
                },
                {
                    TutorialStepId.MovementAndCamera,
                    Bubble(
                        TutorialStepId.MovementAndCamera,
                        "Movement and camera",
                        "Use the existing gameplay controls to move, " +
                        "orbit and zoom around the store.",
                        "Try the camera, then press Next.",
                        "store-view")
                },
                {
                    TutorialStepId.OpenManagement,
                    Bubble(
                        TutorialStepId.OpenManagement,
                        "Management panels",
                        "Open a panel from the left navigation. " +
                        "Gameplay input is blocked while a panel is open.",
                        "Open Inventory.",
                        "management-navigation")
                },
                {
                    TutorialStepId.Inventory,
                    Bubble(
                        TutorialStepId.Inventory,
                        "Inventory",
                        "Review total stock, capacity and product locations.",
                        "Check the Inventory panel.",
                        "panel-inventory")
                },
                {
                    TutorialStepId.Suppliers,
                    Bubble(
                        TutorialStepId.Suppliers,
                        "Suppliers",
                        "Review placed orders, received units and costs.",
                        "Open Suppliers.",
                        "panel-suppliers")
                },
                {
                    TutorialStepId.Displays,
                    Bubble(
                        TutorialStepId.Displays,
                        "Displays",
                        "Displays show assigned products and available stock.",
                        "Open Displays.",
                        "panel-displays")
                },
                {
                    TutorialStepId.CustomersAndShopping,
                    Bubble(
                        TutorialStepId.CustomersAndShopping,
                        "Customers and shopping",
                        "Track active customers, sessions, carts and reservations.",
                        "Open Customers or Shopping.",
                        "panel-customers")
                },
                {
                    TutorialStepId.QueueAndCheckout,
                    Bubble(
                        TutorialStepId.QueueAndCheckout,
                        "Queue and checkout",
                        "Monitor the FIFO queue, station state and transactions.",
                        "Open Checkout.",
                        "panel-checkout")
                },
                {
                    TutorialStepId.DayCycle,
                    Bubble(
                        TutorialStepId.DayCycle,
                        "Day cycle",
                        "A day progresses from Before Open to Closed. " +
                        "Autosave only runs after a valid Closed state.",
                        "Open Day Cycle.",
                        "panel-day-cycle")
                },
                {
                    TutorialStepId.DailyResults,
                    Bubble(
                        TutorialStepId.DailyResults,
                        "Daily results",
                        "Review revenue, received supplier costs and gross result.",
                        "Open Economy.",
                        "panel-economy")
                },
                {
                    TutorialStepId.AutosaveComplete,
                    Bubble(
                        TutorialStepId.AutosaveComplete,
                        "Autosave",
                        "The active slot is saved once after each closed day. " +
                        "The previous valid generation remains as backup.",
                        "Press Finish.",
                        "save-status")
                }
            };
        }

        private static TutorialBubble Bubble(
            TutorialStepId step,
            string title,
            string body,
            string action,
            string anchor)
        {
            return new TutorialBubble(
                step,
                title,
                body,
                action,
                anchor);
        }
    }
}
