using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Project.InputSystem;
using UnityEngine.InputSystem;

namespace Project.PlayerSystem
{
    public class PlayerInput : MonoBehaviour
    {
        #region Temp
        //[Header("Temporary Things", order = 0)]
        #endregion

        #region Fields
        [Header("Fields", order = 1)]
        [SerializeField] private CharacterSystem.Character _character;
        [SerializeField] private PointerPosition _pointerPosition;
        [SerializeField] private InputAsset _inputAsset;
        #endregion

        #region Functions

        #endregion

        #region Methods

        private void OnEnable()
        {
            _inputAsset.GameInput.Gameplay.Move.performed += context => _character.Movement.InputMove = context.ReadValue<Vector3>();
            _inputAsset.GameInput.Gameplay.Run.performed += context => _character.Movement.InputRun = context.ReadValueAsButton();

            _inputAsset.GameInput.Gameplay.LMB.started += InputTrigger;
            _inputAsset.GameInput.Gameplay.LMB.canceled += InputTrigger;



        }
        private void OnDisable()
        {
            _inputAsset.GameInput.Gameplay.Move.performed -= context => _character.Movement.InputMove = context.ReadValue<Vector3>();
            _inputAsset.GameInput.Gameplay.Run.performed -= context => _character.Movement.InputRun = context.ReadValueAsButton();
            _inputAsset.GameInput.Gameplay.LMB.started -= InputTrigger;
            _inputAsset.GameInput.Gameplay.LMB.canceled -= InputTrigger;

        }


        void Update()
        {
#if UNITY_EDITOR
            Vector2 mousePosition = _inputAsset.GameInput.Gameplay.MousePosition.GetVector2();
            if (mousePosition.x == 0 || mousePosition.y == 0 || mousePosition.x >= Screen.width - 1 || mousePosition.y >= Screen.height - 1) return;         
#endif
                InputUpdate();
        }
        void InputTrigger(InputAction.CallbackContext context)
        {
            if (context.started)
            {
                _character.WeaponSystemNode.FireController.InputTriggerPress();            
            }
            if (context.canceled)
            {
                _character.WeaponSystemNode.FireController.InputTriggerRelease();           
            }
        }

        void InputUpdate()
        {      
            //_character.Movement.InputMove = _inputAsset.GameInput.Gameplay.Move.GetVector3();
            //_character.Movement.InputRun = _inputAsset.GameInput.Gameplay.Run.Hold();
           
            //_character.WeaponSystemNode.FireController.InputTriggerPress = _inputAsset.GameInput.Gameplay.LMB.Press();
            //_character.WeaponSystemNode.FireController.InputTriggerRelease = _inputAsset.GameInput.Gameplay.LMB.Release();
            _character.WeaponSystemNode.FireController.InputReloadd = _inputAsset.GameInput.Gameplay.Reload.Release();
            if (_inputAsset.GameInput.Gameplay.ChangeFireMode.Press()) _character.WeaponSystemNode.FireController.InputFireMode();

            if(_inputAsset.GameInput.Gameplay.Interact.Press()) _character.InteractableDetector.InputInteract();

            if (_inputAsset.GameInput.Gameplay.DrawWeapon.Press()) _character.ArmedUnarmedStateSwitch.InputDrawWeapon();

           
        }
        #endregion
    }
}
