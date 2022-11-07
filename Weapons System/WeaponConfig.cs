using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;
using Project.Utilities;
using Project.SoundSystem;

namespace Project.WeaponsSystem
{
    [CreateAssetMenu(fileName = "New Weapon Config", menuName = "Project/WeaponsSystem/Weapon Config")]
    [System.Serializable]
    public class WeaponConfig : ScriptableObject
    {
        [HorizontalLine]
        [Header("Config")]

        [SerializeField] private WeaponType _weaponType;
        [SerializeField][Dropdown(nameof(GetWeaponSubtype))] private WeaponSubtype _weaponSubtype;
        WeaponSubtype[] GetWeaponSubtype()
        {
            switch (_weaponType)
            {
                case WeaponType.ProjectileBased:
                    return new WeaponSubtype[]
                    {
                        WeaponSubtype.AssaultRifle,
                        WeaponSubtype.Pistol,
                        WeaponSubtype.Rifle,
                        WeaponSubtype.Shotgun,
                        WeaponSubtype.RocketLauncher,
                        WeaponSubtype.GranadeLauncher,
                        WeaponSubtype.LightMachineGun,
                        WeaponSubtype.Crossbow,
                        WeaponSubtype.PlasmaImpulse,
                        WeaponSubtype.LaserImpulse
                    };
                  
                case WeaponType.FlameBased:
                    return new WeaponSubtype[]
                    {
                        WeaponSubtype.Flamethrower,                    
                    };
                 
                case WeaponType.BeamBased:
                    return new WeaponSubtype[]
                    {
                        WeaponSubtype.LaserBeam,
                        WeaponSubtype.LightningBeam,
                        WeaponSubtype.PlasmaBeam
                    };
                   
            }
            return null;
        }

        [HorizontalLine]

        [SerializeField] [EnumFlags] private SelectiveFire _selectiveFire;
        [SerializeField] [Range(1,10)] [EnableIf(nameof(HasBurstMode))] private int _burstShotsCount;
        private bool HasBurstMode() => _selectiveFire.HasFlag(SelectiveFire.Burst);
        [SerializeField] private LoadingMode _loadingMode;
        [SerializeField] private float _rateOfFire;
        [SerializeField] private float _dispersion;
        [SerializeField] private DamageType _damageType;
        [SerializeField] private int _damage;
        [SerializeField] private int _penetration;
        [SerializeField] private float _reloadTime;
        [SerializeField] private float _guidingSpeed;
        [SerializeField] private float _aimFocusSpeed;
        [SerializeField] private float _recoil;
        [SerializeField] private int _magazineCapacity;
        [SerializeField] private bool _enableSubmunition;
        [SerializeField] [Range(1, 10)] [EnableIf(nameof(_enableSubmunition))] int _submunitionCount;
        private bool EnableConstainedDistance => _weaponSubtype == WeaponSubtype.Flamethrower || _weaponSubtype == WeaponSubtype.Shotgun;
        [SerializeField][EnableIf(nameof(EnableConstainedDistance))] private float _constrainedDistance;

        public WeaponType WeaponType { get => _weaponType; private set => _weaponType = value; }
        public SelectiveFire SelectiveFire { get => _selectiveFire; private set => _selectiveFire = value; }
        public int BurstShotsCount { get => _burstShotsCount; set => _burstShotsCount = value; }
        public LoadingMode LoadingMode { get => _loadingMode; private set => _loadingMode = value; }
        public float RateOfFire { get => _rateOfFire; private set => _rateOfFire = value; }
        public float Dispersion { get => _dispersion; private set => _dispersion = value; }
        public DamageType DamageType { get => _damageType; private set => _damageType = value; }
        public int Damage { get => _damage; private set => _damage = value; }
        public int Penetration { get => _penetration; private set => _penetration = value; }
        public float ReloadTime { get => _reloadTime; private set => _reloadTime = value; }
        public float GuidingSpeed { get => _guidingSpeed; private set => _guidingSpeed = value; }
        public float AimFocusSpeed { get => _aimFocusSpeed; private set => _aimFocusSpeed = value; }
        public float Recoil { get => _recoil; private set => _recoil = value; }
        public int MagazineCapacity { get => _magazineCapacity; private set => _magazineCapacity = value; }
        public float ConstrainedDistance { get => _constrainedDistance; private set => _constrainedDistance = value; }
        public bool EnableSubmunition { get => _enableSubmunition; set => _enableSubmunition = value; }
        public int SubmunitionCount { get => _submunitionCount; set => _submunitionCount = value; }

        [HorizontalLine]
        [Header("Visual Effects")]
                                                                                                                                                                                                                              
        [SerializeField] private BodyPenetrationEffect _lifeFormsPenetrationEffect;
        [SerializeField] private BodyPenetrationEffect _notLifeFormsPenetrationEffect;
        [SerializeField] private bool _impactEffectOnCharacterHit;
        [SerializeField] private ObjectPoolSO _impactEffect;
        [SerializeField] private bool _enableMuzzleFlash;
        [SerializeField][EnableIf(nameof(_enableMuzzleFlash))] private ObjectPoolSO _muzzleFlash;
        [SerializeField] private MuzzleOutputEffectType _muzzleOutputEffectType;
        [SerializeField][DisableIf(nameof(_muzzleOutputEffectType), MuzzleOutputEffectType.NotVisibleProjectile)] private ObjectPoolSO _muzzleOutputEffect;
        [SerializeField] private float _projectileSpeed;

        public BodyPenetrationEffect LifeFormsPenetrationEffect { get => _lifeFormsPenetrationEffect; private set => _lifeFormsPenetrationEffect = value; }
        public BodyPenetrationEffect NotLifeFormsPenetrationEffect { get => _notLifeFormsPenetrationEffect; set => _notLifeFormsPenetrationEffect = value; }
        public bool ImpactEffectOnCharacterHit { get => _impactEffectOnCharacterHit; private set => _impactEffectOnCharacterHit = value; }
        public ObjectPoolSO ImpactEffect { get => _impactEffect; private set => _impactEffect = value; }
        public bool EnableMuzzleFlash { get => _enableMuzzleFlash; private set => _enableMuzzleFlash = value; }
        public ObjectPoolSO MuzzleFlash { get => _muzzleFlash; private set => _muzzleFlash = value; }
        public MuzzleOutputEffectType MuzzleOutputEffectType { get => _muzzleOutputEffectType; private set => _muzzleOutputEffectType = value; }
        public ObjectPoolSO MuzzleOutputEffect { get => _muzzleOutputEffect; private set => _muzzleOutputEffect = value; }
        public float ProjectileSpeed { get => _projectileSpeed; private set => _projectileSpeed = value; }

        [HorizontalLine]
        [Header("Sound Effects")]
        [SerializeField] private AudioClip _shotSingle;
        [SerializeField] private bool _useShotLong;
        [SerializeField][EnableIf(nameof(_useShotLong))] private AudioClip _shotLong;
        [SerializeField] private AudioClip _reload;
        [SerializeField] private AudioClip _draw;
        [SerializeField] private AudioClip _holster;
        [SerializeField] private ObjectPoolSO _audioSourcePool;
        public AudioClip ShotSingle { get => _shotSingle; private set => _shotSingle = value; }
        public AudioClip ShotLong { get => _shotLong; private set => _shotLong = value; }
        public AudioClip Reload { get => _reload; private set => _reload = value; }
        public AudioClip Draw { get => _draw; private set => _draw = value; }
        public AudioClip Holster { get => _holster; private set => _holster = value; }
        public ObjectPoolSO AudioSourcePool { get => _audioSourcePool; set => _audioSourcePool = value; }
      
    }
}
