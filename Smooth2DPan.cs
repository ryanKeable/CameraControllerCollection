using UnityEngine;

// Attach this class to the camera in the scene
public class Smooth2DPan : MonoBehaviour
{
    // CONST 
    private const float _targetFPS = 30.0f;

    [SerializeField] private Camera _camera;
    [SerializeField] private Transform _targetTransform;
    [SerializeField] private Vector2 cameraBoundsSize = new Vector2(30, 60);
    [SerializeField] private float cameraBoundsCornerRadius = 3.0f;
    [Range(0.0f, 1.0f)]
    [SerializeField] private float _maxCameraSpeed = 0.2f;
    [Range(0.0f, 1.0f)]
    [SerializeField] private float _cameraSmoothness = 0.33f;
    [Range(1.0f, 50.0f)]
    [SerializeField] private float _boundaryPaddingDistance = 50.0f;
    [SerializeField] private AnimationCurve _biasCurve;

    private bool isDragging = false;
    private Vector2 _rawPos;
    private Vector2 _inputDir;
    private Vector2 _resetDir;
    private Vector2 _inputVelocityVector;
    private Vector2 _resetVelocityVector;
    private float _distToBounds;


    public Camera mapCamera { get => _camera; }


    public bool InputEnabled { get; set; }

    void OnEnable()
    {
        InputEnabled = true;

        RelocateCamera(Vector3.zero);
        _rawPos = _targetTransform.transform.localPosition;

    }

    void Update()
    {
        if (InputEnabled == false)
        {
            isDragging = false; //HACK: stop drag when input is disabled
            return;
        }

        SetCameraPos(CalcInputStates());
    }

    private Vector2 CalcInputStates()
    {
        Vector2 dragPosition = Vector2.zero;

        if (Input.GetMouseButtonDown(0) || (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began))
        {
            isDragging = true;
        }

        if (Input.GetMouseButtonUp(0) || (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended))
        {
            isDragging = false;
        }

        if (isDragging)
        {
            dragPosition = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));

            if (Input.touchCount > 0)
            {
                dragPosition = Input.GetTouch(0).position;
            }

        }

        return dragPosition;
    }

    private void LateUpdate()
    {
        if (InputEnabled == false) return;
        _targetTransform.transform.localPosition = new Vector3(_rawPos.x, 0, _rawPos.y);
    }

    public void RelocateCamera(Vector3 pos)
    {
        _targetTransform.transform.localPosition = pos;
        _rawPos = pos;
    }

    public void ToggleMapNavigation(bool toggleOn)
    {
        _camera.gameObject.SetActive(toggleOn);
        InputEnabled = !toggleOn;
    }

    float cameraPanWeight;
    private void SetCameraPos(Vector2 inputPos)
    {
        Vector2 resetPos = _rawPos.normalized;

        cameraPanWeight = GetBoundaryInfluence(_rawPos - inputPos);
        float smoothSpeed = GetCameraSpeed();

        _inputDir = Vector2.SmoothDamp(_inputDir + inputPos, Vector2.zero, ref _inputVelocityVector, smoothSpeed);
        _resetDir = Vector2.SmoothDamp(_resetDir + resetPos, Vector2.zero, ref _resetVelocityVector, smoothSpeed);

        Vector2 targetDir = Vector2.Lerp(_inputDir, _resetDir, cameraPanWeight);

        _rawPos = Vector3.Lerp(_rawPos, _rawPos - targetDir, _cameraSmoothness);
    }

    private float GetCameraSpeed()
    {
        float camSpeed = Time.smoothDeltaTime * _targetFPS; // pin cam update speed to desired frame rate (should be ~ 1.0f)
        camSpeed /= (1 - _maxCameraSpeed) * 60.0f; // apply speed
        return camSpeed;
    }

    private float GetBoundaryInfluence(Vector2 inputPos)
    {
        _distToBounds = DistanceToBounds(inputPos, cameraBoundsSize, new Vector2(cameraBoundsCornerRadius, cameraBoundsCornerRadius));
        float bias = Mathf.Max(_distToBounds / _boundaryPaddingDistance, 0.0f);
        float smoothBias = _biasCurve.Evaluate(bias);
        smoothBias = smoothBias < 0.01f ? 0.0f : smoothBias;

        return smoothBias;
    }

    private float DistanceToBounds(Vector2 inputPos, Vector2 size, Vector2 radius)
    {
        inputPos = new Vector2(Mathf.Abs(inputPos.x), Mathf.Abs(inputPos.y));

        Vector2 halfSize = size * 0.5f;
        Vector2 positionToEvaluatue = inputPos - halfSize + radius;

        float dist = Vector2.Max(positionToEvaluatue, Vector2.zero).magnitude;
        return dist + Mathf.Min(Mathf.Max(positionToEvaluatue.x, positionToEvaluatue.y), 0.0f) - radius.x;
    }
}
