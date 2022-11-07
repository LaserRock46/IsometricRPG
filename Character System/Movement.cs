using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

namespace Project.CharacterSystem
{
    [System.Serializable]
    public class Movement : MonoBehaviour
    {
        #region Temp
        [Header("Temporary Things", order = 0)]
        private Quaternion _turnInPlaceTarget;
        private Quaternion _turnInPlaceStart;   
        #endregion

        #region Fields
        [System.Serializable]
        public struct PersistentData
        {
            public Vector3 targetDirectionAndGear;
            public Vector3 directionAndMoveGear;
            public Quaternion currentRotation;
            public MoveState moveState;
            public float fallVelocity;
            public Vector3 fallingStartAtHeight;

        }
        [Header("Fields", order = 1)]
        [SerializeField] private Character _character;

        [HorizontalLine]

        [SerializeField][ReadOnly] private PersistentData _persistentData;
        public PersistentData MovementPersistentData { get => _persistentData; set => _persistentData = value; }
     
        [HorizontalLine]
        [Header("Move State")]

        [ReadOnly][SerializeField] private Vector3 _moveVelocity;
        public Vector3 MoveVelocity { get => _moveVelocity; set => _moveVelocity = value; }
        public enum MoveState { Idle, TurnInPlace, Walk, Run, Break, Dodge, SingleStep, Hit, Fall, Push, Ladder, MoveTo }
        public MoveState CurrentMoveState { get => _persistentData.moveState; private set => _persistentData.moveState = value; }
        public Vector3 TargetDirectionAndGear { get => _persistentData.targetDirectionAndGear; set => _persistentData.targetDirectionAndGear = value; }
        public Vector3 DirectionAndMoveGear { get => _persistentData.directionAndMoveGear; set => _persistentData.directionAndMoveGear = value; }

        [SerializeField] private float _gearTransitionAcceleration;
        [SerializeField] private float _gearTransitionDeceleration;

        [SerializeField] private float _stopWalkThreshold;
        [SerializeField] private float _singleStepThreshold;
        [SerializeField] private float _moveConstraintToStartThreshold;
        [SerializeField] private float _moveConstraintToStopThreshold;
        private Vector3 _singleStepDirection;
        private float _inputMoveTime;

        [HorizontalLine]

        [SerializeField] private float _maxSafeMoveMagnitude = 10;
        [SerializeField][ReadOnly] private float _previousMoveMagnitude;


        [HorizontalLine]
        [Header("Turn Idle In Place")]

        [SerializeField] private float _minAngleToTriggerTurnInPlace;       
        [SerializeField] private float _targetFocusTime;
        private float _currentFocusTime;
        [SerializeField] private float _focusSpan;
        private Vector3 _previousFocusPoint;   
        private float _inPlaceTurnAmountDegrees;
        public float InPlaceTurnAmountDegrees { get => _inPlaceTurnAmountDegrees; private set => _inPlaceTurnAmountDegrees = value; }

        [HorizontalLine]
        [Header("Turning While Moving")]

        [SerializeField] private AnimationCurve _turningSpeedOverMoveMagnitude;
        [SerializeField] private float _turningSpeedMultiplier;
     
        public Quaternion CurrentRotation { get => _persistentData.currentRotation; set => _persistentData.currentRotation = value; }

        private Quaternion _lookAtRotation;
        public Quaternion LookAtRotation { get => _lookAtRotation; private set => _lookAtRotation = value; }
      
        [HorizontalLine]
        [Header("Falling")]

        [SerializeField] private float _triggerFallingBelowHeight;
        [SerializeField] private float _triggerRagdollAfterFallDistance;
        [SerializeField] private float _highAltitudeFallModeThreshold;   
        [SerializeField] private float _defaultMoveGravity;
        [SerializeField] private Utilities.LayerMaskSO _groundOrWalkable;      
        public enum FallMode {FitFallMode, LowAltitude, HighAltitude}
        [SerializeField] [ReadOnly] FallMode _fallMode;
        public float FallVelocity { get => _persistentData.fallVelocity; set => _persistentData.fallVelocity = value; }
        public Vector3 FallingStartAtHeight { get => _persistentData.fallingStartAtHeight; set => _persistentData.fallingStartAtHeight = value; }


        public Vector3 InputMove { get; set; }
        public bool InputRun { get; set; }
        public Vector3 InputCharacterLookAtPoint { get; set; }

        #endregion

        #region Functions       
        float GetAngleBetweenCharacterAndLookForward()
        {
            return Vector3.SignedAngle(LookAtRotation * Vector3.forward, _character.CharacterRoot.rotation * Vector3.forward, Vector3.up) * -1;
        }
        float GetFallDistance()
        {          
            return Vector3.Distance(_character.CharacterRoot.position, FallingStartAtHeight);          
        }
        float GetDistanceToGround()
        {
            Physics.SphereCast(_character.CharacterRoot.position, _character.CharacterController.radius, Vector3.down, out RaycastHit groundHit, Mathf.Infinity, _groundOrWalkable.LayerMask, QueryTriggerInteraction.Ignore);
            return groundHit.distance;
        }
        #endregion

        #region Methods
        void Start()
        {
            FallingStartAtHeight = _character.CharacterRoot.position;
            FallVelocity = _defaultMoveGravity;
          
        }
        void Update()
        {
            LookAtPointerUpdate();        
            MotionDirectionUpdate();
            UnusualStatesTriggerUpdate();
        }
        private void OnEnable()
        {
            _character.AnimatorCallbacks.OnAnimatorMoveCallback += MoveStateUpdate;

        }
        private void OnDisable()
        {
            _character.AnimatorCallbacks.OnAnimatorMoveCallback -= MoveStateUpdate;

        }
        void LookAtPointerUpdate()
        {
            Vector3 direction = (InputCharacterLookAtPoint - _character.CharacterRoot.position).normalized;
            direction.y = 0;
            LookAtRotation = Quaternion.LookRotation(direction);

        }     
        void RootMotionUpdate(bool updateRotation, bool updateMove = true)
        {
            _moveVelocity = _character.CharacterAnimator.Animator.deltaPosition;
            _moveVelocity.y = FallVelocity;

           
            if (updateMove)
            {
                if (Mathf.Abs(_character.CharacterAnimator.Animator.deltaPosition.magnitude - _previousMoveMagnitude) < _maxSafeMoveMagnitude)
                {                 
                    _previousMoveMagnitude = _character.CharacterAnimator.Animator.deltaPosition.magnitude;
                    _character.CharacterController.Move(_moveVelocity);
                }
            }
           
            if (updateRotation)
            {
                _character.CharacterRoot.rotation = _character.CharacterRoot.rotation * _character.CharacterAnimator.Animator.deltaRotation;
            }
        }
        void MotionDirectionUpdate()
        {
            //if (CurrentMoveState == MoveState.Walk || CurrentMoveState == MoveState.Run)
            {
                if (TargetDirectionAndGear != DirectionAndMoveGear)
                {
                    float speed = TargetDirectionAndGear == Vector3.zero ? _gearTransitionDeceleration : _gearTransitionAcceleration;
                    DirectionAndMoveGear = Vector3.MoveTowards(DirectionAndMoveGear,TargetDirectionAndGear,Time.deltaTime * speed);

                    _character.CharacterAnimator.SetMove(DirectionAndMoveGear.z, DirectionAndMoveGear.x);
                }
            }
        }
        void ChangeInstantlyMotionDirection(Vector3 targetDirectionAndGear, float delay = 0.1f)
        {
            StartCoroutine(Delay());
            IEnumerator Delay()
            {
                yield return new WaitForSeconds(delay);
                TargetDirectionAndGear = targetDirectionAndGear;
                DirectionAndMoveGear = targetDirectionAndGear;
                _character.CharacterAnimator.SetMove(DirectionAndMoveGear.z, DirectionAndMoveGear.x);
            }
        }
        void ChangeMotionDirection(Vector3 inputMove)
        {
            _character.MovementConstraint.MoveConstraintUpdate();
            TargetDirectionAndGear = inputMove.normalized * Mathf.Clamp(inputMove.magnitude, 0, _character.MovementConstraint.MovementConstrainedToDistance);
        }
        void MoveStateUpdate()
        {
            switch (CurrentMoveState)
            {
                case MoveState.Idle:
                    IdleState();               
                    break;
                case MoveState.TurnInPlace:
                    TurnInPlaceState();
                    break;
                case MoveState.Walk:
                    WalkState();
                    break;
                case MoveState.Run:
                    RunState();               
                    break;
                case MoveState.Break:
                    break;
                case MoveState.Dodge:
                    break;
                case MoveState.SingleStep:
                    SingleStepState();                  
                    break;
                case MoveState.Hit:
                    break;
                case MoveState.Fall:
                    FallState();           
                    break;
            }          
            void IdleState()
            {
                _character.MovementConstraint.MoveConstraintUpdate();

                if (_character.CharacterAnimator.ToArmed.StateActive == false && _character.CharacterAnimator.ToUnarmed.StateActive == false)
                {
                    if (_character.MovementConstraint.MovementConstrainedToDistance > _moveConstraintToStartThreshold)
                    {
                        if (InputMove != Vector3.zero)
                        {
                            _inputMoveTime += Time.deltaTime;
                            _singleStepDirection = InputMove;
                        }

                        if (InputMove != Vector3.zero && _inputMoveTime >= _singleStepThreshold)
                        {
                            CurrentMoveState = MoveState.Walk;
                            _inputMoveTime = 0;
                        }
                        else if (InputMove == Vector3.zero && _inputMoveTime > 0.001f && _inputMoveTime <= _singleStepThreshold)
                        {
                            CurrentMoveState = MoveState.SingleStep;
                            _inputMoveTime = 0;
                        }
                    }
                    if (Mathf.Abs(GetAngleBetweenCharacterAndLookForward()) > _minAngleToTriggerTurnInPlace)
                    {
                        CurrentMoveState = MoveState.TurnInPlace;
                        _inputMoveTime = 0;
                    }
                    RootMotionUpdate(true, DirectionAndMoveGear != Vector3.zero);
                }
                else
                {              
                    RootMotionUpdate(true, true);
                }

            }
            void TurnInPlaceState()
            {
                bool CanPerformTurnInPlace()
                {
                    if (_character.CharacterAnimator.TurnInPlaceArmed.WillBeActiveSoon) return false;
                    if (_character.CharacterAnimator.TurnInPlaceUnarmed.WillBeActiveSoon) return false;
                    if (_character.CharacterAnimator.TurnInPlaceArmed.ExitFlag) return false;
                    if (_character.CharacterAnimator.TurnInPlaceUnarmed.ExitFlag) return false;

                    if (Mathf.Abs(GetAngleBetweenCharacterAndLookForward()) < _minAngleToTriggerTurnInPlace) return false;

                    return true;
                }
                bool CanFinishTurnInPlace()
                {
                    if (_character.CharacterAnimator.TurnInPlaceArmed.ExitFlag) return true;
                    if (_character.CharacterAnimator.TurnInPlaceUnarmed.ExitFlag) return true;

                    return false;
                }

                if (CanPerformTurnInPlace())
                {
                    if (Vector3.Distance(_previousFocusPoint, InputCharacterLookAtPoint) > _focusSpan)
                    {
                        _previousFocusPoint = InputCharacterLookAtPoint;
                        _currentFocusTime = 0;
                    }
                    else
                    {
                        _currentFocusTime += Time.deltaTime;
                    }

                    if (_currentFocusTime > _targetFocusTime)
                    {
                        _currentFocusTime = 0;
                        PerformTurn();
                    }
                }
                else if (CanFinishTurnInPlace())
                {
                    _character.CharacterAnimator.TurnInPlaceArmed.ExitFlag = false;
                    _character.CharacterAnimator.TurnInPlaceUnarmed.ExitFlag = false;
                    _inPlaceTurnAmountDegrees = 0;

                    CurrentMoveState = MoveState.Idle;
                }

                if (InputMove != Vector3.zero)
                {
                    _character.MovementConstraint.MoveConstraintUpdate();
                    if (_character.MovementConstraint.MovementConstrainedToDistance > _moveConstraintToStartThreshold)
                    {
                        _character.CharacterAnimator.TurnInPlaceArmed.WillBeActiveSoon = false;
                        _character.CharacterAnimator.TurnInPlaceUnarmed.WillBeActiveSoon = false;
                        _inPlaceTurnAmountDegrees = 0;

                        CurrentMoveState = MoveState.Walk;
                    }
                }

                void PerformTurn()
                {
                    if (_character.CharacterAnimator.ArmedIdle.StateActive)
                    {
                        _character.CharacterAnimator.TurnInPlaceArmed.WillBeActiveSoon = true;
                    }
                    else if (_character.CharacterAnimator.UnarmedIdle.StateActive)
                    {
                        _character.CharacterAnimator.TurnInPlaceUnarmed.WillBeActiveSoon = true;
                    }

                    _inPlaceTurnAmountDegrees = GetAngleBetweenCharacterAndLookForward();
                    _character.CharacterAnimator.SetTurnAmount(_inPlaceTurnAmountDegrees);
                    _character.CharacterAnimator.SetTurnInPlace();

                    _turnInPlaceStart = _character.CharacterRoot.rotation;
                    _turnInPlaceTarget = Quaternion.Euler(0, _character.CharacterRoot.eulerAngles.y + GetAngleBetweenCharacterAndLookForward(), 0);
                }

                RootMotionUpdate(true, true);
            }
            void WalkState()
            {
                ChangeMotionDirection(InputMove);

                if (CurrentMoveState == MoveState.Walk)
                {
                    if (_character.CharacterAnimator.MoveArmed.StateActive == false && _character.CharacterAnimator.MoveUnarmed.StateActive == false)
                    {
                        _character.CharacterAnimator.SetMoving(true);
                        
                    }
                }

                if (_character.CharacterAnimator.MoveArmed.StateActive || _character.CharacterAnimator.MoveUnarmed.StateActive)
                {
                    if (TargetDirectionAndGear == Vector3.zero && DirectionAndMoveGear.magnitude < _stopWalkThreshold)
                    {
                        ChangeInstantlyMotionDirection(Vector3.zero, 0.45f);
                        _character.CharacterAnimator.SetMoving(false);
                        CurrentMoveState = MoveState.Idle;
                    }
                    if (_character.MovementConstraint.MovementConstrainedToDistance < _moveConstraintToStopThreshold)
                    {                      
                        ChangeInstantlyMotionDirection(Vector3.zero, 1f);
                        _character.CharacterAnimator.SetMoving(false);
                        CurrentMoveState = MoveState.Idle;
                    }
                    if (DirectionAndMoveGear.magnitude >= 1 && InputRun == true)
                    {
                        CurrentMoveState = MoveState.Run;
                    }
                }
                TurningWhileMoving();
                RootMotionUpdate(false, true);
            }
            void RunState()
            {
                ChangeMotionDirection(InputMove * (InputRun == true ? 2 : 1));
                if (DirectionAndMoveGear.magnitude <= 1)
                {
                    CurrentMoveState = MoveState.Walk;
                }
                TurningWhileMoving();
                RootMotionUpdate(false, true);
            }
            void SingleStepState()
            {
                if (_character.CharacterAnimator.SingleStepArmed.ExitFlag == false && _character.CharacterAnimator.SingleStepUnarmed.ExitFlag == false)
                {
                    if (_character.CharacterAnimator.SingleStepArmed.WillBeActiveSoon == false && _character.CharacterAnimator.SingleStepUnarmed.WillBeActiveSoon == false)
                    {
                        if (_character.CharacterAnimator.ArmedIdle.StateActive == true)
                        {
                            _character.CharacterAnimator.SingleStepArmed.WillBeActiveSoon = true;
                        }
                        else if (_character.CharacterAnimator.UnarmedIdle.StateActive == true)
                        {
                            _character.CharacterAnimator.SingleStepUnarmed.WillBeActiveSoon = true;
                        }
                        _character.CharacterAnimator.SetMove(_singleStepDirection.z, _singleStepDirection.x);
                        _character.CharacterAnimator.SetSingleStep();
                    }
                }
                if (_character.CharacterAnimator.SingleStepArmed.ExitFlag == true || _character.CharacterAnimator.SingleStepUnarmed.ExitFlag == true)
                {
                    _character.CharacterAnimator.SingleStepArmed.ExitFlag = false;
                    _character.CharacterAnimator.SingleStepUnarmed.ExitFlag = false;
                    CurrentMoveState = MoveState.Idle;               
                }
                RootMotionUpdate(true, true);
            }
            void FallState() 
            {
                switch (_fallMode)
                {
                    case FallMode.FitFallMode:
                        _character.CharacterAnimator.SetMoving(false);
                        ChangeInstantlyMotionDirection(Vector3.zero);

                        float distanceToGround = GetDistanceToGround();
                        if (distanceToGround < _highAltitudeFallModeThreshold)
                        {
                            _character.CharacterAnimator.SetJumpDown(distanceToGround);
                            _fallMode = FallMode.LowAltitude;                       
                        }
                        else
                        {
                            _fallMode = FallMode.HighAltitude;
                        }
                        break;
                    case FallMode.LowAltitude:
                        if(_character.CharacterAnimator.JumpDownArmed.ExitFlag == true || _character.CharacterAnimator.JumpDownUnarmed.ExitFlag == true)
                        {
                            _character.CharacterAnimator.JumpDownArmed.ExitFlag = false;
                            _character.CharacterAnimator.JumpDownUnarmed.ExitFlag = false;
                            FallingStartAtHeight = _character.CharacterRoot.position;
                            CurrentMoveState = MoveState.Idle;
                            _fallMode = FallMode.FitFallMode;
                            FallVelocity = _defaultMoveGravity;
                        }
                        else
                        {
                            FallVelocity = _character.CharacterAnimator.Animator.deltaPosition.y;
                        }
                        break;
                    case FallMode.HighAltitude:
                        if (_character.CharacterController.isGrounded == false)
                        {
                            if (GetFallDistance() > _triggerRagdollAfterFallDistance && _character.RagdollController.RagdollState == RagdollState.Disabled)
                            {
                                _character.RagdollController.Ragdolled(true);
                            }
                        }
                        if (_character.CharacterController.isGrounded == true)
                        {
                            if (_character.CharacterAnimator.StandUpFaceDown.ExitFlag || _character.CharacterAnimator.StandUpFaceUp.ExitFlag)
                            {
                                _character.CharacterAnimator.StandUpFaceDown.ExitFlag = false;
                                _character.CharacterAnimator.StandUpFaceUp.ExitFlag = false;
                                CurrentMoveState = MoveState.Idle;
                                _fallMode = FallMode.FitFallMode;
                                FallVelocity = _defaultMoveGravity;
                            }
                        }
                        break;
                }
                RootMotionUpdate(true, true);
            }
          
            void TurningWhileMoving()
            {              
                if (LookAtRotation != CurrentRotation)
                {
                    float turningSpeed = _turningSpeedOverMoveMagnitude.Evaluate(DirectionAndMoveGear.magnitude) * _turningSpeedMultiplier * Mathf.Abs(GetAngleBetweenCharacterAndLookForward());
                    CurrentRotation = Quaternion.RotateTowards(_character.CharacterAnimator.Animator.rootRotation, LookAtRotation, turningSpeed * Time.deltaTime).normalized;
                    _character.CharacterRoot.rotation = CurrentRotation;
                }
            }
        }
        void UnusualStatesTriggerUpdate()
        {
            FallTrigger();
            void FallTrigger()
            {
                if (_character.CharacterController.isGrounded == true && CurrentMoveState != MoveState.Fall)
                {
                    FallingStartAtHeight = _character.CharacterRoot.position;
                   
                }
                if (_character.CharacterController.isGrounded == false && CurrentMoveState != MoveState.Fall)
                {                  
                    if (GetFallDistance() > _triggerFallingBelowHeight)
                    {
                        Debug.Log(GetFallDistance());
                        Debug.DrawLine(FallingStartAtHeight, _character.CharacterRoot.position, Color.red, 3);
                        CurrentMoveState = MoveState.Fall;                     
                    }
                }
            }
        }
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = _character.CharacterController.isGrounded ? Color.green : Color.red;
            Gizmos.DrawWireSphere(_character.CharacterRoot.position, _character.CharacterController.radius);

            Gizmos.color = Color.blue;
            Gizmos.DrawSphere(_character.CharacterRoot.position + _character.CharacterRoot.forward * 5, 0.05f);
            Gizmos.color = Color.green;
            Gizmos.DrawSphere(_character.CharacterRoot.position + LookAtRotation * (Vector3.forward * 5), 0.05f);

            Gizmos.color = Color.blue;
            Gizmos.DrawRay(_character.CharacterRoot.position, _character.CharacterRoot.forward * 5);
            Gizmos.color = Color.green;
            Gizmos.DrawRay(_character.CharacterRoot.position, LookAtRotation * (Vector3.forward * 5));         

            if (CurrentMoveState == MoveState.TurnInPlace)
            {
                Gizmos.color = new Color(0, 1, 1, 0.3f);
                Gizmos.DrawRay(_character.CharacterRoot.position, _character.CharacterRoot.forward);
                Gizmos.DrawRay(_character.CharacterRoot.position, -_character.CharacterRoot.forward);
                Gizmos.DrawRay(_character.CharacterRoot.position, _character.CharacterRoot.right);
                Gizmos.DrawRay(_character.CharacterRoot.position, -_character.CharacterRoot.right);
                Gizmos.DrawRay(_character.CharacterRoot.position, Vector3.Lerp(_character.CharacterRoot.forward, _character.CharacterRoot.right, 0.5f).normalized);
                Gizmos.DrawRay(_character.CharacterRoot.position, Vector3.Lerp(-_character.CharacterRoot.forward, _character.CharacterRoot.right, 0.5f).normalized);
                Gizmos.DrawRay(_character.CharacterRoot.position, Vector3.Lerp(_character.CharacterRoot.forward, -_character.CharacterRoot.right, 0.5f).normalized);
                Gizmos.DrawRay(_character.CharacterRoot.position, Vector3.Lerp(-_character.CharacterRoot.forward, -_character.CharacterRoot.right, 0.5f).normalized);

                Gizmos.color = Color.white;
                Gizmos.DrawRay(_character.CharacterRoot.position, (_turnInPlaceStart * Vector3.forward) * 5);
                Gizmos.DrawRay(_character.CharacterRoot.position, (_turnInPlaceTarget * Vector3.forward) * 5);


                if (_currentFocusTime > _targetFocusTime)
                {
                    Gizmos.color = Color.red;
                }
                else
                {
                    Gizmos.color = Color.green;
                }
                Gizmos.DrawLine(InputCharacterLookAtPoint, _previousFocusPoint);
            }
        }
        #endregion
    }
}
