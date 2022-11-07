using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Project.Utilities
{
    public class UIFunctions
    {
        public static Vector2 WorldPositionToCanvas(Vector3 worldPosition, RectTransform canvasRect, float maxVerticalOffsetOverZoom = 0)
        {
            worldPosition.y = worldPosition.y + Mathf.Lerp(0, maxVerticalOffsetOverZoom, Mathf.InverseLerp(5, 20, Camera.main.fieldOfView));
          
            Vector2 viewportPosition = Camera.main.WorldToViewportPoint(worldPosition);
            Vector2 proportionalPosition = new Vector2(viewportPosition.x * canvasRect.sizeDelta.x, viewportPosition.y * canvasRect.sizeDelta.y);
            return proportionalPosition;
        }
        public static bool IsBehindScreen(Vector3 worldPosition)
        {
            Vector3 forward = Camera.main.transform.TransformDirection(Vector3.forward);
            Vector3 toOther = worldPosition - Camera.main.transform.position;
            return Vector3.Dot(forward, toOther) < 0;
        }
    }

}
