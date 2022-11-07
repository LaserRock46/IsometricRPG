using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Project.WeaponsSystem;
using Project.Utilities;

namespace Project.CharacterSystem
{
    public class CharacterDamage : MonoBehaviour
    {
        #region Temp
        //[Header("Temporary Things", order = 0)]
        #endregion

        #region Fields
        [Header("Fields", order = 1)]
        [SerializeField] private Character _character;
        [SerializeField] private BloodSpawner _bloodSpawner;
        [SerializeField] private SharedScripts.TauntsSO _tauntsSO;
        #endregion

        #region Functions
        
        #endregion
        
        #region Methods
        void Start()
        {
            
        }
        void Update()
        {
            
        }

        public void DealDamage(WeaponConfig weaponConfig, RaycastHit raycastHit, int penetrationLeft, out bool projectilePenetrated, BodyPart bodyPart)
        {
            projectilePenetrated = true;
            SpawnBodyPenetrationEffect(weaponConfig,raycastHit);
            _tauntsSO.TauntsDisplay.EvokeTaunt(_tauntsSO, _character.CharacterRoot);
        }
        void SpawnBodyPenetrationEffect(WeaponConfig weaponConfig, RaycastHit raycastHit)
        {
            if(_character.Stats.CreatureType == SharedScripts.CreatureType.HumanCharacter)
            {
                if(weaponConfig.LifeFormsPenetrationEffect == BodyPenetrationEffect.Blood)
                {
                    _bloodSpawner.Spawn(raycastHit, _character.CharacterRoot);
                }
                else if (weaponConfig.LifeFormsPenetrationEffect == BodyPenetrationEffect.Fire)
                {

                }
            }
            else if(_character.Stats.CreatureType == SharedScripts.CreatureType.RobotCharacter)
            {
                if (weaponConfig.LifeFormsPenetrationEffect == BodyPenetrationEffect.Sparks)
                {

                }
                else if (weaponConfig.LifeFormsPenetrationEffect == BodyPenetrationEffect.Fire)
                {

                }
            }
        }
        #endregion
    }
}
