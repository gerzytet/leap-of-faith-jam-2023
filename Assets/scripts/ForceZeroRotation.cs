using UnityEngine;

public class ForceZeroRotation : MonoBehaviour
{
    void Update()
    {
        transform.rotation = Quaternion.identity;
    }
}
