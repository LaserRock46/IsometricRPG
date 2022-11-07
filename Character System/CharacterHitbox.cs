using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Project.SharedScripts;
using Project.WeaponsSystem;

namespace Project.CharacterSystem
{
    public class CharacterHitbox : MonoBehaviour, IDamageable
    {
        #region Temp
        //[Header("Temporary Things", order = 0)]
        #endregion

        #region Fields
        [Header("Fields", order = 1)]
        [SerializeField] private Character _character;
        [SerializeField] private BodyPart _bodyPart;

        public Character Character { get => _character; private set => _character = value; }
        public Transform Root { get => _character.CharacterRoot; set => Root = value; }
        public bool IsCharacterOrCreature { get =>true;}
        public Transform Head { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }


        #endregion

        #region Functions


        #endregion

        #region Methods
        public void DealDamage(WeaponConfig weaponConfig,RaycastHit raycastHit, int penetrationLeft, out bool projectilePenetrated)
        {
            _character.CharacterDamage.DealDamage(weaponConfig, raycastHit, penetrationLeft, out projectilePenetrated, _bodyPart);
           
        }

        #endregion
    }
}
