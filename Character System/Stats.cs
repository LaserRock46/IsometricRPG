using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;
using Project.SharedScripts;

namespace Project.CharacterSystem
{
    public class Stats : MonoBehaviour
    {
        [System.Serializable]
        public class PersistentData
        {            
            [Dropdown(nameof(GetCharacterCreatureType))] public CreatureType creatureType;
            private CreatureType[] GetCharacterCreatureType()
            {
                return new CreatureType[] { CreatureType.HumanCharacter, CreatureType.RobotCharacter };
            }
        }
        #region Temp
        //[Header("Temporary Things", order = 0)]
        #endregion

        #region Fields
        [Header("Fields", order = 1)]
        [SerializeField] private PersistentData _persistentData;
        public PersistentData StatsPersistentData { get => _persistentData; set => _persistentData = value; }

        public CreatureType CreatureType { get => _persistentData.creatureType; set => _persistentData.creatureType = value; }
        #endregion

        #region Functions

        #endregion

        #region Methods

        #endregion
    }


}
