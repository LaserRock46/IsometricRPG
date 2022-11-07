using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Project.InputSystem
{
    [AddComponentMenu(nameof(Project) + "/" + nameof(InputSystem) + "/InputExtension")]
    public static class InputExtension
    {
        #region Temp
        //[Header("Temporary Things", order = 0)]
        #endregion

        #region Fields
        //[Header("Fields", order = 1)]

        #endregion

        #region Functions
        public static bool Hold(this InputAction action) => action.ReadValue<float>() > 0;
        public static bool Press(this InputAction action) => action.triggered && action.ReadValue<float>() > 0;
        public static bool Release(this InputAction action) => action.triggered && action.ReadValue<float>() == 0;
        public static bool MultiTap(this InputAction action) => action.WasPerformedThisFrame();
        public static float GetAxis(this InputAction action) => action.ReadValue<float>();
        public static Vector2 GetVector2(this InputAction action) => action.ReadValue<Vector2>();
        public static Vector3 GetVector3(this InputAction action) => action.ReadValue<Vector3>();

        #endregion



        #region Methods
        #endregion
    }
}