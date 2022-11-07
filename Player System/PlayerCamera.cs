using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Project.InputSystem;
using NaughtyAttributes;

namespace Project.PlayerSystem
{
    public class PlayerCamera : MonoBehaviour
    {
        #region Temp
        //[Header("Temporary Things", order = 0)]
        #endregion

        #region Fields
        [Header("Fields", order = 1)]
        [SerializeField] private InputAsset _inputAsset;
        [SerializeField] private PointerPosition _pointerPosition;
        [SerializeField] private CharacterSystem.Character _character;

        [HorizontalLine]

        [SerializeField] private Transform _follow;
        [SerializeField] private Transform _camCenter;
        [SerializeField] private Transform _camZoom;
        public Transform CamCenter { get => _camCenter; set => _camCenter = value; }

        [HorizontalLine]

        [SerializeField] private AnimationCurve _rotationAcceleration;
        private float _accelerationTime;
        private float _rotationSpeed;
        private float _direction;

        [HorizontalLine]
        [Range(0.01f, 1)]
        [SerializeField] private float _lookAtSpeed;
        private Quaternion _lookAtCurrent;
        private Quaternion _lookAtTarget;
        public Quaternion LookAtCurrent { get => _lookAtCurrent; set => _lookAtCurrent = value; }
        public Quaternion LookAtTarget { get => _lookAtTarget; set => _lookAtTarget = value; }
        

        [HorizontalLine]
        [Range(0.01f,1)]
        [SerializeField] private float _zoomSpeed;
        [SerializeField] private float _zoomMax;
        [SerializeField] private float _zoomMin;
        [SerializeField] private float _zoomCurrent;
        [SerializeField] private float _zoomTarget;
        [SerializeField] private float _zoomInitial;
        [SerializeField] private float _zoomStep;


        #endregion

        #region Functions

        #endregion

        #region Methods
        void Start()
        {
            
        }
        void LateUpdate()
        {
            CenterUpdate();
            KeyboardRotation();
            TowardsTargetRotation();
            LookAtPointerUpdate();
            ZoomUpdate();
        }
        void CenterUpdate()
        {
            _camCenter.position =Vector3.Lerp(_camCenter.position, _follow.position + _character.CharacterRoot.TransformDirection(_character.Movement.DirectionAndMoveGear * 0.1f), 1 - Mathf.Pow(1 - 0.995f, Time.deltaTime));
           
        }
        void KeyboardRotation()
        {
            if (_pointerPosition.CursorAnchored) return;

            float cameraInput = _inputAsset.GameInput.Gameplay.RotateCamera.GetAxis();
            if (cameraInput == 0 && _accelerationTime > 0)
            {
                _accelerationTime -= Time.deltaTime;
            }
            else if (cameraInput != 0 && _accelerationTime < _rotationAcceleration.keys[1].time)
            {
                _accelerationTime += Time.deltaTime;
                _direction = cameraInput;
            }

            _rotationSpeed = _rotationAcceleration.Evaluate(_accelerationTime) * Time.deltaTime * _direction;
            _camCenter.Rotate(Vector3.up, _rotationSpeed);

        }
        void TowardsTargetRotation()
        {

        }      
        void LookAtPointerUpdate()
        {         
            if (_pointerPosition.CursorAnchored)
            {
                _lookAtTarget =Quaternion.LookRotation((_pointerPosition.CameraLookAtTransform.position - _character.CharacterRoot.position).normalized);
            }
            if(Quaternion.Angle(_lookAtTarget, _lookAtCurrent) > 1)
            {
                _lookAtCurrent = Quaternion.Slerp(_camCenter.rotation, _lookAtTarget, 1 - Mathf.Pow(1 - _lookAtSpeed, Time.deltaTime));
                _camCenter.rotation = _lookAtCurrent;
            }
        }
        void ZoomUpdate()
        {
            float scroll = _inputAsset.GameInput.Gameplay.Scroll.GetAxis();
            if (scroll > 0)
            {
                _zoomTarget = Mathf.Clamp(_zoomTarget + _zoomStep, _zoomMax, _zoomMin);
            }
            else if (scroll < 0)
            {
                _zoomTarget = Mathf.Clamp(_zoomTarget - _zoomStep, _zoomMax, _zoomMin);
            }
            if (_inputAsset.GameInput.Gameplay.MMB.Press())
            {
                if(Mathf.InverseLerp(_zoomMin,_zoomMax, _zoomCurrent) < 0.5f)
                {
                    _zoomTarget = _zoomMax;
                }
                else
                {
                    _zoomTarget = _zoomMin;
                }
            }

            _zoomCurrent = Mathf.Lerp(_zoomCurrent, _zoomTarget, 1 - Mathf.Pow(1 - _zoomSpeed, Time.deltaTime));

            //_camZoom.localPosition = new Vector3(0, 0, _zoomCurrent);
            Camera.main.fieldOfView = _zoomCurrent;

        }
        #endregion
    }
}
