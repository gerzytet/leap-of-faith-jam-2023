using System;
using System.Collections;
using UnityEngine;

public class Frog : MonoBehaviour
{
    enum FrogState
    {
        JUMPING,
        LANDING,
        IDLE
    }
    [SerializeField] private GameObject backFootTarget;
    [SerializeField] private GameObject frontFootTarget;
    public Vector2 jumpDirection;
    [SerializeField] private GameObject body;
    [SerializeField] private float rotationRate;

    [SerializeField] private GameObject frontFoot;
    [SerializeField] private GameObject handReferencePoint;
    private Vector3 targetOffset;
    
    [SerializeField] private GameObject frontHand;
    private FrogState state = FrogState.IDLE;
    public float fixLegsRate;

    private bool flippedLeft = false;
    IEnumerator Jump()
    {
        state = FrogState.JUMPING;
        while (frontFoot.GetComponent<ToggleCollider>().IsColliding())
        {
            Debug.Log(frontFoot.transform.position.y);
            body.GetComponent<Rigidbody2D>().AddForce(jumpDirection);
            body.transform.Rotate(0, 0, rotationRate);
            yield return null;
        }

        while (!frontHand.GetComponent<ToggleCollider>().IsColliding())
        {
            yield return null;
        }
        
        state = FrogState.LANDING;
    }
    
    // Update is called once per frame
    void Start()
    {
        targetOffset = handReferencePoint.transform.position - frontFootTarget.transform.position;
        StartCoroutine(Jump());
    }

    private void AdjustTargets()
    {
        Vector3 targetTarget = handReferencePoint.transform.position - targetOffset;
        Vector3 currentKnee = frontFoot.transform.position;
        Vector3 pos = Vector3.MoveTowards(currentKnee, targetTarget, fixLegsRate);
        frontFootTarget.transform.position = pos;
        backFootTarget.transform.position = pos;
        if (Vector3.Distance(frontFootTarget.transform.position, targetTarget) < 0.01f)
        {
            state = FrogState.IDLE;
        }
    }

    private void SetFlip(bool flippedLeft)
    {
        if (this.flippedLeft != flippedLeft)
        {
            //rotationRate = -rotationRate;
            targetOffset.x = -targetOffset.x;
            jumpDirection.x = -jumpDirection.x;
            body.transform.localScale = new Vector3(-body.transform.localScale.x, body.transform.localScale.y, body.transform.localScale.z);
            this.flippedLeft = flippedLeft;
        }
    }

    private void FixedUpdate()
    {
        if (state == FrogState.LANDING)
        {
            AdjustTargets();
        }
        
        if (Input.GetKey(KeyCode.D))
        {
            SetFlip(false);
        }
        else if (Input.GetKey(KeyCode.A))
        {
            SetFlip(true);
        }

        if (state == FrogState.IDLE && (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.A)))
        {
            StartCoroutine(Jump());
        }
    }
}
