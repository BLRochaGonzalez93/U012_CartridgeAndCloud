using System;
using UnityEngine;
using VRMGames.CartridgeAndCloud.Domain.Displays;

namespace VRMGames.CartridgeAndCloud.Infrastructure.Displays
{
    public sealed class DisplayRuntimeAuthoring : MonoBehaviour
    {
        [SerializeField]
        private string _displayInstanceId;

        [SerializeField]
        private DisplayDefinitionAsset _definition;

        public string DisplayInstanceId => _displayInstanceId;

        public DisplayDefinitionAsset Definition => _definition;

        public DisplayInstance BuildInstance()
        {
            if (_definition == null)
            {
                throw new InvalidOperationException(
                    "Display runtime authoring requires a definition asset.");
            }

            return new DisplayInstance(
                new DisplayInstanceId(_displayInstanceId),
                _definition.BuildDefinition());
        }

        public void Configure(
            string displayInstanceId,
            DisplayDefinitionAsset definition)
        {
            _displayInstanceId = displayInstanceId;
            _definition = definition;
        }
    }
}
