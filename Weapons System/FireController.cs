using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Project.CharacterSystem;
using NaughtyAttributes;

namespace Project.WeaponsSystem
{
    public class FireController : MonoBehaviour
    {
        #region Temp
        //[Header("Temporary Things", order = 0)]
        #endregion

        #region Fields
        [Header("Fields", order = 1)]
        [SerializeField] private Character _character;

        [SerializeField] [ReadOnly] private bool _roundLoaded;
        [SerializeField] [ReadOnly] private float _shootingTimeLeft;

        [SerializeField] [ReadOnly] private bool _reloading;
        [SerializeField] [ReadOnly] private float _reloadingProgress;

        [SerializeField][ReadOnly]private bool _isFiring;
        public bool IsFiring { get => _isFiring; private set => _isFiring = value; }

        private RaycastHit _shotRaycastHit;

        private bool _triggerPress;
        private bool _triggerRelease;  
        public bool InputReloadd { get; set; }
        
        #endregion

        #region Functions
        bool StartFiring()
        {
            if (_isFiring == true) return false;
            if (_triggerPress == false) return false;
            if (_character.WeaponSystemNode.WeaponWorldObject.WeaponPersistantData.CurrentAmmoCount == 0) return false;

             return true;
        }
        bool StopFiring()
        {
            if (_isFiring)
            {
                if (_triggerRelease == true)
                {
                    if (_character.WeaponSystemNode.WeaponWorldObject.WeaponPersistantData.CurrentFireMode == SelectiveFire.Automatic)
                    {                    
                        return true;
                    }
                }

                if (_character.WeaponSystemNode.WeaponWorldObject.WeaponPersistantData.CurrentFireMode == SelectiveFire.Burst)
                {
                    if (_character.WeaponSystemNode.WeaponWorldObject.WeaponPersistantData.ProjectilesFired == _character.WeaponSystemNode.WeaponWorldObject.WeaponConfig.BurstShotsCount)
                    {
                        return true;
                    }
                }

                if (_character.WeaponSystemNode.WeaponWorldObject.WeaponPersistantData.CurrentFireMode == SelectiveFire.Single)
                {
                    if (_character.WeaponSystemNode.WeaponWorldObject.WeaponPersistantData.ProjectilesFired == 1)
                    {
                        return true;
                    }
                }

                if (_character.WeaponSystemNode.WeaponWorldObject.WeaponPersistantData.CurrentAmmoCount == 0)
                {
                    return true;
                }
            }
            return false;
        }
        #endregion

        #region Methods
        void Start()
        {
            
        }
        void Update()
        {
            if (_character.WeaponSystemNode.WeaponWorldObject == null) return;

          
            FiringStatusUpdate();
            ShotUpdate();
            RoundLoadingUpdate();
            WeaponReloadingUpdate();
        }

        public void InputTriggerPress()
        {
            if (StartFiring())
            {
                _triggerPress = true;
            }
        }
        public void InputTriggerRelease()
        {
            if (StopFiring())
            {
                _triggerRelease = true;
            }       
        }
        void FiringStatusUpdate()
        {
            if (StartFiring())
            {
                _isFiring = true;
                _triggerPress = false;
                _character.WeaponSystemNode.WeaponWorldObject.StartShooting();
            }
            if (StopFiring())
            {
                _isFiring = false;
                _triggerRelease = false;
                _character.WeaponSystemNode.WeaponWorldObject.StopShooting();
            }

        }
        void ShotUpdate()
        {
            if (_roundLoaded == true && IsFiring == true)
            {
                _roundLoaded = false;
         
                int randomHitscanIndex = Random.Range(0, _character.WeaponSystemNode.WeaponHitscan.Hitscans.Length);
                _shotRaycastHit = _character.WeaponSystemNode.WeaponHitscan.Hitscans[randomHitscanIndex];
                Transform shotOrigin = _character.WeaponSystemNode.SpreadController.SpreadConePoint[randomHitscanIndex];
               
                _character.WeaponSystemNode.WeaponWorldObject.Shot(shotOrigin,_shotRaycastHit);
            }
        }
        void RoundLoadingUpdate()
        {
            if (_reloading == false && _character.WeaponSystemNode.WeaponWorldObject.WeaponPersistantData.CurrentAmmoCount > 0)
            {
                if (_isFiring == true || _roundLoaded == false)
                {
                    _shootingTimeLeft -= Time.deltaTime;
                    if ((_character.WeaponSystemNode.WeaponWorldObject.WeaponPersistantData.CurrentAmmoCount - 1) * _character.WeaponSystemNode.WeaponWorldObject.WeaponConfig.RateOfFire >= _shootingTimeLeft)
                    {
                        _roundLoaded = true;
                    }
                }
            }
        }
        void WeaponReloadingUpdate()
        {         
            if (InputReloadd && _reloading == false)
            {
                _reloading = true;
                _roundLoaded = false;
            }
            if (_reloading == true)
            {
                if (_reloadingProgress < _character.WeaponSystemNode.WeaponWorldObject.WeaponConfig.ReloadTime)
                {
                    _reloadingProgress += Time.deltaTime;
                }
                else
                {
                    _reloading = false;
                    _reloadingProgress = 0;
                    int ammoAmount = _character.WeaponSystemNode.WeaponWorldObject.WeaponConfig.MagazineCapacity;
                    _character.WeaponSystemNode.WeaponWorldObject.WeaponPersistantData.CurrentAmmoCount = ammoAmount;
                    _shootingTimeLeft = ammoAmount * _character.WeaponSystemNode.WeaponWorldObject.WeaponConfig.RateOfFire;
                }
            }
        }
        public void InputReload()
        {
            if (_reloading == false)
            {
                _reloading = true;
                _roundLoaded = false;

                StartCoroutine(WaitForReload());
            }
            IEnumerator WaitForReload()
            {
                yield return new WaitForSeconds(_character.WeaponSystemNode.WeaponWorldObject.WeaponConfig.ReloadTime);

                _reloading = false;

                int ammoAmount = _character.WeaponSystemNode.WeaponWorldObject.WeaponConfig.MagazineCapacity;
                _character.WeaponSystemNode.WeaponWorldObject.WeaponPersistantData.CurrentAmmoCount = ammoAmount;
                _shootingTimeLeft = ammoAmount * _character.WeaponSystemNode.WeaponWorldObject.WeaponConfig.RateOfFire;
            }
        }
        public void InputFireMode()
        {
            switch (_character.WeaponSystemNode.WeaponWorldObject.WeaponPersistantData.CurrentFireMode)
            {
                case SelectiveFire.Single:
                    if (_character.WeaponSystemNode.WeaponWorldObject.WeaponConfig.SelectiveFire.HasFlag(SelectiveFire.Burst))
                    {
                        _character.WeaponSystemNode.WeaponWorldObject.WeaponPersistantData.CurrentFireMode = SelectiveFire.Burst;
                    }
                    else if (_character.WeaponSystemNode.WeaponWorldObject.WeaponConfig.SelectiveFire.HasFlag(SelectiveFire.Automatic))
                    {
                        _character.WeaponSystemNode.WeaponWorldObject.WeaponPersistantData.CurrentFireMode = SelectiveFire.Automatic;
                    }
                    break;
                case SelectiveFire.Burst:
                    if (_character.WeaponSystemNode.WeaponWorldObject.WeaponConfig.SelectiveFire.HasFlag(SelectiveFire.Automatic))
                    {
                        _character.WeaponSystemNode.WeaponWorldObject.WeaponPersistantData.CurrentFireMode = SelectiveFire.Automatic;
                    }
                    else if (_character.WeaponSystemNode.WeaponWorldObject.WeaponConfig.SelectiveFire.HasFlag(SelectiveFire.Single))
                    {
                        _character.WeaponSystemNode.WeaponWorldObject.WeaponPersistantData.CurrentFireMode = SelectiveFire.Single;
                    }
                    break;
                case SelectiveFire.Automatic:
                    if (_character.WeaponSystemNode.WeaponWorldObject.WeaponConfig.SelectiveFire.HasFlag(SelectiveFire.Single))
                    {
                        _character.WeaponSystemNode.WeaponWorldObject.WeaponPersistantData.CurrentFireMode = SelectiveFire.Single;
                    }
                    else if (_character.WeaponSystemNode.WeaponWorldObject.WeaponConfig.SelectiveFire.HasFlag(SelectiveFire.Burst))
                    {
                        _character.WeaponSystemNode.WeaponWorldObject.WeaponPersistantData.CurrentFireMode = SelectiveFire.Burst;
                    }
                    break;
            }
        }
        private void OnDrawGizmosSelected()
        {
            if (_triggerPress == true && _shootingTimeLeft > 0 && _shotRaycastHit.collider != null)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawLine(_character.WeaponSystemNode.SpreadController.SpreadConeCenter.position, _shotRaycastHit.point);
            }
        }
        #endregion
    }
}
