using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using NaughtyAttributes;

namespace Project.Utilities
{
    [CreateAssetMenu(fileName = "New Object Pool", menuName = "Project/Utilities/Object Pool")]
    [System.Serializable]
    public class ObjectPoolSO : ScriptableObject
    {
        #region Temp
        //[Header("Temporary Things", order = 0)]
        #endregion

        #region Fields
        [Header("Fields", order = 1)]       
        [SerializeField] private GameObject[] _randomPrefabs;
        private ObjectPool<ObjectPoolReference> _objectPool;
        private List<ObjectPoolReference> _used;
        [SerializeField] private int _defaultCapacity;
        [SerializeField] private int _maxSize;
        private Transform _parent;
        public Transform Parent { get => _parent; private set => _parent = value; }

        #endregion

        #region Functions
        ObjectPoolReference CreatePooledPrefab()
        {
            GameObject randomPrefab = Instantiate(_randomPrefabs[Random.Range(0,_randomPrefabs.Length)]);
            randomPrefab.transform.SetParent(_parent);
            //randomPrefab.name = _parent.childCount.ToString();
            //randomPrefab.SetActive(false);
            
            ObjectPoolReference objectPoolReference = randomPrefab.AddComponent<ObjectPoolReference>();
            objectPoolReference.ObjectPool = this;          
           
            return objectPoolReference;
        }
       
        #endregion

        #region Methods
        private void OnEnable()
        {
            Actions.OnSceneLoad += Initialize;
        }
        private void OnDisable()
        {
            Actions.OnSceneLoad -= Initialize;
        }
      
        void Initialize()
        {
            if (_parent != null) return;
           
            _parent = new GameObject(name).transform;
            _used.Clear();
            _objectPool = new ObjectPool<ObjectPoolReference>(CreatePooledPrefab, OnTakeFromPool, OnReturnedToPool, OnDestroyPoolObject, true, _defaultCapacity, _maxSize);
           
        }     
        public void Get(out ObjectPoolReference prefab)
        {
            if(_objectPool.CountActive == _maxSize)
            {
                _objectPool.Release(_used[0]);
                _used.RemoveAt(0);
            }

            prefab = _objectPool.Get();
            _used.Add(prefab);
        }
        public void Release(ObjectPoolReference prefab)
        {
            _objectPool.Release(prefab);
            _used.Remove(prefab);
           
        }

        void OnReturnedToPool(ObjectPoolReference prefab)
        {
                prefab.transform.SetParent(_parent);
            if (prefab.gameObject.activeSelf)
            {
                prefab.gameObject.SetActive(false);
            }
        }
        void OnTakeFromPool(ObjectPoolReference prefab)
        {          
            prefab.gameObject.SetActive(true);
        }   
        void OnDestroyPoolObject(ObjectPoolReference prefab)
        {
            Destroy(prefab);
        }
        #endregion
    }
}
