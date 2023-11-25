/*
@Authors - Craig
@Description - This is where all collision code for tilemap layers goes
*/

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
        float frogVelocityMagnitude = Frog.instance.GetComponentInChildren<Rigidbody2D>().velocity.magnitude;

        //determines where camera should be
        float targetSize = Mathf.Lerp(defaultSize, maxSize, Mathf.InverseLerp(0, maxSizeVelocity, frogVelocityMagnitude));
        
        //gradually shifts camera
        currentSize = Mathf.Lerp(currentSize, targetSize, resizeRate * Time.deltaTime);
        GetComponent<Camera>().orthographicSize = currentSize;
    }
}
