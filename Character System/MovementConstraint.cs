using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;
using Project.Utilities;

namespace Project.CharacterSystem
{
    public class MovementConstraint : MonoBehaviour
    {
        #region Temp
        //[Header("Temporary Things", order = 0)]
        #endregion

        #region Fields
        [Header("Fields", order = 1)]
        [SerializeField] private Character _character;

        [HorizontalLine]

        [SerializeField] private LayerMaskSO _movmentConstraintLayer;
        [ReadOnly][SerializeField] private float _movementConstrainedToDistance;
        [SerializeField] private Transform _raysOrigin;
        [SerializeField] private Transform[] _raysDirections;
        private RaycastHit[] _raycastHits;

        public float MovementConstrainedToDistance { get => _movementConstrainedToDistance; set => _movementConstrainedToDistance = value; }
        #endregion

        #region Functions   
        private void Start()
        {
            _raycastHits = new RaycastHit[_raysDirections.Length];
            _raysOrigin.SetParent(_character.CharacterRoot);
        }
        public void MoveConstraintUpdate()
        {
            if (_character.Movement.DirectionAndMoveGear.magnitude == 0 && _character.Movement.InputMove == Vector3.zero) return;

            Vector3 direction = _character.CharacterRoot.TransformDirection(_character.Movement.DirectionAndMoveGear.magnitude > 0 ? _character.Movement.DirectionAndMoveGear.normalized : _character.Movement.InputMove);

            _raysOrigin.forward = direction;

            float shortestDistance = 3;

            for (int i = 0; i < _raysDirections.Length; i++)
            {
                if (Physics.Raycast(_raysOrigin.position,_raysDirections[i].forward, out _raycastHits[i], 3, _movmentConstraintLayer.LayerMask, QueryTriggerInteraction.Ignore))
                {
                    if(_raycastHits[i].distance < shortestDistance)
                    {
                        shortestDistance = _raycastHits[i].distance;
                    }
                }
                else
                {
                    _raycastHits[i].distance = 3;
                }
            }
            _movementConstrainedToDistance = shortestDistance;
        }
        #endregion

        #region Methods
        private void OnDrawGizmosSelected()
        {          
            if (_raycastHits == null) return;
            for (int i = 0; i < _raysDirections.Length; i++)
            {
                Gizmos.DrawRay(_raysOrigin.position, _raysDirections[i].forward * _raycastHits[i].distance);
            }
          
        }
        #endregion
    }
}
