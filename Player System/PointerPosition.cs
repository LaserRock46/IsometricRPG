using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Runtime.InteropServices;
using NaughtyAttributes;
using Project.InputSystem;

namespace Project.PlayerSystem
{
    public class PointerPosition : MonoBehaviour
    {
        #region Temp
        [Header("Temporary Things", order = 0)]

        #endregion

        #region Fields
        [Header("Fields", order = 1)]
        [SerializeField] private InputAsset _inputAsset;
        [SerializeField] private CharacterSystem.Character _character;
        [SerializeField] private PlayerCamera _playerCamera;

        [SerializeField] private Transform _cameraLookAtTransform = null;
        [SerializeField] private Transform _anchoredCursorPivot;
        [SerializeField] private float _anchoredCursorRotationSpeed;
        public Transform CameraLookAtTransform { get => _cameraLookAtTransform;  set => _cameraLookAtTransform = value; }
        public Transform AnchoredCursorPivot { get => _anchoredCursorPivot; set => _anchoredCursorPivot = value; }

        [SerializeField] private RectTransform _pointer;
        [SerializeField] private Canvas _canvas = null;
        private RectTransform _canvasRect;

        [SerializeField] private bool _componentEnabled;
        private bool _cursorVisible;
        private bool _cursorAnchored;
        public bool ComponentEnabled { get => _componentEnabled; set => _componentEnabled = value; }
        public bool CursorVisible { get => _cursorVisible; set => _cursorVisible = value; }
        public bool CursorAnchored { get => _cursorAnchored; set => _cursorAnchored = value; }

        //cursor position
        //private Vector2 _diff;       
        private Vector2 _cursorPosition;
        public Vector2 CursorPosition { get => _cursorPosition; set => _cursorPosition = value; }
        

        //private Vector2 _uiOffset;
        //private Vector2 _initRectPointer;
        //private Vector2 _initMousePosition;
        private bool _pushMouse;
        
        //lookAt position
        //private Vector3 _lookAtDiff;
        //private Vector3 _previousPlayerTransformPosition;

        #endregion

        #region Functions
        [DllImport("user32.dll")]
        public static extern bool SetCursorPos(int X, int Y);     
       
       
        #endregion

        #region Methods
        private void Awake()
        {
            _canvasRect = _canvas.GetComponent<RectTransform>();
            //_uiOffset = new Vector2(_canvasRect.sizeDelta.x / 2f, _canvasRect.sizeDelta.y / 2f);
            _cameraLookAtTransform.position = _character.CharacterRoot.position;
        }
        void Start()
        {
            SetInitialPositionsAtStart();
        }
        void Update()
        {
            if (!_componentEnabled) return;
            HideHardwareCursor();
            AnchorCursor();
            CursorMovement();
        }
        void HideHardwareCursor()
        {
            if (_cursorVisible == false && Cursor.visible == true)
            {
                Cursor.visible = false;
            }
            if (_cursorVisible == true && Cursor.visible == false)
            {
                Cursor.visible = true;
            }
        }
        void SetInitialPositionsAtStart()
        {
            //po rozpoczêciu gry kursor pojawi siê na œrodku ekranu
            _pushMouse = true;
            //_initRectPointer = _pointer.anchoredPosition;
            //_lookAtDiff = _character.CharacterRoot.position - _cameraLookAtTransform.position;
        }
        void AnchorCursor()
        {
            if (_inputAsset.GameInput.Gameplay.PointerLook.Press())
            {
                Ray ray = Camera.main.ScreenPointToRay(_pointer.anchoredPosition3D);
                Plane playerGroundLevelPlane = new Plane(Vector3.up, _character.CharacterRoot.position);

                float enter = 0.0f;
                if (playerGroundLevelPlane.Raycast(ray, out enter))
                {
                    _cameraLookAtTransform.position = ray.GetPoint(enter);
                    _cursorAnchored = true;
                    // cache difference between player and LookAt;
                    //_lookAtDiff = _character.CharacterRoot.position - _cameraLookAtTransform.position;

                    _anchoredCursorPivot.position = _character.CharacterRoot.position;
                    _anchoredCursorPivot.LookAt(_cameraLookAtTransform);
                    _cameraLookAtTransform.parent = _anchoredCursorPivot;
                }
                
            }
            if (_inputAsset.GameInput.Gameplay.PointerLook.Hold() == false)
            {
                if (_cursorAnchored == true && Quaternion.Angle(_playerCamera.CamCenter.rotation, _anchoredCursorPivot.rotation) < 2f)
                {
                    _cursorAnchored = false;
                    _cameraLookAtTransform.parent = transform;
                }
            }
            if (_cursorAnchored)
            {
                _anchoredCursorPivot.position = _character.CharacterRoot.position;
                float angle = _inputAsset.GetMouseDelta().x * Time.deltaTime * _anchoredCursorRotationSpeed;
                float offset = _inputAsset.GetMouseDelta().y * Time.deltaTime * _anchoredCursorRotationSpeed;
                //_cameraLookAtTransform.localPosition = new Vector3(0, 0, Mathf.Clamp(_cameraLookAtTransform.localPosition.z - offset, 1, 50));
                _anchoredCursorPivot.Rotate(Vector3.up, angle);

                _cursorPosition = Utilities.UIFunctions.WorldPositionToCanvas(_cameraLookAtTransform.position, _canvasRect);
                _pointer.anchoredPosition = _cursorPosition;
            }
        }
        void CursorMovement()
        {
            // lock if anchored
            if (Cursor.lockState != CursorLockMode.Locked && _cursorAnchored)
            {
                Cursor.lockState = CursorLockMode.Locked;
            }
            // cache initial _pointer and mouse positions
            if (_inputAsset.GameInput.Gameplay.PointerLook.Release())
            {
                Cursor.lockState = CursorLockMode.None;
                //_initRectPointer = _pointer.anchoredPosition;
                _pushMouse = true;
            }
            // debug after release
            if (Vector2.zero != _inputAsset.GameInput.Gameplay.MouseDelta.ReadValue<Vector2>() && _pushMouse == true)
            {
                Cursor.lockState = CursorLockMode.None;
                //_initMousePosition = _inputAsset.GameInput.Gameplay.MousePosition.ReadValue<Vector2>();
                _pushMouse = false;
            }
            // update cursor position
            if (!_cursorAnchored && !_pushMouse)
            {
                //Vector2 mousePosition = _inputAsset.GameInput.Gameplay.MousePosition.ReadValue<Vector2>();           
                //_diff = _initMousePosition - mousePosition;
                //_target = _initRectPointer - _diff;
                _cursorPosition = _pointer.anchoredPosition + _inputAsset.GameInput.Gameplay.MouseDelta.GetVector2();
                _cursorPosition = new Vector2(Mathf.Clamp(_cursorPosition.x, 0, Screen.width), Mathf.Clamp(_cursorPosition.y, 0, Screen.height));
              
                _pointer.anchoredPosition = _cursorPosition;
            }
            // center mouse if hardware cursor reach edge of the screen
            if (_inputAsset.GameInput.Gameplay.MousePosition.ReadValue<Vector2>().y <= 1
                || _inputAsset.GameInput.Gameplay.MousePosition.ReadValue<Vector2>().y >= Screen.height - 1
                || _inputAsset.GameInput.Gameplay.MousePosition.ReadValue<Vector2>().x <= 1
                || _inputAsset.GameInput.Gameplay.MousePosition.ReadValue<Vector2>().x >= Screen.width - 1)
            {
                Cursor.lockState = CursorLockMode.Locked;
                //_initRectPointer = _pointer.anchoredPosition;
                _pushMouse = true;
            }         
        }
        public void PointerLock()
        {
            _componentEnabled = false;
            _cursorVisible = true;
            _cursorAnchored = true;
            Cursor.visible = true;
            //_initRectPointer = _pointer.anchoredPosition;
        }
        public void PointerUnlock()
        {
            _componentEnabled = true;
            _cursorVisible = false;
            _cursorAnchored = false;

            //_initMousePosition = _inputAsset.GameInput.Gameplay.MousePosition.ReadValue<Vector2>();
        }      
        #endregion
    }
}
