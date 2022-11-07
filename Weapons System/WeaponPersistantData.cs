using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Project.WeaponsSystem
{
    [System.Serializable]
    public class WeaponPersistantData
    {
        [SerializeField] private int _currentAmmoCount;
        [SerializeField] private SelectiveFire _currentFireMode;
        [SerializeField] private int _projectilesFired;

        public int CurrentAmmoCount { get => _currentAmmoCount; set => _currentAmmoCount = value; }
        public SelectiveFire CurrentFireMode { get => _currentFireMode; set => _currentFireMode = value; }
        public int ProjectilesFired { get => _projectilesFired; set => _projectilesFired = value; }
    }
}
