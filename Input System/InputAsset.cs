using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Project.InputSystem
{
    [CreateAssetMenu(fileName = "InputAsset", menuName = "Project/Input System/InputAsset")]
    public class InputAsset : ScriptableObject
    {
        #region Temp
        //[Header("Temporary Things", order = 0)]
        #endregion

        #region Fields
        [Header("Fields", order = 1)]
        private GameInput _gameInput;

        [SerializeField] private AnimationCurve _mouseSensitivityCurve;
        [SerializeField] private Vector2 _mouseSensitivity = Vector2.one;
        [SerializeField] private bool _invertY;
        [SerializeField] private bool _mouseLocked;

        public GameInput GameInput { get => _gameInput; private set => _gameInput = value; }

        #endregion

        #region Functions
        public Vector2 GetMouseDelta(bool fixedDeltaTime = false)
        {
            Vector2 inputDeltaTime = (fixedDeltaTime ? Time.fixedDeltaTime : Time.deltaTime) * 5 * _gameInput.Gameplay.MouseDelta.GetVector2();
            if (_invertY == false)
            {
                inputDeltaTime.y = -inputDeltaTime.y;
            }

            float sensitivityFromCurve = _mouseSensitivityCurve.Evaluate(inputDeltaTime.magnitude);
            float sensitivityFactorX = sensitivityFromCurve * _mouseSensitivity.x;
            float sensitivityFactorY = sensitivityFromCurve * _mouseSensitivity.y;

            return new Vector2(inputDeltaTime.x * sensitivityFactorX, inputDeltaTime.y * sensitivityFactorY);
        }
        #endregion



        #region Methods
        private void OnEnable()
        {
            if (_gameInput == null)
            {
                _gameInput = new GameInput();
            }
            EnableGameplayInput(true);
            EnableMenuInput(true);
        }
        private void OnDisable()
        {
            DisableAllInput();
        }
        public void EnableGameplayInput(bool gameplayEnabled)
        {
            if (gameplayEnabled)
            {
                _gameInput.Gameplay.Enable();
            }
            else
            {
                _gameInput.Gameplay.Disable();
            }
        }
        public void EnableMenuInput(bool menuEnabled)
        {
            if (menuEnabled)
            {
                _gameInput.Menu.Enable();
            }
            else
            {
                _gameInput.Menu.Disable();
            }
        }
        public void DisableAllInput()
        {
            _gameInput.Menu.Disable();
            _gameInput.Gameplay.Disable();
        }

        #endregion
    }
}