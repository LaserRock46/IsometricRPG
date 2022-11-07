using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

namespace Project.CharacterSystem
{
    public partial class ArmedUnarmedStateSwitch : MonoBehaviour
    {
        #region Temp
        //[Header("Temporary Things", order = 0)]
        #endregion

        #region Fields
        [Header("Fields", order = 1)]
        [SerializeField] private Character _character;

        [HorizontalLine]

        [SerializeField] private State _state;
        public State State { get => _state; set => _state = value; }
       
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
        private void OnEnable()
        {
            _character.RagdollController.OnRagdollEnabled += ToUnarmedAfterFall;
        }
        private void OnDisable()
        {
            _character.RagdollController.OnRagdollEnabled -= ToUnarmedAfterFall;
        }
        public void InputDrawWeapon()
        {

        }
        void ToUnarmedAfterFall()
        {
            if(State == State.Armed)
            {
                State = State.Unarmed;

                _character.WeaponSystemNode.WeaponWorldObject.Drop();
                _character.WeaponSystemNode.WeaponWorldObject = null;
                _character.CharacterAnimator.SetUnarmedRifleDown(true);
            }
        }
        #endregion
    }
}
