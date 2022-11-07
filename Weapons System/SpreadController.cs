using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;
using Project.CharacterSystem;

namespace Project.WeaponsSystem
{
    public class SpreadController : MonoBehaviour
    {
        #region Temp
        [Header("Temporary Things", order = 0)]
        [SerializeField] private float _debugSpreadLength;
        #endregion

        #region Fields
        [Header("Fields", order = 1)]
        [SerializeField] private Character _character;
          
        [OnValueChanged("ConeConfig")]
        [SerializeField] private float _maxSpread;
        [SerializeField] private Transform _spreadConeCenter;
        [SerializeField] private Transform[] _spreadConePoint;
        [SerializeField] private Transform[] _spreadConeLine;
        
        public float MaxSpread { get => _maxSpread; set => _maxSpread = value; }
        public Transform SpreadConeCenter { get => _spreadConeCenter; set => _spreadConeCenter = value; }
        public Transform[] SpreadConePoint { get => _spreadConePoint; set => _spreadConePoint = value; }

        #endregion

        #region Functions

        #endregion

        #region Methods
        void Start()
        {
            
        }
        void Update()
        {
            if (_character.WeaponSystemNode.WeaponWorldObject == null) return;
            ConePositionUpdate();
          
        }
        void ConePositionUpdate()
        {
            _spreadConeCenter.SetPositionAndRotation(_character.WeaponSystemNode.WeaponWorldObject.PhysicalFiringPoint.position, _character.WeaponSystemNode.WeaponWorldObject.PhysicalFiringPoint.rotation);
        }
        void ConeConfig()
        {
            float spreadFraction = _maxSpread / 10;
            for (int i = 0; i < _spreadConeLine.Length; i++)
            {
                for (int j = 0; j < _spreadConeLine[i].childCount; j++)
                {
                    float spreadForPoint = 0;
                    if (i % 2 == 0)
                    {
                        spreadForPoint = spreadFraction * (j+1);
                    }
                    else
                    {
                        spreadForPoint = (spreadFraction * j) + spreadFraction * 0.5f;
                    }
                    _spreadConeLine[i].GetChild(j).localEulerAngles = new Vector3(0, spreadForPoint, 0);
                }
            }
        }
        private void OnDrawGizmosSelected()
        {         
            //Gizmos.color = Color.red;
            //for (int i = 0; i < _spreadConePoint.Length; i++)
            //{
            //    Gizmos.DrawRay(_spreadConeCenter.position,_spreadConePoint[i].forward * _debugSpreadLength);
            //}
    }
        #endregion
    }
}
