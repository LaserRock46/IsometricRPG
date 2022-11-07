using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Project.CharacterSystem
{
    public class AnimatorCallbacks : MonoBehaviour
    {
        #region Temp
        //[Header("Temporary Things", order = 0)]
        #endregion

        #region Fields
        //[Header("Fields", order = 1)]

        public delegate void AnimatorMove();
        public event AnimatorMove OnAnimatorMoveCallback;
        public delegate void AnimatorIK();
        public event AnimatorIK OnAnimatorIKCallback;
        #endregion

        #region Functions

        #endregion

        #region Methods
        private void OnAnimatorMove()
        {
            OnAnimatorMoveCallback?.Invoke();
        }
        private void OnAnimatorIK(int layerIndex)
        {
            OnAnimatorIKCallback?.Invoke();
        }
        #endregion
    }
}
