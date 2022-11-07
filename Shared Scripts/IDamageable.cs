using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Project.WeaponsSystem;

namespace Project.SharedScripts
{
    public interface IDamageable
    {
        public void DealDamage(WeaponConfig weaponConfig, RaycastHit raycastHit, int penetrationLeft, out bool projectilePenetrated);
        public Transform Root { get; set; }
        public Transform Head { get; set; }
        public bool IsCharacterOrCreature { get; }
    }
}
