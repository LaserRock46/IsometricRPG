using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Project.Utilities
{
    public class AnimatorStateActivity : StateMachineBehaviour
    {
        [SerializeField] private bool _stateActive;
        [SerializeField] private bool _willBeActiveSoon;
        [SerializeField] private bool _exitFlag;
        [SerializeField] private string _propertyPath;
        [SerializeField] private string _stateName;

        [SerializeField] private bool _debugLog;
        private AnimatorStateInfo _animatorStateInfo;
        public Animator Animator;
        public bool StateActive { get => _stateActive; private set => _stateActive = value; }
        public bool ExitFlag { get => _exitFlag; set => _exitFlag = value; }
        public bool WillBeActiveSoon { get => _willBeActiveSoon; set => _willBeActiveSoon = value; }
        public string PropertyPath { get => _propertyPath; private set => _propertyPath = value; }
        public AnimatorStateInfo AnimatorStateInfo { get => _animatorStateInfo; set => _animatorStateInfo = value; }

      
        public void Play()
        {
            Animator.Play(_stateName);
        }
        // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
        override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {       
            _stateActive = true;
            _animatorStateInfo = stateInfo;

            if (_debugLog)
            {
                Debug.Log(nameof(OnStateEnter) + _propertyPath + " " + Time.realtimeSinceStartup);
            }
        }

        // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
        //override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        //{
        //   
        //}

        // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
        override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            _stateActive = false;
            _willBeActiveSoon = false;
            _exitFlag = true;

            if (_debugLog)
            {
                Debug.Log(nameof(OnStateExit) + _propertyPath + " " + Time.realtimeSinceStartup);
            }
        }

        // OnStateMove is called right after Animator.OnAnimatorMove()
        //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        //{
        //    // Implement code that processes and affects root motion
        //}

        // OnStateIK is called right after Animator.OnAnimatorIK()
        //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        //{
        //    // Implement code that sets up animation IK (inverse kinematics)
        //}
    }
}
