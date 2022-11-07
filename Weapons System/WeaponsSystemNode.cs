using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Project.WeaponsSystem
{
    public class WeaponsSystemNode : MonoBehaviour
    {
         #region Temp
        //[Header("Temporary Things", order = 0)]
        #endregion

        #region Fields
        [Header("Fields", order = 1)]
        [SerializeField] private Aiming _aiming;
        [SerializeField] private SpreadController _spreadController;
        [SerializeField] private WeaponHitscan _weaponHitscan;
        [SerializeField] private WeaponWorldObject _weaponWorldObject;
        [SerializeField] private FireController _fireController;
        [SerializeField] private DrawAndHolsterAnimator _drawAndHolsterAnimator;

        public Aiming Aiming { get => _aiming; private set => _aiming = value; }
        public SpreadController SpreadController { get => _spreadController; private set => _spreadController = value; }
        public WeaponHitscan WeaponHitscan { get => _weaponHitscan; private set => _weaponHitscan = value; }
        public WeaponWorldObject WeaponWorldObject { get => _weaponWorldObject; set => _weaponWorldObject = value; }
        public FireController FireController { get => _fireController; private set => _fireController = value; }
        public DrawAndHolsterAnimator DrawAndHolsterAnimator { get => _drawAndHolsterAnimator; private set => _drawAndHolsterAnimator = value; }
        #endregion

        #region Functions

        #endregion

        #region Methods

        #endregion
    }
}
