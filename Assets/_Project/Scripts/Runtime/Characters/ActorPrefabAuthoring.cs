using System;
using UnityEngine;
using UnityEngine.AI;
using VRMGames.CartridgeAndCloud.Domain.Characters;

namespace VRMGames.CartridgeAndCloud.Runtime.Characters
{
    /// <summary>
    /// Serialized physical and locomotion profile owned by an actor wrapper prefab.
    /// </summary>
    [DisallowMultipleComponent]
    [RequireComponent(typeof(CapsuleCollider), typeof(NavMeshAgent))]
    public sealed class ActorPrefabAuthoring : MonoBehaviour
    {
        [SerializeField]
        private CharacterRole _role;

        [SerializeField, Min(0.05f)]
        private float _agentRadius = 0.32f;

        [SerializeField, Min(0.2f)]
        private float _agentHeight = 1.8f;

        [SerializeField, Min(0.1f)]
        private float _moveSpeed = 2f;

        [SerializeField, Min(0f)]
        private float _stoppingDistance = 0.12f;

        [SerializeField, Range(0, 99)]
        private int _avoidancePriority = 50;

        public CharacterRole Role => _role;

        public void Configure(CharacterRole role)
        {
            _role = role;
            ConfigureAgent();
        }

        public NavMeshAgent ConfigureAgent()
        {
            CapsuleCollider capsule = GetComponent<CapsuleCollider>();
            capsule.isTrigger = true;
            capsule.radius = _agentRadius;
            capsule.height = _agentHeight;
            capsule.center = Vector3.up * (_agentHeight * 0.5f);

            NavMeshAgent agent = GetComponent<NavMeshAgent>();
            if (agent == null)
            {
                throw new InvalidOperationException(
                    "Actor prefab requires a serialized NavMeshAgent.");
            }

            agent.radius = _agentRadius;
            agent.height = _agentHeight;
            agent.baseOffset = 0f;
            agent.speed = _moveSpeed;
            agent.angularSpeed = 540f;
            agent.acceleration = 12f;
            agent.stoppingDistance = _stoppingDistance;
            agent.obstacleAvoidanceType =
                ObstacleAvoidanceType.HighQualityObstacleAvoidance;
            agent.avoidancePriority = _avoidancePriority;
            return agent;
        }
    }
}
