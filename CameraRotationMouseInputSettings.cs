using System;
using UnityEngine;

/*
    - update the camera's position based on how far we have rotated the gyro from it's init position
    - smoothdamping in this application is not currently optimal. needs to slow down coming into the max angle, but not be slow moving away from the max angle
    - need to have seperate configs for the editor and the device 
    - needs a little overshoot, so we need some velocity applied to the accumulatedRotation and then dampen the velocity
*/

namespace CameraGyro
{
    [CreateAssetMenu(menuName = "Camera/Camera Mouse Input Settings", order = 1000)]

    public class CameraRotationMouseInputSettings : CameraRotationInputSettings
    {
        private Vector3 _currResolvedInput;
        private Vector3 _prevInputPos;

        public override Vector3 GetDevcieInput()
        {
            Vector3 mouseInputPos = Camera.main.ScreenToViewportPoint(Input.mousePosition);
            mouseInputPos *= 2;
            mouseInputPos -= Vector3.one;

            Magnitude = Mathf.Abs((_prevInputPos - mouseInputPos).magnitude);
            _prevInputPos = mouseInputPos;

            if (_invert) mouseInputPos = new Vector3(-mouseInputPos.x, -mouseInputPos.y, mouseInputPos.z);

            bool isOutside = Mathf.Abs(mouseInputPos.x) > 1 || Mathf.Abs(mouseInputPos.y) > 1;
            if (isOutside) return _currResolvedInput;

            Vector2 sign = new Vector2(Mathf.Sign(mouseInputPos.x), Mathf.Sign(mouseInputPos.y));

            float x = Mathf.Lerp(0, MaxCameraAngle.x, Mathf.Abs(mouseInputPos.x)) * sign.x;
            float y = Mathf.Lerp(0, MaxCameraAngle.y, Mathf.Abs(mouseInputPos.y)) * sign.y;


            _currResolvedInput = new Vector3(-y, x, Mathf.Min(Input.mouseScrollDelta.y, MaxCameraAngle.z));

            return _currResolvedInput;
        }


    }
}