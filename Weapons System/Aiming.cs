using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using NaughtyAttributes;
using Project.CharacterSystem;

namespace Project.WeaponsSystem
{
    public class Aiming : MonoBehaviour
    {
        #region Temp
        //[Header("Temporary Things", order = 0)]

        #endregion

        #region Fields
        [Header("Fields", order = 1)]
        [SerializeField] private Character _character;

        [HorizontalLine]

        [SerializeField] private Transform _rigWeaponPivot;
        [SerializeField] private Transform _aimLookAt;
        public Transform AimLookAt { get => _aimLookAt; set => _aimLookAt = value; }

        [HorizontalLine]
        
        [SerializeField] private AnimationCurve _aimSpeedOverRotationDifference;
        [SerializeField] private AnimationCurve _aimSpeedOverMoveSpeed;
        [SerializeField][ReadOnly] private float _aimSpeed;

        Vector2 _anglesBetweenTransformAndInputAimPoint;
        public Vector2 AnglesBetweenTransformAndInputAimPoint { get => _anglesBetweenTransformAndInputAimPoint; set => _anglesBetweenTransformAndInputAimPoint = value; }

        [HorizontalLine]


        Quaternion _targetLookAtRotation;       
        Quaternion _currentLookAtRotation;
        Quaternion _nextLookAtRotation;

      
        public Vector3 InputAimLookAt { get; set; }
     

        #endregion

        #region Functions
        public static Vector2 GetAnglesBetweenTransformAndAimPoint(Transform relativeTransform, Vector3 aimPoint)
        {
            Vector3 relative = relativeTransform.InverseTransformPoint(aimPoint);
            float vertical = Mathf.Atan2(relative.y, relative.z) * Mathf.Rad2Deg;
            float horizontal = Mathf.Atan2(relative.x, relative.z) * Mathf.Rad2Deg;
                      
            return new Vector2(vertical, horizontal);
        }
        #endregion

        #region Methods
        void Start()
        {
                  
        }
        void Update()
        {
            AimUpdate();
            AimSpeedConstraintUpdate();
        }

        void AimSpeedConstraintUpdate()
        {
            _anglesBetweenTransformAndInputAimPoint = GetAnglesBetweenTransformAndAimPoint(_character.CharacterRoot, InputAimLookAt);
            float currentAimSpeedOverRotationDifference = _aimSpeedOverRotationDifference.Evaluate(Mathf.Abs(_anglesBetweenTransformAndInputAimPoint.y));
            _aimSpeed = currentAimSpeedOverRotationDifference * _aimSpeedOverMoveSpeed.Evaluate(_character.Movement.DirectionAndMoveGear.magnitude);
        }

        void AimUpdate()
        {
            _targetLookAtRotation = Quaternion.LookRotation((InputAimLookAt - _rigWeaponPivot.position).normalized);
            _currentLookAtRotation = Quaternion.LookRotation((_aimLookAt.position - _rigWeaponPivot.position).normalized);     
            _nextLookAtRotation = Quaternion.Slerp(_currentLookAtRotation, _targetLookAtRotation, Time.deltaTime * _aimSpeed);
            _aimLookAt.position = _rigWeaponPivot.position + (_nextLookAtRotation * Vector3.forward);

        }
        private void OnDrawGizmosSelected()
        {
           
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(_aimLookAt.position, 0.1f);
            
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(_rigWeaponPivot.position + (_targetLookAtRotation * new Vector3(0, 0, 1.2f)), 0.1f);
        }
        #endregion
    }
}
