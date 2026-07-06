using UnityEngine;

namespace VRMGames.CartridgeAndCloud.Presentation.Characters
{
    [DisallowMultipleComponent]
    public sealed class CharacterLocomotionAnimator : MonoBehaviour
    {
        private static readonly int MoveSpeedHash = Animator.StringToHash("MoveSpeed");
        private static readonly int IsStoppedHash = Animator.StringToHash("IsStopped");
        private static readonly int IsWalkingHash = Animator.StringToHash("IsWalking");
        private static readonly int MovementInputPressedHash = Animator.StringToHash("MovementInputPressed");
        private static readonly int MovementInputHeldHash = Animator.StringToHash("MovementInputHeld");
        private static readonly int CurrentGaitHash = Animator.StringToHash("CurrentGait");

        [SerializeField] private Animator _animator;
        [SerializeField, Min(0.001f)] private float _movementThreshold = 0.025f;
        [SerializeField, Min(0f)] private float _speedDamping = 0.08f;

        private Vector3 _previousPosition;
        private bool _initialized;
        private Transform _animatedRoot;
        private Vector3 _animatedRootLocalPosition;
        private Quaternion _animatedRootLocalRotation;
        private bool _animatedRootPoseCaptured;

        public float CurrentSpeed { get; private set; }

        private void Awake()
        {
            ResolveAnimator();
            CaptureAnimatedRootPose();
            _previousPosition = transform.position;
            _initialized = true;
        }

        private void OnEnable()
        {
            ResolveAnimator();
            CaptureAnimatedRootPose();
            _previousPosition = transform.position;
            _initialized = true;
        }

        private void Update()
        {
            if (!_initialized)
            {
                _previousPosition = transform.position;
                _initialized = true;
            }

            float deltaTime = Time.deltaTime;
            if (deltaTime <= 0f)
            {
                return;
            }

            Vector3 delta = transform.position - _previousPosition;
            delta.y = 0f;
            _previousPosition = transform.position;
            CurrentSpeed = delta.magnitude / deltaTime;
            Apply(CurrentSpeed);
        }

        public void Configure(Animator animator)
        {
            _animator = animator;
            ResolveAnimator();
            CaptureAnimatedRootPose();
        }

        public void SetSpeedImmediately(float speed)
        {
            CurrentSpeed = Mathf.Max(0f, speed);
            Apply(CurrentSpeed, true);
        }


        private void LateUpdate()
        {
            if (!_animatedRootPoseCaptured || _animatedRoot == null)
            {
                return;
            }

            // Some imported humanoid clips contain translation/rotation curves on
            // the model root even when Animator.applyRootMotion is disabled.
            // Keep the wrapper responsible for world movement and prevent the
            // visual model from floating or drifting away from it.
            _animatedRoot.localPosition = _animatedRootLocalPosition;
            _animatedRoot.localRotation = _animatedRootLocalRotation;
        }

        private void CaptureAnimatedRootPose()
        {
            if (_animator == null)
            {
                _animatedRoot = null;
                _animatedRootPoseCaptured = false;
                return;
            }

            _animatedRoot = _animator.transform;
            _animatedRootLocalPosition = _animatedRoot.localPosition;
            _animatedRootLocalRotation = _animatedRoot.localRotation;
            _animatedRootPoseCaptured = true;
        }

        private void Apply(float speed, bool immediate = false)
        {
            ResolveAnimator();
            if (_animator == null)
            {
                return;
            }

            bool moving = speed > _movementThreshold;
            SetFloatIfPresent(MoveSpeedHash, speed, immediate ? 0f : _speedDamping);
            SetBoolIfPresent(IsStoppedHash, !moving);
            SetBoolIfPresent(IsWalkingHash, moving);
            SetBoolIfPresent(MovementInputPressedHash, moving);
            SetBoolIfPresent(MovementInputHeldHash, moving);
            SetIntIfPresent(CurrentGaitHash, moving ? 1 : 0);
        }

        private void ResolveAnimator()
        {
            if (_animator == null)
            {
                _animator = GetComponentInChildren<Animator>(true);
            }

            if (_animator != null)
            {
                _animator.applyRootMotion = false;
                _animator.cullingMode = AnimatorCullingMode.AlwaysAnimate;
            }
        }

        private bool HasParameter(int hash, AnimatorControllerParameterType type)
        {
            if (_animator == null)
            {
                return false;
            }

            foreach (AnimatorControllerParameter parameter in _animator.parameters)
            {
                if (parameter.nameHash == hash && parameter.type == type)
                {
                    return true;
                }
            }

            return false;
        }

        private void SetBoolIfPresent(int hash, bool value)
        {
            if (HasParameter(hash, AnimatorControllerParameterType.Bool))
            {
                _animator.SetBool(hash, value);
            }
        }

        private void SetIntIfPresent(int hash, int value)
        {
            if (HasParameter(hash, AnimatorControllerParameterType.Int))
            {
                _animator.SetInteger(hash, value);
            }
        }

        private void SetFloatIfPresent(int hash, float value, float damping)
        {
            if (!HasParameter(hash, AnimatorControllerParameterType.Float))
            {
                return;
            }

            if (damping > 0f)
            {
                _animator.SetFloat(hash, value, damping, Time.deltaTime);
            }
            else
            {
                _animator.SetFloat(hash, value);
            }
        }
    }
}
