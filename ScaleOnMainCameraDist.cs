using UnityEngine;

public class ScaleOnMainCameraDist : MonoBehaviour
{

    [SerializeField] float _spriteScale = 1.5f;
    [SerializeField] float _scaleDistance = 2.5f;

    private static float cachedScale;

    // Update is called once per frame
    void OnEnable()
    {
        cachedScale = transform.localScale.x;
    }
    void Update()
    {
        transform.localScale = ScaleButtonOnDist();
    }

    Vector3 ScaleButtonOnDist()
    {
        Vector3 fullScale = Vector3.one * cachedScale * _spriteScale;
        Vector3 smallScale = Vector3.one * cachedScale;
        float distToCam = DistToCam() / _scaleDistance;
        float smoothScale = Mathf.SmoothStep(0.0f, 1.0f, distToCam);

        return Vector3.Lerp(fullScale, smallScale, smoothScale);
    }

    float DistToCam()
    {
        Vector3 camPos = Camera.main.transform.position;
        Vector3 myPos = transform.position;

        Vector2 camPosXY = new Vector2(camPos.x, camPos.y);
        Vector2 myPosXY = new Vector2(myPos.x, myPos.y);

        float dist = Vector2.Distance(camPosXY, myPosXY);

        return dist;
    }
}
