using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Project.SoundSystem;
using Project.Utilities;

namespace Project.CharacterSystem
{
    public class FootstepsSFX : MonoBehaviour
    {
        #region Temp
        //[Header("Temporary Things", order = 0)]
        #endregion

        #region Fields
        [Header("Fields", order = 1)]
        [SerializeField] private Character _character;
        [SerializeField] private SoundsLibrary _soundsLibrary;
        [SerializeField] private LayerMaskSO _footsteps;


        [NaughtyAttributes.HorizontalLine]
        [SerializeField] private AudioSource _audioSourceLeftFoot;
        [SerializeField] private AudioSource _audioSourceRightFoot;

        [NaughtyAttributes.HorizontalLine]

        private Transform _leftFoot;
        private Transform _rightFoot;
        [SerializeField] private AnimationRiggingExtension.ExtractTransformConstraint _leftFootBeforeIK;
        [SerializeField] private AnimationRiggingExtension.ExtractTransformConstraint _rightFootBeforeIK;

        [NaughtyAttributes.HorizontalLine]

        [SerializeField] private float _triggerPlayBelowHeight;
        [SerializeField] private float _releasePlayAboveHeight;
        private bool _playedLeft;
        private bool _playedRight;

        private bool _canRepeatThisSound = true;

        #endregion

        #region Functions
  
         PhysicMaterial GetSoundKey(Transform foot)
        {
            Ray ray = new Ray(foot.position, Vector3.down);
            if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, _footsteps.LayerMask, QueryTriggerInteraction.Ignore))
            {          
                 return hit.collider.sharedMaterial;
            }
          
            return null;
        }

        bool ShouldPlayFootstepSound(ref bool played, AnimationRiggingExtension.ExtractTransformConstraint footBeforeIK)
        {
            if (played == false && _character.CharacterController.isGrounded)
            {
                if (_character.CharacterRoot.InverseTransformPoint(footBeforeIK.data.position).y < _triggerPlayBelowHeight)
                {
                    played = true;
                    return true;
                }
            }
            else if (played)
            {
                if (_character.CharacterRoot.InverseTransformPoint(footBeforeIK.data.position).y > _releasePlayAboveHeight)
                {
                    played = false;
                }
            }

            return false;
        }
      
        #endregion

        #region Methods
        void Start()
        {
            AssignTransforms();
        }
        void AssignTransforms()
        {
            _leftFoot = _character.CharacterAnimator.Animator.GetBoneTransform(HumanBodyBones.LeftFoot);
            _rightFoot = _character.CharacterAnimator.Animator.GetBoneTransform(HumanBodyBones.RightFoot);          
        }

        void Update()
        {
            DetectStep();
        }
        void DetectStep()
        {

            if (ShouldPlayFootstepSound(ref _playedLeft, _leftFootBeforeIK))
            {
                PhysicMaterial fromLeftFoot = GetSoundKey(_leftFoot);
                PlayFootstep(fromLeftFoot, _audioSourceLeftFoot);
                //Debug.Log(nameof(_playedLeft));
            }

            if (ShouldPlayFootstepSound(ref _playedRight, _rightFootBeforeIK))
            {
                PhysicMaterial fromRightFoot = GetSoundKey(_rightFoot);
                PlayFootstep(fromRightFoot, _audioSourceRightFoot);
                //Debug.Log(nameof(_playedRight));
            }
        }
        void PlayFootstep(PhysicMaterial key, AudioSource audioSource)
        {
            if (key == null) return;
          
            SoundsLibrary.SoundsSet soundsSet = _soundsLibrary.GetSoundsSet(key);
            AudioClip footstepClip = null;

            if (_character.CharacterAnimator.MoveArmed.StateActive
                || _character.CharacterAnimator.MoveUnarmed.StateActive
                || _character.CharacterAnimator.TurnInPlaceArmed.StateActive
                || _character.CharacterAnimator.TurnInPlaceUnarmed.StateActive)
            {               
                if (_character.Movement.DirectionAndMoveGear.magnitude <= 1.5f)
                {
                    footstepClip = soundsSet.GetRandomFromVariant(soundsSet.FootstepWalk);
                }
                if (_character.Movement.DirectionAndMoveGear.magnitude > 1.5f)
                {
                    footstepClip = soundsSet.GetRandomFromVariant(soundsSet.FootstepRun);
                }
            }

            if (_canRepeatThisSound)
            {
                if (_character.CharacterAnimator.JumpDownArmed.StateActive || _character.CharacterAnimator.JumpDownUnarmed.StateActive)
                {
                    _canRepeatThisSound = false;
                    footstepClip = soundsSet.GetRandomFromVariant(soundsSet.FootstepLanding);
                }         
            }
            else
            {
                if (_character.CharacterAnimator.JumpDownArmed.StateActive == false && _character.CharacterAnimator.JumpDownUnarmed.StateActive == false)
                {
                    _canRepeatThisSound = true;
                }
            }

            if (footstepClip == null) return;
            audioSource.PlayOneShot(footstepClip, 1);
        }
        private void OnDrawGizmosSelected()
        {
            if (_playedLeft)
            {
                Gizmos.color = Color.magenta;
                Gizmos.DrawWireSphere(_leftFoot.position, 0.1f);
            }

            if (_playedRight)
            {
                Gizmos.color = Color.magenta;
                Gizmos.DrawWireSphere(_rightFoot.position, 0.1f);
            }
        }

        #endregion
    }
}
