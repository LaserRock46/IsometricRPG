using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using NaughtyAttributes;
using System;

namespace Project.CharacterSystem
{
    public enum RagdollState
    {
        Setup = 0,
        WaitFarFallVelocity = 1,
        WaitForStablePosition = 2,
        WaitForGetUp = 3,
        SyncRagdollLayerWithAnimation = 4,
        Disabled = 5,
    }
    public class RagdollController : MonoBehaviour
    {
        #region Temp
        //[Header("Temporary Things", order = 0)]
        #endregion

        #region Fields
        [Header("Fields", order = 1)]
        [SerializeField] private Character _character;

        [HorizontalLine]
        [SerializeField] private RagdollState _ragdollState;

        [SerializeField] private Rigidbody[] _bodyParts;

        [SerializeField] private RigBuilder _rigBuilder;
        private float[] _previousLayersWeight;

        [HorizontalLine]
        [Header("Ragdoll Pose")]

        [SerializeField] private RigLayer _ragdollPoseRig;

        [SerializeField] private MultiPositionConstraint[] _positionConstraints;
        [SerializeField] private MultiRotationConstraint[] _rotationConstraints;


        [SerializeField] private Transform _hips;
        [SerializeField] private Transform _head;

        [HorizontalLine]
        [Header("Get Up")]

        [SerializeField] private Rigidbody _hipsRB;
        private bool _getUp;
        [SerializeField] private float _timeToGetUp;
        private float _getUpTime;
        private Vector3 _getUpRootPosition;

        [SerializeField] private float _fallVelocityThreshold;
        [SerializeField] private float _stableVelocityThreshold;
        [SerializeField] private float[] _ragdollConstraintSyncSpeed;


        public RagdollState RagdollState { get => _ragdollState; private set => _ragdollState = value; }
        public event Action OnRagdollEnabled;
        //public event Action OnRagdollDisabled;
        #endregion

        #region Functions
        bool IsCharacterFacingUp()
        {
            return Vector3.Angle(_hips.forward, Vector3.up) < 90;
        }
        bool AllConstraintsMuted()
        {
            bool notMuted = false;

            for (int i = 0; i < _positionConstraints.Length; i++)
            {
                if (_positionConstraints[i].weight > 0)
                {
                    notMuted = true;
                }
            }

            return notMuted == false;
        }
        #endregion

        #region Methods     
        public void Ragdolled(bool getUp)
        {
            enabled = true;
            RagdollState = RagdollState.Setup;
            _getUp = getUp;
            OnRagdollEnabled?.Invoke();
        }
        private void Awake()
        {
            _ragdollState = RagdollState.Disabled;
           
        }
        private void Update()
        {
           StateUpdate();
        }
        void StateUpdate()
        {
            switch (RagdollState)
            {
                case RagdollState.Setup:
                    _character.AnimatorCallbacks.OnAnimatorMoveCallback += SyncRootPosition;
                    DisableIK();
                    SetRagdollActiveState(true);
              
                    if (_getUp)
                    {
                        RagdollState = RagdollState.WaitFarFallVelocity;
                    }
                    else
                    {
                        RagdollState = RagdollState.Disabled;
                    }
                    break;
                case RagdollState.WaitFarFallVelocity:
                    if (_hipsRB.velocity.magnitude > _fallVelocityThreshold)
                    {
                        RagdollState = RagdollState.WaitForStablePosition;
                    }
                    break;
                case RagdollState.WaitForStablePosition:
                    if (_hipsRB.velocity.magnitude < _stableVelocityThreshold)
                    {
                        _getUpTime = Time.time + _timeToGetUp;
                        RagdollState = RagdollState.WaitForGetUp;
                    }
                    break;
                case RagdollState.WaitForGetUp:
                    if (Time.time > _getUpTime)
                    {
                        RealignRootTransform();
                        SyncIKPoseWithRagdoll();
                        SetRagdollActiveState(false);
                        PlayRelevantAnimation();
                        RagdollState = RagdollState.SyncRagdollLayerWithAnimation;                   
                    }
                    break;              
                case RagdollState.SyncRagdollLayerWithAnimation:
                    for (int i = 0; i < _positionConstraints.Length; i++)
                    {
                        _positionConstraints[i].weight -= Time.deltaTime * _ragdollConstraintSyncSpeed[i];
                        _rotationConstraints[i].weight -= Time.deltaTime * _ragdollConstraintSyncSpeed[i];
                    }
                  
                    if (AllConstraintsMuted())
                    {
                        RagdollState = RagdollState.Disabled;
                    }                
                    break;
                case RagdollState.Disabled:
                    for (int i = 0; i < _positionConstraints.Length; i++)
                    {
                        _positionConstraints[i].weight = 1;
                        _rotationConstraints[i].weight = 1;
                    }
                    _ragdollPoseRig.rig.weight = 0;
                    
                    enabled = false;
                    _character.AnimatorCallbacks.OnAnimatorMoveCallback -= SyncRootPosition;
                    break;
            }
        }
        void SyncRootPosition()
        {
            if (_positionConstraints[0].weight > 0.95f)
            {            
                _character.CharacterRoot.position = _getUpRootPosition;
            }
        }
        void DisableIK()
        {
            _previousLayersWeight = new float[_rigBuilder.layers.Count];
            for (int i = 0; i < _rigBuilder.layers.Count; i++)
            {
                _previousLayersWeight[i] = _rigBuilder.layers[i].rig.weight;
                _rigBuilder.layers[i].rig.weight = 0;
            }

            _ragdollPoseRig.rig.weight = 1;
          
        }
        void SyncIKPoseWithRagdoll()
        {         
            for (int i = 0; i < _positionConstraints.Length; i++)
            {
                Transform constrainedObject = _positionConstraints[i].data.constrainedObject;
                _positionConstraints[i].data.sourceObjects[0].transform.SetPositionAndRotation(constrainedObject.position, constrainedObject.rotation);
             
            }          
        }
       
        void RealignRootTransform()
        {
            Physics.Raycast(_hips.position, Vector3.down, out RaycastHit newRootHit, Mathf.Infinity, Physics.AllLayers, QueryTriggerInteraction.Ignore);

            _character.CharacterRoot.position = newRootHit.point;
            _getUpRootPosition = newRootHit.point;

            Vector3 headWithSameHeightAsHips = _head.position;
            headWithSameHeightAsHips.y = newRootHit.point.y;

            if(IsCharacterFacingUp())
            {
                _character.CharacterRoot.forward = (newRootHit.point - headWithSameHeightAsHips).normalized;
            }
            else
            {
                _character.CharacterRoot.forward = (headWithSameHeightAsHips - newRootHit.point).normalized;
            }
            Debug.DrawRay(_character.CharacterRoot.position, _character.CharacterRoot.forward, Color.blue, 6);
            Debug.DrawRay(_character.CharacterRoot.position, _character.CharacterRoot.up, Color.green, 6);
            Debug.DrawRay(_character.CharacterRoot.position, _character.CharacterRoot.right, Color.red, 6);       
        }
        void PlayRelevantAnimation()
        {
            if (IsCharacterFacingUp())
            {
                _character.CharacterAnimator.SetStandUpFaceUp();
            }
            else
            {
                _character.CharacterAnimator.SetStandUpFaceDown();
            }
            _character.CharacterRoot.position = _getUpRootPosition;           
        }
        void SetRagdollActiveState(bool enabled)
        {
            _hips.parent = enabled ? null : _character.CharacterRoot;
            _character.CharacterAnimator.Animator.enabled = !enabled;
            _character.CharacterController.enabled = !enabled;
        
            if (enabled)
            {
                _character.CharacterRoot.position = _getUpRootPosition;
                _character.CharacterAnimator.Animator.rootPosition = _getUpRootPosition;
            }
           
            for (int i = 0; i < _bodyParts.Length; i++)
            {
                _bodyParts[i].isKinematic = !enabled;
                _bodyParts[i].constraints = RigidbodyConstraints.None;
            }
        }
        private void OnDrawGizmosSelected()
        {
            Gizmos.DrawWireSphere(_getUpRootPosition, 0.1f);

            Gizmos.DrawRay(_character.CharacterRoot.position, _character.CharacterRoot.forward);
            Gizmos.DrawRay(_character.CharacterRoot.position, _character.CharacterRoot.up);
            Gizmos.DrawRay(_character.CharacterRoot.position, _character.CharacterRoot.right);      
        }

        #endregion
    }
}
