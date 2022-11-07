using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Project.SharedScripts;

namespace Project.Environment
{
    public class LadderTrigger : MonoBehaviour, IInteractable
    {


        #region Temp
        //[Header("Temporary Things", order = 0)]
        #endregion

        #region Fields
        [Header("Fields", order = 1)]
        [SerializeField] private string _description;
        public string Description => _description;
        public Vector3 Position => transform.position;
        #endregion

        #region Functions

        #endregion

        #region Methods    
        public void Interact()
        {
            throw new System.NotImplementedException();
        }
        #endregion
    }
}
