using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;
using Project.SharedScripts;

namespace Project.CharacterSystem
{
    public class InteractableDetector : MonoBehaviour
    {
        #region Temp
        //[Header("Temporary Things", order = 0)]
        #endregion

        #region Fields
        [Header("Fields", order = 1)]
        [SerializeField] private Character _character;

        [HorizontalLine]

        [SerializeField][ReadOnly] private IInteractable _interactable;
        private List<IInteractable> _interactablesInRange = new List<IInteractable>();
 


        public IInteractable Interactable { get => _interactable; set => _interactable = value; }

        #endregion

        #region Functions

        #endregion

        #region Methods
        private void OnTriggerEnter(Collider other)
        {
            if(other.TryGetComponent(out IInteractable interactable))
            {
                _interactablesInRange.Add(interactable);
                GetNearestInteractable();
            }
        }
        private void OnTriggerExit(Collider other)
        {
            if (other.TryGetComponent(out IInteractable interactable))
            {
                _interactablesInRange.Remove(interactable);
                GetNearestInteractable();
            }
        }
        void GetNearestInteractable()
        {
            if (_interactablesInRange.Count != 0)
            {
                float nearestDistance = Mathf.Infinity;
                for (int i = 0; i < _interactablesInRange.Count; i++)
                {
                    float distance = Vector3.Distance(_character.CharacterRoot.position, _interactablesInRange[i].Position);
                    if (distance < nearestDistance)
                    {
                        _interactable = _interactablesInRange[i];
                        nearestDistance = distance;
                       
                    }
                }
            }
            else
            {
                _interactable = null;
            }
        }
        public void InputInteract()
        {
            _interactable.Interact();
        }
        #endregion
    }
}
