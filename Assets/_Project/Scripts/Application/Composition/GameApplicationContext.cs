using System;
using VRMGames.CartridgeAndCloud.Application.DayCycle;
using VRMGames.CartridgeAndCloud.Application.GameSession;
using VRMGames.CartridgeAndCloud.Application.Persistence;

namespace VRMGames.CartridgeAndCloud.Application.Composition
{
    public interface IGameApplicationContext
    {
        IActiveGameSession ActiveSession { get; }

        ISaveMutationRegistry SaveMutations { get; }

        ISimulationClock SimulationClock { get; }

        IPauseService PauseService { get; }

        IUtcClock UtcClock { get; }
    }

    public sealed class GameApplicationContext :
        IGameApplicationContext
    {
        public IActiveGameSession ActiveSession { get; }

        public ISaveMutationRegistry SaveMutations { get; }

        public ISimulationClock SimulationClock { get; }

        public IPauseService PauseService { get; }

        public IUtcClock UtcClock { get; }

        public GameApplicationContext(
            IActiveGameSession activeSession,
            ISaveMutationRegistry saveMutations,
            ISimulationClock simulationClock,
            IPauseService pauseService,
            IUtcClock utcClock)
        {
            ActiveSession = activeSession ??
                throw new ArgumentNullException(
                    nameof(activeSession));
            SaveMutations = saveMutations ??
                throw new ArgumentNullException(
                    nameof(saveMutations));
            SimulationClock = simulationClock ??
                throw new ArgumentNullException(
                    nameof(simulationClock));
            PauseService = pauseService ??
                throw new ArgumentNullException(
                    nameof(pauseService));
            UtcClock = utcClock ??
                throw new ArgumentNullException(
                    nameof(utcClock));
        }
    }
}
