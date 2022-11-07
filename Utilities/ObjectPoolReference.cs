using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;


namespace Project.Utilities
{
    public class ObjectPoolReference : MonoBehaviour
    {
        #region Temp
        //[Header("Temporary Things", order = 0)]
        #endregion

        #region Fields
        [Header("Fields", order = 1)]
        [SerializeField] private ObjectPoolSO _objectPool;

        public ObjectPoolSO ObjectPool { get => _objectPool; set => _objectPool = value; }
        #endregion

        #region Functions

        #endregion

        #region Methods   
        public void ReleaseThis()
        {
            transform.SetParent(_objectPool.Parent);
            _objectPool.Release(this);
        }
        //private void OnParticleSystemStopped()
        //{
        //    ReleaseThis();
        //    Debug.Log("Release");
        //}
        #endregion
    }
}
