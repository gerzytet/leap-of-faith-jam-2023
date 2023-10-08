using UnityEditor;
using UnityEngine;

public class CameraZoomControl : MonoBehaviour
{
    public float defaultSize;
    public float maxSize;
    public float maxSizeVelocity;
    public float resizeRate;
    private float currentSize;

    void Start()
    {
        currentSize = defaultSize;
    }
    void Update()
    {
        float frogVelocity = Frog.instance.GetComponentInChildren<Rigidbody2D>().velocity.magnitude;
        float targetSize = Mathf.Lerp(defaultSize, maxSize, Mathf.InverseLerp(0, maxSizeVelocity, frogVelocity));
        currentSize = Mathf.Lerp(currentSize, targetSize, resizeRate * Time.deltaTime);
        GetComponent<Camera>().orthographicSize = currentSize;
    }
}
