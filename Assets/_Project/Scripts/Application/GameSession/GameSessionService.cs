using System;
using GameSessionSnapshot = VRMGames.CartridgeAndCloud.Domain.GameSession.GameSessionSnapshot;
using DomainGameSession = VRMGames.CartridgeAndCloud.Domain.GameSession.GameSession;
using VRMGames.CartridgeAndCloud.Domain.Identifiers;

namespace VRMGames.CartridgeAndCloud.Application.GameSession
{
    public sealed class GameSessionService : IGameSessionService
    {
        private readonly ISaveGameRepository _repository;
        private readonly IUtcClock _clock;

        public bool HasActiveSession => Current != null;

        public DomainGameSession Current { get; private set; }

        public GameSessionService(
            ISaveGameRepository repository,
            IUtcClock clock)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _clock = clock ?? throw new ArgumentNullException(nameof(clock));
        }

        public void StartNew(SaveSlotId slotId)
        {
            Current = DomainGameSession.CreateNew(
                slotId,
                StableId.New(),
                _clock.UtcNow);
        }

        public GameSessionOperationResult SaveCurrent()
        {
            if (Current == null)
            {
                return GameSessionOperationResult.NoActiveSession;
            }

            try
            {
                _repository.Save(Current.CaptureSnapshot(_clock.UtcNow));
                return GameSessionOperationResult.Success;
            }
            catch
            {
                return GameSessionOperationResult.StorageFailure;
            }
        }

        public GameSessionOperationResult Load(SaveSlotId slotId)
        {
            try
            {
                if (!_repository.TryLoad(slotId, out GameSessionSnapshot snapshot))
                {
                    return GameSessionOperationResult.SlotEmpty;
                }

                Current = DomainGameSession.Restore(snapshot);
                return GameSessionOperationResult.Success;
            }
            catch
            {
                return GameSessionOperationResult.StorageFailure;
            }
        }

        public GameSessionOperationResult Delete(SaveSlotId slotId)
        {
            try
            {
                bool deleted = _repository.Delete(slotId);

                if (Current != null && Current.SlotId == slotId)
                {
                    Current = null;
                }

                return deleted
                    ? GameSessionOperationResult.Success
                    : GameSessionOperationResult.SlotEmpty;
            }
            catch
            {
                return GameSessionOperationResult.StorageFailure;
            }
        }

        public bool SlotExists(SaveSlotId slotId)
        {
            try
            {
                return _repository.Exists(slotId);
            }
            catch
            {
                return false;
            }
        }
    }
}
