using System;
using System.Collections.Generic;
using VRMGames.CartridgeAndCloud.Domain.Identifiers;
using VRMGames.CartridgeAndCloud.Domain.VerticalSlicePhase1;

namespace VRMGames.CartridgeAndCloud.Application.VerticalSlicePhase1
{
    public interface IPhase1Catalog
    {
        IReadOnlyList<Phase1FurnitureDefinition>
            Furniture { get; }

        IReadOnlyList<Phase1ProductDefinition>
            Products { get; }

        bool TryGetFurniture(
            string definitionId,
            out Phase1FurnitureDefinition definition);

        bool TryGetProduct(
            string productId,
            out Phase1ProductDefinition definition);
    }

    public interface IPhase1StateRepository
    {
        Phase1StoreState Load(
            SaveSlotId slotId);

        void Save(
            Phase1StoreState state);

        bool Delete(
            SaveSlotId slotId);
    }

    public interface IPhase1FeedbackSink
    {
        void Publish(
            Phase1FeedbackEvent feedbackEvent);
    }

    public interface IPhase1AudioRouter
    {
        void Play(
            string eventId);

        void SetChannelVolume(
            Phase1AudioChannel channel,
            float normalizedVolume);

        float GetChannelVolume(
            Phase1AudioChannel channel);
    }

    public interface IPhase1UtcClock
    {
        DateTime UtcNow { get; }
    }
}
