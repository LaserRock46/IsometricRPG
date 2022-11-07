using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;
using Project.Utilities;

namespace Project.CharacterSystem
{
    public class SightAndDetect : MonoBehaviour
    {
        #region Temp
        //[Header("Temporary Things", order = 0)]
        #endregion

        #region Fields
        [Header("Fields", order = 1)]
        [SerializeField] private Character _character;

        [HorizontalLine]

        [SerializeField] LayerMaskSO _visibleForCharacter;
        private Transform _sightPointOrigin;
        private Vector3 _sightPoint;
        private GameObject _sightObject;

        public Vector3 SightPoint { get => _sightPoint; set => _sightPoint = value; }
        public GameObject SightObject { get => _sightObject; set => _sightObject = value; }

        #endregion

        #region Functions

        #endregion

        #region Methods
        void Start()
        {
            _sightPointOrigin = _character.CharacterAnimator.Animator.GetBoneTransform(HumanBodyBones.Head);
        }
        void Update()
        {
            SightPointUpdate();
        }
        void SightPointUpdate()
        {
            Ray ray = new Ray(_sightPointOrigin.position, _sightPointOrigin.forward);
            if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, _visibleForCharacter.LayerMask, QueryTriggerInteraction.Ignore))
            {
                _sightPoint = hit.point;
                _sightObject = hit.collider.gameObject;
            }
            else
            {
                _sightObject = null;
            }
        }
        private void OnDrawGizmosSelected()
        {
            if (_sightPoint != Vector3.zero)
            {
                Gizmos.color = Color.green;
                Gizmos.DrawLine(_sightPointOrigin.position, _sightPoint);
            }
        }
        #endregion
    }
}
