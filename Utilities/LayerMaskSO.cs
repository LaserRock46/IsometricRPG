using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Project.Utilities
{
    [CreateAssetMenu(fileName = "New Layer Mask", menuName = "Project/Utilities/Layer Mask")]
    [System.Serializable]
    public class LayerMaskSO : ScriptableObject
    {
        [SerializeField] private LayerMask _layerMask;

        public LayerMask LayerMask { get => _layerMask; private set => _layerMask = value; }
    }
}
