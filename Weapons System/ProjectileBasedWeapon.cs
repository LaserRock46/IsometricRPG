using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Project.Utilities;

namespace Project.WeaponsSystem
{
    public class ProjectileBasedWeapon : WeaponWorldObject
    {
        #region Temp
        //[Header("Temporary Things", order = 0)]
        #endregion

        #region Fields
        [Header("Fields", order = 1)]
        [SerializeField] private ObjectPoolSO _projectile;

        private WaitForSeconds _muzzleFlashTime = new WaitForSeconds(0.075f);
        #endregion

        #region Functions

        #endregion

        #region Methods
        void Start()
        {
          
        }
        void Update()
        {
            
        }
        public override void StartShooting()
        {
            base.StartShooting();

        }
        public override void StopShooting()
        {
            base.StopShooting();

        }
        public override void Shot(Transform shotOrigin, RaycastHit raycastHit)
        {
            base.Shot(shotOrigin, raycastHit);
            _projectile.Get(out ObjectPoolReference prefab);
            Projectile projectile = prefab.GetComponent<Projectile>();
            projectile.Initialize(WeaponConfig, shotOrigin.position, shotOrigin.rotation, raycastHit);
            MuzzleFlash();
            ShotSound();
        }
        void MuzzleFlash()
        {
            if (WeaponConfig.EnableMuzzleFlash == false) return;

            WeaponConfig.MuzzleFlash.Get(out ObjectPoolReference flash);

            Vector3 cachedLocalScale = flash.transform.localScale;
            flash.transform.parent = VisualFiringPoint;
            flash.transform.localScale = cachedLocalScale * Random.Range(cachedLocalScale.magnitude * 1.2f, cachedLocalScale.magnitude * 0.8f);
            flash.transform.localPosition = Vector3.zero;
            flash.transform.localEulerAngles = new Vector3(0, 0, Random.Range(0, 360));
           
           
            StartCoroutine(FlashTimer(flash,cachedLocalScale));

            IEnumerator FlashTimer(ObjectPoolReference flash, Vector3 cacheLocalScale)
            {
                yield return _muzzleFlashTime;
                flash.transform.localScale = cacheLocalScale;
                flash.ReleaseThis();
            }
        }
        void ShotSound()
        {
            WeaponConfig.AudioSourcePool.Get(out ObjectPoolReference objectPoolReference);
            objectPoolReference.transform.position = transform.position;
            AudioSource audioSource = objectPoolReference.GetComponent<AudioSource>();
            audioSource.PlayOneShot(WeaponConfig.ShotSingle);
            //Debug.Log(Time.time + " | " + WeaponPersistantData.CurrentAmmoCount);
        }
        #endregion
    }
}
