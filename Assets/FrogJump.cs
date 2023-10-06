using System.Collections;
using UnityEngine;

public class FrogJump : MonoBehaviour
{
    [SerializeField] private GameObject backFootTarget;
    [SerializeField] private GameObject frontFootTarget;
    private Vector2 jumpDirection = new(0, 1);
    [SerializeField] private GameObject body;
    [SerializeField] private float rotationRate = -0.001f;

    [SerializeField] private GameObject frontFoot;
    IEnumerator Jump()
    {
        float originalHeight = frontFootTarget.transform.position.y;
        while (frontFoot.transform.position.y <= originalHeight + 0.01f)
        {
            body.transform.position += (Vector3)jumpDirection * 0.01f;
            body.transform.Rotate(0, 0, rotationRate);
            yield return null;
        }
    }
    
    // Update is called once per frame
    void FixedUpdate()
    {
        StartCoroutine(Jump());
    }
}
