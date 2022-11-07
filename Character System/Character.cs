using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;
using Project.WeaponsSystem;

namespace Project.CharacterSystem
{
    public class Character : MonoBehaviour
    {
        #region Temp
        //[Header("Temporary Things", order = 0)]
        #endregion

        #region Fields
        [Header("Fields", order = 1)]
        [SerializeField] private CharacterController _characterController;
        [SerializeField] private Transform _characterRoot;
        [SerializeField][ReadOnly] private Transform _characterHead;
        [SerializeField] private Abilities _abilities;
        [SerializeField] private Stats _stats;
        [SerializeField] private Movement _movement;
        [SerializeField] private MovementConstraint _movementConstraint;
        [SerializeField] private CharacterAnimator _characterAnimator;
        [SerializeField] private AnimatorCallbacks _animatorCallbacks;
        [SerializeField] private SightAndDetect _sightAndDetect;
        [SerializeField] private FootstepsSFX _footstepsSFX;
        [SerializeField] private Equipment _equipment;
        [SerializeField] private Inventory _inventory;
        [SerializeField] private CharacterDamage _characterDamage;
        [SerializeField] private WeaponsSystemNode _weaponSystemNode;
        [SerializeField] private InteractableDetector _interactableDetector;     
        [SerializeField] private RagdollController _ragdollController;
        [SerializeField] private ArmedUnarmedStateSwitch _armedUnarmedStateSwitch;

        public CharacterController CharacterController { get => _characterController; private set => _characterController = value; }
        public Transform CharacterRoot { get => _characterRoot; private set => _characterRoot = value; }
        public Transform CharacterHead { get => _characterHead; private set => _characterHead = value; }
        public Abilities Abilities { get => _abilities; private set => _abilities = value; }
        public Stats Stats { get => _stats; private set => _stats = value; }
        public Movement Movement { get => _movement; private set => _movement = value; }
        public MovementConstraint MovementConstraint { get => _movementConstraint; private set => _movementConstraint = value; }
        public CharacterAnimator CharacterAnimator { get => _characterAnimator; private set => _characterAnimator = value; }
        public AnimatorCallbacks AnimatorCallbacks { get => _animatorCallbacks; private set => _animatorCallbacks = value; }      
        public SightAndDetect SightAndDetect { get => _sightAndDetect; private set => _sightAndDetect = value; }
        public FootstepsSFX FootstepsSFX { get => _footstepsSFX; private set => _footstepsSFX = value; }
        public Equipment Equipment { get => _equipment; private set => _equipment = value; }
        public Inventory Inventory { get => _inventory; private set => _inventory = value; } 
        public CharacterDamage CharacterDamage { get => _characterDamage; private set => _characterDamage = value; }
        public WeaponsSystemNode WeaponSystemNode { get => _weaponSystemNode; private set => _weaponSystemNode = value; }
        public InteractableDetector InteractableDetector { get => _interactableDetector; private set => _interactableDetector = value; }      
        public RagdollController RagdollController { get => _ragdollController; private set => _ragdollController = value; }
        public ArmedUnarmedStateSwitch ArmedUnarmedStateSwitch { get => _armedUnarmedStateSwitch; private set => _armedUnarmedStateSwitch = value; }


        #endregion

        #region Functions

        #endregion

        #region Methods
        void Start()
        {
            _characterHead = _characterAnimator.Animator.GetBoneTransform(HumanBodyBones.Head);
           
        }
       
       
        #endregion
    }
}
