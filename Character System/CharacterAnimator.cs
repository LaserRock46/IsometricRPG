using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Project.Utilities;
using NaughtyAttributes;
using UnityEngine.Animations.Rigging;

namespace Project.CharacterSystem
{
    public class CharacterAnimator : MonoBehaviour
    {
        #region Temp
        //[Header("Temporary Things", order = 0)]
        #endregion

        #region Fields
        [Header("Fields", order = 1)]
        [SerializeField] private Character _character;

        [SerializeField] private Animator _animator;

        [SerializeField] private CharacterAnimatorParams _characterAnimatorParams;
        public Character Character { get => _character; private set => _character = value; }
        public Animator Animator { get => _animator; private set => _animator = value; }
        public CharacterAnimatorParams CharacterAnimatorParams { get => _characterAnimatorParams; set => _characterAnimatorParams = value; }

        [HorizontalLine]
        #region States
        [SerializeField] private AnimatorStateActivity _moveArmed;
        [SerializeField] private AnimatorStateActivity _moveUnarmed;
        [SerializeField] private AnimatorStateActivity _armedIdle;
        [SerializeField] private AnimatorStateActivity _unarmedIdle;
        [SerializeField] private AnimatorStateActivity _singleStepArmed;
        [SerializeField] private AnimatorStateActivity _singleStepUnarmed;
        [SerializeField] private AnimatorStateActivity _turnInPlaceArmed;
        [SerializeField] private AnimatorStateActivity _turnInPlaceUnarmed;
        [SerializeField] private AnimatorStateActivity _toArmed;
        [SerializeField] private AnimatorStateActivity _toUnarmed;
        [SerializeField] private AnimatorStateActivity _jumpDownArmed;
        [SerializeField] private AnimatorStateActivity _jumpDownUnarmed;
        [SerializeField] private AnimatorStateActivity _climbingLadder;
        [SerializeField] private AnimatorStateActivity _climbingLadderToTop;
        [SerializeField] private AnimatorStateActivity _climbingLadderFromTop;
        [SerializeField] private AnimatorStateActivity _standUpFaceUp;
        [SerializeField] private AnimatorStateActivity _standUpFaceDown;


        public AnimatorStateActivity MoveArmed { get => _moveArmed; set => _moveArmed = value; }
        public AnimatorStateActivity MoveUnarmed { get => _moveUnarmed; set => _moveUnarmed = value; }
        public AnimatorStateActivity ArmedIdle { get => _armedIdle; set => _armedIdle = value; }
        public AnimatorStateActivity UnarmedIdle { get => _unarmedIdle; set => _unarmedIdle = value; }
        public AnimatorStateActivity SingleStepArmed { get => _singleStepArmed; set => _singleStepArmed = value; }
        public AnimatorStateActivity SingleStepUnarmed { get => _singleStepUnarmed; set => _singleStepUnarmed = value; }
        public AnimatorStateActivity TurnInPlaceArmed { get => _turnInPlaceArmed; set => _turnInPlaceArmed = value; }
        public AnimatorStateActivity TurnInPlaceUnarmed { get => _turnInPlaceUnarmed; set => _turnInPlaceUnarmed = value; }
        public AnimatorStateActivity ToArmed { get => _toArmed; set => _toArmed = value; }
        public AnimatorStateActivity ToUnarmed { get => _toUnarmed; set => _toUnarmed = value; }
        public AnimatorStateActivity JumpDownArmed { get => _jumpDownArmed; set => _jumpDownArmed = value; }
        public AnimatorStateActivity JumpDownUnarmed { get => _jumpDownUnarmed; set => _jumpDownUnarmed = value; }      
        public AnimatorStateActivity ClimbingLadder { get => _climbingLadder; set => _climbingLadder = value; }
        public AnimatorStateActivity ClimbingLadderToTop { get => _climbingLadderToTop; set => _climbingLadderToTop = value; }
        public AnimatorStateActivity ClimbingLadderFromTop { get => _climbingLadderFromTop; set => _climbingLadderFromTop = value; }
        public AnimatorStateActivity StandUpFaceUp { get => _standUpFaceUp; set => _standUpFaceUp = value; }
        public AnimatorStateActivity StandUpFaceDown { get => _standUpFaceDown; set => _standUpFaceDown = value; }


        #endregion

        #endregion

        #region Functions

        #endregion

        #region Methods
        void Start()
        {
            GetStatesActivity();
        }
        void GetStatesActivity()
        {
            AnimatorStateActivity[] allStatasActivity = _animator.GetBehaviours<AnimatorStateActivity>();

            Dictionary<string, AnimatorStateActivity> statesDictionary = new Dictionary<string, AnimatorStateActivity>();

            for (int i = 0; i < allStatasActivity.Length; i++)
            {
                statesDictionary.Add(allStatasActivity[i].PropertyPath, allStatasActivity[i]);
            }

            SetStatesActivityReferences();

            void SetStatesActivityReferences()
            {
                _moveArmed = statesDictionary[nameof(_moveArmed)];
                _moveUnarmed = statesDictionary[nameof(_moveUnarmed)];
                _armedIdle = statesDictionary[nameof(_armedIdle)];
                _unarmedIdle = statesDictionary[nameof(_unarmedIdle)];
                _singleStepArmed = statesDictionary[nameof(_singleStepArmed)];
                _singleStepUnarmed = statesDictionary[nameof(_singleStepUnarmed)];
                _turnInPlaceArmed = statesDictionary[nameof(_turnInPlaceArmed)];
                _turnInPlaceUnarmed = statesDictionary[nameof(_turnInPlaceUnarmed)];
                _toArmed = statesDictionary[nameof(_toArmed)];
                _toUnarmed = statesDictionary[nameof(_toUnarmed)];
                _jumpDownArmed = statesDictionary[nameof(_jumpDownArmed)];
                _jumpDownUnarmed = statesDictionary[nameof(_jumpDownUnarmed)];             
                _climbingLadder = statesDictionary[nameof(_climbingLadder)];
                _climbingLadderFromTop = statesDictionary[nameof(_climbingLadderFromTop)];
                _climbingLadderToTop = statesDictionary[nameof(_climbingLadderToTop)];
                _standUpFaceUp = statesDictionary[nameof(_standUpFaceUp)];
                _standUpFaceDown = statesDictionary[nameof(_standUpFaceDown)];
            }
        }
        #region Parameters     
        public void SetMove(float forward, float sideward)
        {
            Animator.SetFloat(_characterAnimatorParams.MoveForward, forward);
            Animator.SetFloat(_characterAnimatorParams.MoveSideward, sideward);
        }
        public void SetMoving(bool value)
        {
            Animator.SetBool(_characterAnimatorParams.Moving, value);
        }
        public void SetTurnAmount(float degree)
        {
            Animator.SetFloat(_characterAnimatorParams.TurnAmountDegrees, degree);
        }
        public void SetTurnInPlace()
        {
            Animator.SetTrigger(_characterAnimatorParams.TurnInPlace);
        }
        public void SetSingleStep()
        {
            Animator.SetTrigger(_characterAnimatorParams.SingleStep);
        }
        public void SetJumpDown(float distance)
        {
            Animator.SetFloat(_characterAnimatorParams.DistanceVertical, distance);
            Animator.SetTrigger(_characterAnimatorParams.JumpDown);
        }
        public void SetUnarmedRifleDown(bool value)
        {
            Animator.SetBool(_characterAnimatorParams.UnarmedRifleDown, value);
        }
        public void SetClimbLadderTop()
        {
            Animator.SetTrigger(_characterAnimatorParams.ClimbLadderTop);
        }
        public void SetClimbLadderBottom()
        {
            Animator.SetTrigger(_characterAnimatorParams.ClimbLadderBottom);
        }
        public void SetClimbingLadderSpeed(float value)
        {
            Animator.SetFloat(_characterAnimatorParams.ClimbingLadderSpeed, value);
        }
        public void SetStandUpFaceUp()
        {
            Animator.Play("Stand Up Face Up");
        }
        public void SetStandUpFaceDown()
        {
            Animator.Play("Stand Up Face Down");
        }
        #endregion

        #region Layers

        #endregion

        #endregion
    }
}
