using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;
using Project.Utilities;
using Project.SharedScripts;

namespace Project.WeaponsSystem
{
    public class WeaponHitscan : MonoBehaviour
    {
        #region Temp
        //[Header("Temporary Things", order = 0)]
        #endregion

        #region Fields
        [Header("Fields", order = 1)]
        [SerializeField] private CharacterSystem.Character _character;
        [SerializeField] private LayerMaskSO _visibleForCharacter;
        [SerializeField] private float _centerHitSphereRadius;
        private RaycastHit _centerHit;
        private Vector3 _centerHitPoint;
        public Vector3 HitPoint { get => _centerHitPoint; set => _centerHitPoint = value; }

        [SerializeField] private RaycastHit[] _hitscans;
        public RaycastHit[] Hitscans { get => _hitscans; set => _hitscans = value; }

        [HorizontalLine]

        [SerializeField][Tag] private string _traceableTag;
        [SerializeField] private List<GameObject> _possibleTargets = new List<GameObject>();
        [SerializeField] private List<int> _possibleTargetsHitchance = new List<int>();


        public List<GameObject> PossibleTargets { get => _possibleTargets; private set => _possibleTargets = value; }
        public List<int> PossibleTargetsHitchance { get => _possibleTargetsHitchance; private set => _possibleTargetsHitchance = value; }

        #endregion
        GameObject GetPossibleTarget(GameObject hitObject)
        {
            if (hitObject.TryGetComponent(out IDamageable damageable))
            {
                if (damageable.Root != _character.CharacterRoot)
                {
                    return damageable.Root.gameObject;
                }
                else return null;
            }
            else if (hitObject.CompareTag("Traceable"))
            {
                return hitObject;
            }
            

            return null;
        }
        #region Functions

        #endregion

        #region Methods
        void Start()
        {
            _hitscans = new RaycastHit[_character.WeaponSystemNode.SpreadController.SpreadConePoint.Length];
        }
        void Update()
        {
            if (_character.WeaponSystemNode.WeaponWorldObject == null) return;

            CenterHitscan();
            Hitscan();
        }
        void CenterHitscan()
        {          
            if(Physics.SphereCast(
                _character.WeaponSystemNode.SpreadController.SpreadConeCenter.position,
                _centerHitSphereRadius,
                _character.WeaponSystemNode.SpreadController.SpreadConeCenter.forward,
                out _centerHit,
                Mathf.Infinity,
                _visibleForCharacter.LayerMask,
                QueryTriggerInteraction.Ignore))
            {
                _centerHitPoint = _centerHit.point;           
            }
        }
        void Hitscan()
        {
            if (_character.WeaponSystemNode.SpreadController.SpreadConeCenter.hasChanged == false) return;
            _character.WeaponSystemNode.SpreadController.SpreadConeCenter.hasChanged = false;

            _possibleTargets.Clear();
            _possibleTargetsHitchance.Clear();

            Vector3 origin = _centerHit.distance < 1.2f ? _character.WeaponSystemNode.WeaponWorldObject.PhysicalFiringPoint.position : _character.WeaponSystemNode.WeaponWorldObject.VisualFiringPoint.position;

            for (int i = 0; i < _character.WeaponSystemNode.SpreadController.SpreadConePoint.Length; i++)
            {
                if (Physics.Raycast(origin, _character.WeaponSystemNode.SpreadController.SpreadConePoint[i].forward, out _hitscans[i], Mathf.Infinity, _visibleForCharacter.LayerMask, QueryTriggerInteraction.Ignore))
                {
                    GameObject possibleTarget = GetPossibleTarget(_hitscans[i].collider.gameObject);
                    if (possibleTarget != null)
                    {                       
                        if (_possibleTargets.Contains(possibleTarget))
                        {
                            int indexOfTarget = _possibleTargets.IndexOf(possibleTarget);
                            _possibleTargetsHitchance[indexOfTarget]++;
                        }
                        else
                        {
                            _possibleTargets.Add(possibleTarget);
                            _possibleTargetsHitchance.Add(1);
                        }
                    }                  
                }
                else
                {
                    _hitscans[i].point = _character.WeaponSystemNode.SpreadController.SpreadConeCenter.position + _character.WeaponSystemNode.SpreadController.SpreadConePoint[i].forward * 1000;
                }
            }

        }
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(_character.WeaponSystemNode.SpreadController.SpreadConeCenter.position, _centerHitPoint);
        }
        #endregion
    }
}
