using System.Collections;
using UnityEngine;
using System.Runtime.CompilerServices;

namespace CameraGyro
{
    public class CameraGyroRotationRate : MonoBehaviour
    {
        [SerializeField] private Transform _transformToRotate;
        [SerializeField] private CameraRotationInputSettings _gyroSettings;
        [SerializeField] private CameraRotationMouseInputSettings _editorSettings;

        private Vector3 _targetRotation;
        private Vector3 _smoothRotationEulers;

        private CameraRotationInputSettings InputSettings => Application.isEditor ? _editorSettings : _gyroSettings;
        private float SmoothSpeed => InputSettings.SmoothSpeed * InputSettings.Damping.Evaluate(InputSettings.AccelerationScalar);


        private void OnEnable()
        {
            Initialize();
        }

        private void Initialize()
        {
            Input.gyro.enabled = true;
            Input.gyro.updateInterval = 1 / InputSettings.TargetFPS;

            ResetRotation();
        }

        private void OnApplicationFocus(bool hasFocus)
        {
            if (hasFocus) ResetRotation();
        }

        public void ResetRotation()
        {
            _transformToRotate.localEulerAngles = Vector3.zero;
            _smoothRotationEulers = Vector3.zero;
            _targetRotation = Vector3.zero;
            InputSettings.Reset();
        }

        private void Update()
        {
            _targetRotation = InputSettings.GetRotation();
        }

        private void LateUpdate()
        {
            SmoothLerpEulerRotation();
        }

        private void SmoothLerpEulerRotation()
        {
            _smoothRotationEulers = Vector3.Lerp(_smoothRotationEulers, _targetRotation, SmoothSpeed);
            _transformToRotate.localEulerAngles = _smoothRotationEulers;
        }

        internal static void LogInternal(string log, [CallerMemberName] string caller = "")
        {
            Debug.Log(GetLogString(log, caller));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static string GetLogString(string message, string callerName)
            => $"[{nameof(CameraGyroRotationRate)}.{callerName}] {message}";

    }
}