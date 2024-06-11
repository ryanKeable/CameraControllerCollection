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
    [CreateAssetMenu(menuName = "Camera/Camera Gyro Input Settings", order = 1000)]

    public class CameraRotationGyroInputSettings : CameraRotationInputSettings
    {

        // SETTINGS
        [Range(0.001f, 30.0f)]
        [SerializeField] protected float _cameraSpeed = 10.0f;
        [Range(0.001f, 0.1f)]
        [SerializeField] private float _noiseThreshold = 0.05f;
        [SerializeField] private Vector3 _sensitivity = new Vector3(0.5f, 1.0f, 0.25f);

        private Vector3 _accumulatedRotationRate;

        private Vector3 GyroRotationRate => Input.gyro.rotationRateUnbiased;
        private Vector3 Sensitivity => _sensitivity * _cameraSpeed * Time.deltaTime * TargetFPS;

        public float NoiseThreshold { get => _noiseThreshold; }

        protected override float Magnitude => GyroRotationRate.magnitude;

        public override Vector3 GetDevcieInput()
        {
            _accumulatedRotationRate += RotationRateOverTime;

            return ClampToMaxAngle();
        }

        public override void Reset()
        {
            _accumulatedRotationRate = Vector3.zero;
        }

        private Vector3 RotationRateOverTime
        {
            get
            {
                Vector3 rotation = IgnoreNoise(GyroRotationRate);


                if (_invert) rotation = new Vector3(-rotation.x, -rotation.y, rotation.z);
                rotation = Clamp01(rotation);

                return Vector3.Scale(rotation, Sensitivity.SwizzleYXZ());
            }
        }



        private Vector3 IgnoreNoise(Vector3 input)
        {
            float x = IgnoreNoise(input.x);
            float y = IgnoreNoise(input.y);
            float z = IgnoreNoise(input.z);

            return new Vector3(x, y, z);
        }

        private float IgnoreNoise(float input)
        {
            if (Mathf.Abs(input) < _noiseThreshold)
            {
                input = 0;
            }
            return input;
        }

        private Vector3 Clamp01(Vector3 input)
        {
            float x = Clamp01(input.x);
            float y = Clamp01(input.y);
            float z = Clamp01(input.z);

            return new Vector3(x, y, z);
        }

        private float Clamp01(float input)
        {
            int sign = (int)Mathf.Sign(input);
            float min = Mathf.Min(Mathf.Abs(input), 1);

            return min * sign;
        }

        private Vector3 ClampToMaxAngle()
        {
            float x = ClampAccumulatedRotationRateToMaxAngle(_accumulatedRotationRate.x, MaxCameraAngle.y);
            float y = ClampAccumulatedRotationRateToMaxAngle(_accumulatedRotationRate.y, MaxCameraAngle.x);
            float z = ClampAccumulatedRotationRateToMaxAngle(_accumulatedRotationRate.z, MaxCameraAngle.z);

            _accumulatedRotationRate = new Vector3(x, y, z);

            return _accumulatedRotationRate;
        }

        private float ClampAccumulatedRotationRateToMaxAngle(float input, float maxAngle)
        {
            int sign = (int)Mathf.Sign(input);
            float min = Mathf.Min(Mathf.Abs(input), maxAngle);
            return min * sign;
        }
    }
}