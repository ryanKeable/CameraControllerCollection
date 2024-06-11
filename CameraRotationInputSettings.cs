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

    public abstract class CameraRotationInputSettings : ScriptableObject
    {
        [Range(0.001f, 1.0f)]
        [SerializeField] private float _smoothSpeed = 0.133f;
        [SerializeField] private Vector3 _maxCameraAngle = new Vector3(60.0f, 20.0f, 15.0f);
        [SerializeField] protected bool _invert = false;
        [SerializeField] private AnimationCurve _dampingCurve = AnimationCurve.EaseInOut(0f, 1f, 1f, 1f);
        [SerializeField] private float _magnitudeScalar = 0.03f;
        private float _prevMagnitude;

        public float TargetFPS { get => 30.0f; }
        public float SmoothSpeed { get => _smoothSpeed * Time.deltaTime * TargetFPS; }
        public Vector3 MaxCameraAngle { get => _maxCameraAngle; }
        public AnimationCurve Damping { get => _dampingCurve; }
        public virtual float AccelerationScalar { get; protected set; }

        public Vector3 GetRotation()
        {
            SetAccelerationScalar();
            return GetDevcieInput();
        }
        public abstract Vector3 GetDevcieInput();
        protected virtual float Magnitude { get; set; }

        public virtual void Reset() { }

        private void SetAccelerationScalar()
        {
            AccelerationScalar = MathfExtensions.Map((_prevMagnitude + Magnitude) / 2, 0, _magnitudeScalar, 0, 1);
            AccelerationScalar = Mathf.Clamp01(AccelerationScalar);
            _prevMagnitude = Magnitude;
        }

    }
}