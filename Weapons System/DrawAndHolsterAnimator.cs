using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using UnityEngine.Splines;
using Project.AnimationRiggingExtension;
using NaughtyAttributes;

namespace Project.WeaponsSystem
{
    public class DrawAndHolsterAnimator : MonoBehaviour
    {
        #region Temp
        //[Header("Temporary Things", order = 0)]
        #endregion

        #region Fields
        [Header("Fields", order = 1)]
        [SerializeField] private CharacterSystem.Character _character;

        [HorizontalLine]

        [SerializeField] private Transform _rigRightHandGrip;
        [SerializeField] private Transform _rigLeftHandGrip;

        //[HorizontalLine]
        //public enum State {Draw, Holster, }

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
        void SetTargetHandsGrip()
        {
            _rigLeftHandGrip.localPosition = _character.WeaponSystemNode.WeaponWorldObject.LeftHandGrip.position;
            _rigLeftHandGrip.localRotation = _character.WeaponSystemNode.WeaponWorldObject.LeftHandGrip.localRotation;
            _rigRightHandGrip.localPosition = _character.WeaponSystemNode.WeaponWorldObject.RightHandGrip.position;
            _rigRightHandGrip.localRotation = _character.WeaponSystemNode.WeaponWorldObject.RightHandGrip.localRotation;
        } 
        
        #endregion
    }
}
