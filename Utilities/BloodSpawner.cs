using System.Collections;
using System.Collections.Generic;
using UnityEngine; 

namespace Project.Utilities
{
    [CreateAssetMenu(fileName = "New Blood Spawner", menuName = "Project/Utilities/Blood Spawner")]
    [System.Serializable]
    public class BloodSpawner : ScriptableObject
    {
        #region Temp
        //[Header("Temporary Things", order = 0)]
        #endregion

        #region Fields
        [Header("Fields", order = 1)]
        [SerializeField] private ObjectPoolSO _bloodAttach;
        [SerializeField] private ObjectPoolSO _bloodFX;
        [SerializeField] private float _lightIntensityMultiplier;

        #endregion

        #region Functions
        Transform GetNearestObject(Transform hit, Vector3 hitPos)
        {
            float closestPos = 100f;
            Transform closestBone = null;
            Transform[] childs = hit.GetComponentsInChildren<Transform>();

            foreach (var child in childs)
            {
                var dist = Vector3.Distance(child.position, hitPos);
                if (dist < closestPos)
                {
                    closestPos = dist;
                    closestBone = child;
                }
            }

            var distRoot = Vector3.Distance(hit.position, hitPos);
            if (distRoot < closestPos)
            {
                closestPos = distRoot;
                closestBone = hit;
            }
            return closestBone;
        }
        #endregion

        #region Methods
        public void Spawn(RaycastHit hit, Transform characterRoot)
        {
            float angle = Mathf.Atan2(hit.normal.x, hit.normal.z) * Mathf.Rad2Deg + 180;
            _bloodFX.Get(out ObjectPoolReference instance);
            instance.transform.SetPositionAndRotation(hit.point, Quaternion.Euler(0, angle + 90, 0));
            BFX_BloodSettings settings = instance.GetComponent<BFX_BloodSettings>();
            settings.LightIntensityMultiplier = _lightIntensityMultiplier;
            settings.GroundHeight = characterRoot.position.y;

            Transform nearestBone = GetNearestObject(hit.transform.root, hit.point);
            if (nearestBone != null)
            {              
                _bloodAttach.Get(out ObjectPoolReference attachBloodInstance);
                Transform bloodT = attachBloodInstance.transform;
                bloodT.position = hit.point;
                bloodT.localRotation = Quaternion.identity;
                bloodT.localScale = Vector3.one * Random.Range(0.75f, 1.2f);
                bloodT.LookAt(hit.point + hit.normal, Vector3.up);
                bloodT.Rotate(90, 0, 0);
                bloodT.transform.parent = nearestBone;               
            }
        }
        #endregion
    }
}
