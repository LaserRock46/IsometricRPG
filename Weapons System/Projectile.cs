using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Project.SharedScripts;
using Project.Utilities;

namespace Project.WeaponsSystem
{
    public class Projectile : MonoBehaviour
    {
        #region Temp
        //[Header("Temporary Things", order = 0)]
        #endregion

        #region Fields
        [Header("Fields", order = 1)]
        private WeaponConfig _weaponConfig;
        private RaycastHit _raycastHit;
        private IDamageable _damageable;
        private int _penetrationLeft;
        private bool _projectilePenetrated;

        private Stage _stage;
        private enum Stage { FirstSequence, Raycast, RepeatSequence, Dispose }

        private ObjectPoolReference _objectPoolReference;
        #endregion

        #region Functions

        #endregion

        #region Methods
        private void Start()
        {
            _objectPoolReference = GetComponent<ObjectPoolReference>();
        }
        public void Initialize(WeaponConfig weaponConfig, Vector3 startPosition, Quaternion startRotation, RaycastHit raycastHit)
        {
            transform.SetPositionAndRotation(startPosition, startRotation);
            if (raycastHit.collider != null)
            {
                _damageable = raycastHit.collider.GetComponent<IDamageable>();
            }
            _stage = Stage.FirstSequence;
            _weaponConfig = weaponConfig;
            _raycastHit = raycastHit;
        }
        void Update()
        {
            StateUpdate();
        }
        void StateUpdate()
        {
            switch (_stage)
            {
                case Stage.FirstSequence:
                    transform.position = Vector3.MoveTowards(transform.position, _raycastHit.point, _weaponConfig.ProjectileSpeed * Time.deltaTime);
                    if (transform.position == _raycastHit.point)
                    {
                        _stage = Stage.Dispose;
                        SpawnImpact();
                        DealDamege();
                    }
                    break;
                case Stage.Raycast:
                    // Ray...
                    _damageable = _raycastHit.collider.GetComponent<IDamageable>();
                    break;
                case Stage.RepeatSequence:
                    break;
                case Stage.Dispose:
                   _objectPoolReference.ReleaseThis();
                    _damageable = null;
                    break;
            }
        }
        void SpawnImpact()
        {
            if (_damageable != null)
            {
                if(_weaponConfig.ImpactEffectOnCharacterHit == true && _damageable.IsCharacterOrCreature == true || _damageable.IsCharacterOrCreature == false)
                {
                    Spawn();
                }
            }
            else
            {
                Spawn();
            }
            void Spawn()
            {
                _weaponConfig.ImpactEffect.Get(out ObjectPoolReference impact);
                impact.transform.position = _raycastHit.point + (_raycastHit.normal * 0.3f);              
            }
        }
        void DealDamege()
        {
            if (_damageable != null)
            {
                _damageable.DealDamage(_weaponConfig,_raycastHit,_penetrationLeft, out _projectilePenetrated);
            }
        }
        #endregion
    }
}
