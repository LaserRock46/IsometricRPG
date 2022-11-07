using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Project.Utilities;
using NaughtyAttributes;

namespace Project.PlayerSystem
{
    public class PlayerPointer : MonoBehaviour
    {
         #region Temp
        //[Header("Temporary Things", order = 0)]
        #endregion

        #region Fields
        [Header("Fields", order = 1)]
        [SerializeField] private CharacterSystem.Character _character;
        [SerializeField] private PointerPosition _pointerPosition;
        [SerializeField] private LayerMaskSO _pointerMask;
        private RaycastHit _worldPointHit;
        [SerializeField] private Vector3 _worldPoint;
        [SerializeField] private Vector3 _characterLookAtPoint;
        [SerializeField] private GameObject _object;
        private Plane _playerGroundLevelPlane;

        [HorizontalLine]

        [SerializeField] private LayerMaskSO _visibleForCharacter;
        private Transform _playerHead;
        private Vector3 _previousAimLookAtUpdate;

        public Vector3 WorldPoint { get => _worldPoint; set => _worldPoint = value; }
        public GameObject Object { get => _object; set => _object = value; }
        public Vector3 CharacterLookAtPoint { get => _characterLookAtPoint; set => _characterLookAtPoint = value; }
        public RaycastHit WorldPointHit { get => _worldPointHit; set => _worldPointHit = value; }

        #endregion

        #region Functions

        #endregion

        #region Methods
        void Start()
        {
            _playerHead = _character.CharacterAnimator.Animator.GetBoneTransform(HumanBodyBones.Head);
        }
        void Update()
        {
            WorldPointUpdate();
            CharacterLevelPointUpdate();
            InputUpdate();
        }
       
        void WorldPointUpdate()
        {
            Ray ray = Camera.main.ScreenPointToRay(_pointerPosition.CursorPosition);
            if (Physics.Raycast(ray, out _worldPointHit, Mathf.Infinity, _pointerMask.LayerMask, QueryTriggerInteraction.Ignore))
            {
                _worldPoint = _worldPointHit.point;
                _object = _worldPointHit.collider.gameObject;
            }
            else if( _object != null)
            {
                _object = null;
            }     
        }
        void CharacterLevelPointUpdate()
        {
            Ray ray = Camera.main.ScreenPointToRay(_pointerPosition.CursorPosition);

            if (_object)
            {
                if (_object.CompareTag("Walkable Slope"))
                {
                    _playerGroundLevelPlane = new Plane(WorldPointHit.normal, _character.CharacterRoot.position);
                }
                else
                {
                    _playerGroundLevelPlane = new Plane(Vector3.up, _character.CharacterRoot.position);
                }
            }

            float enter = 0.0f;
            if (_playerGroundLevelPlane.Raycast(ray, out enter))
            {
                _characterLookAtPoint = ray.GetPoint(enter);
            }
        }
        void InputUpdate()
        {
            _character.Movement.InputCharacterLookAtPoint = CharacterLookAtPoint;


            if(_object && _object.TryGetComponent(out CharacterSystem.CharacterHitbox characterHitbox) && characterHitbox.Root == _character.CharacterRoot)
            {
                _character.WeaponSystemNode.Aiming.InputAimLookAt = CharacterLookAtPoint;
            }
            else
            {
                _character.WeaponSystemNode.Aiming.InputAimLookAt = WorldPoint;
            }

            //DISABLED 
            //InputAimLookAtUpdate();
            //void InputAimLookAtUpdate()
            //{
            //    Vector3 lookAt = (WorldPoint - _playerHead.position).normalized;
            //    Ray ray = new Ray(_playerHead.position, lookAt);
            //    Physics.Raycast(ray, out RaycastHit sightHit, Mathf.Infinity, _visibleForCharacter.LayerMask, QueryTriggerInteraction.Ignore);

            //    float distance = Vector3.Distance(WorldPoint, sightHit.point);

            //    if (_previousAimLookAtUpdate != WorldPoint)
            //    {
            //        _previousAimLookAtUpdate = WorldPoint;
            //        if (distance < 1.5f)
            //        {
            //            _character.WeaponSystemNode.Aiming.InputAimLookAt = WorldPoint;
            //        }
            //        else if (distance > 1.55f)
            //        {
            //            _character.WeaponSystemNode.Aiming.InputAimLookAt = CharacterLookAtPoint;
            //        }
            //    }
            //}
        }
        private void OnDrawGizmosSelected()
        {
           
            Vector3 p0 = _playerGroundLevelPlane.ClosestPointOnPlane(_character.CharacterRoot.position + new Vector3(-2, 0, -2));
            Vector3 p1 = _playerGroundLevelPlane.ClosestPointOnPlane(_character.CharacterRoot.position + new Vector3(-2, 0, 2));
            Vector3 p2 = _playerGroundLevelPlane.ClosestPointOnPlane(_character.CharacterRoot.position + new Vector3(2, 0, 2));
            Vector3 p3 = _playerGroundLevelPlane.ClosestPointOnPlane(_character.CharacterRoot.position + new Vector3(2, 0, -2));

            Gizmos.color = Color.green;

            Gizmos.DrawLine(p0, p1);
            Gizmos.DrawLine(p1, p2);
            Gizmos.DrawLine(p2, p3);
            Gizmos.DrawLine(p3, p0);

            if (_playerHead)
            {
                if (_character.WeaponSystemNode.Aiming.InputAimLookAt == WorldPoint)
                {
                    Gizmos.color = Color.green;
                    Gizmos.DrawLine(_playerHead.position, WorldPoint);
                }
                else if (_character.WeaponSystemNode.Aiming.InputAimLookAt == CharacterLookAtPoint)
                {
                    Gizmos.color = Color.red;
                    Gizmos.DrawLine(_playerHead.position, CharacterLookAtPoint);
                }
            }
        }
        #endregion
    }
}
