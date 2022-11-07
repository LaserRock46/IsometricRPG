using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Project.Utilities;

namespace Project.PlayerSystem
{
    public class ReticleRenderer : MonoBehaviour
    {
        #region Temp
        [Header("Temporary Things", order = 0)]
        [SerializeField] private float _debugWeight;
        [SerializeField] private float _debugAngleBetween;
        [SerializeField] private Quaternion _debugFiringPointRot;
        [SerializeField] private Quaternion _debugStableDirRot;
        #endregion

        #region Fields
        [Header("Fields", order = 1)]
        [SerializeField] private CharacterSystem.Character _character;
        [SerializeField] private LineRenderer _reticleLoop;
        [UnityEngine.Serialization.FormerlySerializedAs("_reticleCenter")]
        [SerializeField] private Transform _reticleObject;
        [SerializeField] private Transform _reticleOrigin;
        [SerializeField] private Transform[] _reticlePoints;
        [SerializeField] private float _widthMultiplier;

        private Vector3 _stabilizedOrigin;
        private Vector3 _stabilizedDirection;
        private Vector3 _stabilizedHitPoint;
        [SerializeField] private float _weightBiasOut;
        [SerializeField] private float _weightBiasIn;
      
        [SerializeField] private LayerMaskSO _visibleForCharacter;

        #endregion

        #region Functions
        Vector3 GetNearestPointOnSegment(Vector3 start, Vector3 end, Vector3 point)
        {
            Vector3 dir = (end - start);
            dir.Normalize();

            Vector3 v = point - start;
            float dist = Vector3.Dot(v, dir);

            return start + dir * dist;
        }
        Vector3 GetSegmentPlaneIntersection(Vector3 s0, Vector3 s1, Vector3 planeOrigin, Vector3 planeNormal)
        {
            Vector3 u = s1 - s0;
            Vector3 w = s0 - planeOrigin;

            float D =Vector3.Dot(planeNormal, u);
            float N = -Vector3.Dot(planeNormal, w);
            float sI = N / D;

            return s0 + sI * u;
        }

        Vector3 GetEndOfDispersionSegment(float angle, Transform start)
        {
            Vector3 direction = start.TransformDirection(Quaternion.Euler(0, angle, 0) * Vector3.forward);
            return start.position + (direction * 1000);
        }
       
        #endregion

        #region Methods
        void Start()
        {
            _reticleLoop.positionCount = _reticlePoints.Length;
        }
        void Update()
        {
            StateUpdate();
        }
        void StateUpdate()
        {
            if(_character != null)
            {
                if (_character.WeaponSystemNode.WeaponWorldObject != null)
                {
                    if(_reticleLoop.gameObject.activeSelf == false)
                    {
                        _reticleLoop.gameObject.SetActive(true);
                    }
                    ReticleSightOrientationUpdate();
                    ReticleVisualsUpdate();
                }
                else if(_reticleObject.gameObject.activeSelf == true)
                {
                    _reticleLoop.gameObject.SetActive(false);
                   
                }
            }
        }
        void ReticleSightOrientationUpdate()
        {          
           _stabilizedOrigin = _character.WeaponSystemNode.WeaponWorldObject.VisualFiringPoint.position;
           
            Vector3 stableFakeDirection = (_character.WeaponSystemNode.Aiming.InputAimLookAt - _stabilizedOrigin).normalized;
            Quaternion fakeDirectionAsQuaternion = Quaternion.LookRotation(stableFakeDirection, Vector3.up);
            float absAngleBetween = Quaternion.Angle(_character.WeaponSystemNode.WeaponWorldObject.VisualFiringPoint.rotation, fakeDirectionAsQuaternion);
            float weight = Mathf.InverseLerp(_weightBiasOut, _weightBiasIn, absAngleBetween) * Mathf.Clamp(_character.Movement.DirectionAndMoveGear.magnitude,0,1);
            _stabilizedDirection = Vector3.Lerp(_character.WeaponSystemNode.WeaponWorldObject.VisualFiringPoint.forward, stableFakeDirection, weight);

            _debugAngleBetween = absAngleBetween;
            _debugWeight = weight;
            _debugFiringPointRot = _character.WeaponSystemNode.WeaponWorldObject.VisualFiringPoint.rotation;
            _debugStableDirRot = fakeDirectionAsQuaternion;

            Ray ray = new Ray(_stabilizedOrigin, _stabilizedDirection);
            if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, _visibleForCharacter.LayerMask, QueryTriggerInteraction.Ignore))
            {
                _stabilizedHitPoint = hit.point;
            }

            _reticleOrigin.position = _stabilizedOrigin;
            _reticleOrigin.forward = _stabilizedDirection;
            _reticleObject.position = _stabilizedHitPoint;
            _reticleObject.forward = _stabilizedDirection;
        }
        void ReticleVisualsUpdate()
        {
            _reticleLoop.widthMultiplier = Camera.main.fieldOfView * _widthMultiplier;

            if (_character.WeaponSystemNode.WeaponHitscan.HitPoint == _reticleObject.position) return;
       
            Vector3 leftDispersionEnd = GetEndOfDispersionSegment(_character.WeaponSystemNode.SpreadController.MaxSpread / 2, _reticleOrigin);         
            Vector3 intersection = GetSegmentPlaneIntersection(_reticleOrigin.position, leftDispersionEnd, _reticleObject.position, _reticleObject.forward);
            float distanceFromCenter = Vector3.Distance(_reticleObject.position, intersection);
            _reticleObject.localScale = distanceFromCenter * Vector3.one;

            for (int i = 0; i < _reticlePoints.Length; i++)
            {
                _reticleLoop.SetPosition(i, _reticlePoints[i].position);             
            }
        }
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.green;         
            Vector3 leftDispersionEnd = GetEndOfDispersionSegment(-_character.WeaponSystemNode.SpreadController.MaxSpread / 2, _reticleOrigin);
            Vector3 rightDispersionEnd = GetEndOfDispersionSegment(_character.WeaponSystemNode.SpreadController.MaxSpread / 2, _reticleOrigin);

            Gizmos.DrawLine(_stabilizedOrigin,leftDispersionEnd);
            Gizmos.DrawLine(_stabilizedOrigin, rightDispersionEnd);

            if (_character.WeaponSystemNode.WeaponHitscan.HitPoint != Vector3.zero)
            {
                Vector3 nearestLeft = GetSegmentPlaneIntersection(_stabilizedOrigin, leftDispersionEnd, _character.WeaponSystemNode.WeaponHitscan.HitPoint, _reticleObject.forward);
                Vector3 nearestRight = GetSegmentPlaneIntersection(_stabilizedOrigin, rightDispersionEnd, _character.WeaponSystemNode.WeaponHitscan.HitPoint, _reticleObject.forward);              

                Gizmos.DrawLine(_character.WeaponSystemNode.WeaponHitscan.HitPoint, nearestLeft);
                Gizmos.DrawLine(_character.WeaponSystemNode.WeaponHitscan.HitPoint, nearestRight);
            }
            Gizmos.color = Color.yellow;
            Gizmos.DrawRay(_stabilizedOrigin, -_character.WeaponSystemNode.WeaponWorldObject.VisualFiringPoint.forward);
        }
        #endregion
    }
}
