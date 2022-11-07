using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Project.Utilities;
using Project.SharedScripts;
using NaughtyAttributes;

namespace Project.WeaponsSystem
{
    [RequireComponent(typeof(BoxCollider))]
    [RequireComponent(typeof(Rigidbody))]
    public class WeaponWorldObject : MonoBehaviour, IInteractable
    {
        #region Temp
        //[Header("Temporary Things", order = 0)]
        #endregion

        #region Fields
        [Header("Fields", order = 1)]

        [HorizontalLine]
        [Header("Data")]

        [SerializeField] private WeaponConfig _weaponConfig;
        [SerializeField] private WeaponPersistantData _weaponPersistantData;

        [HorizontalLine]
        [Header("IK References")]

        [SerializeField] private Transform _leftHandGrip;
        [SerializeField] private Transform _rightHandGrip;
        [SerializeField] private Transform _visualFiringPoint;
        [SerializeField] private Transform _physicalFiringPoint;
        [SerializeField] private Transform _magazine;
        [SerializeField] private Transform _magazineInsertPoint;
        [SerializeField] private Transform _magazineHandGrip;

        [HorizontalLine]
        [Header("Physicial Functionalities")]
        [SerializeField] private BoxCollider _collider;
        [SerializeField] private Rigidbody _rigidbody;

        public WeaponConfig WeaponConfig { get => _weaponConfig; set => _weaponConfig = value; }
        public WeaponPersistantData WeaponPersistantData { get => _weaponPersistantData; set => _weaponPersistantData = value; }
        public Transform LeftHandGrip { get => _leftHandGrip; set => _leftHandGrip = value; }
        public Transform RightHandGrip { get => _rightHandGrip; set => _rightHandGrip = value; }
        public Transform VisualFiringPoint { get => _visualFiringPoint; set => _visualFiringPoint = value; }
        public Transform PhysicalFiringPoint { get => _physicalFiringPoint; set => _physicalFiringPoint = value; }
        public Transform Magazine { get => _magazine; set => _magazine = value; }
        public Transform MagazineInsertPoint { get => _magazineInsertPoint; set => _magazineInsertPoint = value; }
        public Transform MagazineHandGrip { get => _magazineHandGrip; set => _magazineHandGrip = value; }

        public string Description => throw new System.NotImplementedException();

        public Vector3 Position => transform.position;

        #endregion

        #region Functions

        #endregion

        #region Methods
        public virtual void StartShooting()
        {
            if(_weaponPersistantData.CurrentFireMode == SelectiveFire.Burst || _weaponPersistantData.CurrentFireMode == SelectiveFire.Single)
            {
                _weaponPersistantData.ProjectilesFired = 0;
            }
        }
        public virtual void StopShooting()
        {

        }
        public virtual void Shot(Transform shotOrigin, RaycastHit raycastHit)
        {
            _weaponPersistantData.CurrentAmmoCount--;
            if (_weaponPersistantData.CurrentFireMode == SelectiveFire.Burst || _weaponPersistantData.CurrentFireMode == SelectiveFire.Single)
            {
                _weaponPersistantData.ProjectilesFired++;
            }
        }

        public void Interact()
        {
         
        }
        public void Drop()
        {
           
;            _collider.enabled = true;
            _rigidbody.isKinematic = false;
            _collider.isTrigger = false;
            transform.parent = null;

            StartCoroutine(WaitForGroundedState());
            IEnumerator WaitForGroundedState()
            {
                yield return new WaitForSeconds(3f);
                if(_rigidbody.velocity.magnitude < 0.1f)
                {
                    _collider.isTrigger = true;
                    _rigidbody.isKinematic = true;
                   
                }
                else
                {
                   
                    StartCoroutine(WaitForGroundedState());
                }
               
            }
        }
        public void Equip()
        {
            _collider.enabled = false;
            _rigidbody.isKinematic = true;
        }
        #endregion
    }
}
