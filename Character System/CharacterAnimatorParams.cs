using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

namespace Project.CharacterSystem
{
    [System.Serializable]
    public class CharacterAnimatorParams
    {
        [SerializeField] private Animator _animator;

        [AnimatorParam("_animator")] [SerializeField] private int _moveForward;
        [AnimatorParam("_animator")] [SerializeField] private int _moveSideward;
        [AnimatorParam("_animator")] [SerializeField] private int _moving;
        [AnimatorParam("_animator")] [SerializeField] private int _unarmedRifleDown;
        [AnimatorParam("_animator")] [SerializeField] private int _turnAmountDegrees;
        [AnimatorParam("_animator")] [SerializeField] private int _turnInPlace;
        [AnimatorParam("_animator")] [SerializeField] private int _turnStartWalk;
        [AnimatorParam("_animator")] [SerializeField] private int _singleStep;
        [AnimatorParam("_animator")] [SerializeField] private int _jumpDown;
        [AnimatorParam("_animator")] [SerializeField] private int _climbLadderTop;
        [AnimatorParam("_animator")] [SerializeField] private int _climbLadderBottom;
        [AnimatorParam("_animator")] [SerializeField] private int _climbingLadderSpeed;
        [AnimatorParam("_animator")] [SerializeField] private int _distanceVertical;
        [AnimatorParam("_animator")] [SerializeField] private int _distanceHorizontal;



        public int MoveForward { get => _moveForward; set => _moveForward = value; }
        public int MoveSideward { get => _moveSideward; set => _moveSideward = value; }
        public int Moving { get => _moving; set => _moving = value; }
        public int UnarmedRifleDown { get => _unarmedRifleDown; set => _unarmedRifleDown = value; }
        public int TurnAmountDegrees { get => _turnAmountDegrees; set => _turnAmountDegrees = value; }
        public int TurnInPlace { get => _turnInPlace; set => _turnInPlace = value; }
        public int TurnStartWalk { get => _turnStartWalk; set => _turnStartWalk = value; }
        public int SingleStep { get => _singleStep; set => _singleStep = value; }
        public int JumpDown { get => _jumpDown; set => _jumpDown = value; }
        public int ClimbLadderTop { get => _climbLadderTop; set => _climbLadderTop = value; }
        public int ClimbLadderBottom { get => _climbLadderBottom; set => _climbLadderBottom = value; }
        public int ClimbingLadderSpeed { get => _climbingLadderSpeed; set => _climbingLadderSpeed = value; }
        public int DistanceVertical { get => _distanceVertical; set => _distanceVertical = value; }
        public int DistanceHorizontal { get => _distanceHorizontal; set => _distanceHorizontal = value; }
    }
  
}
