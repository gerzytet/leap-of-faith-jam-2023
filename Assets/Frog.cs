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
    [SerializeField] private GameObject backFoot;
    [SerializeField] private GameObject handReferencePoint;
    private Vector3 targetOffset;
    
    [SerializeField] private GameObject frontHand;
    private FrogState state = FrogState.IDLE;
    private int count = 0;
    public float fixLegsRate;

    private bool flippedLeft = false;
    public float timeScale;
    IEnumerator Jump()
    {
        state = FrogState.JUMPING;
        float iterationCount = 0;
        while (frontFoot.GetComponent<ToggleCollider>().IsColliding() || iterationCount < 5)
        {
            iterationCount++;
            body.GetComponent<Rigidbody2D>().AddForce(jumpDirection);
            body.transform.Rotate(0, 0, rotationRate);
            Debug.Log("Jumping " + count);
            yield return new WaitForFixedUpdate();
        }

        while (!frontHand.GetComponent<ToggleCollider>().IsColliding())
        {
            Debug.Log("waiting to land: " + count);
            yield return new WaitForFixedUpdate();
        }
        
        state = FrogState.LANDING;
    }
    
    // Update is called once per frame
    void Start()
    {
        targetOffset = handReferencePoint.transform.position - frontFootTarget.transform.position;
        //StartCoroutine(Jump());
    }

    private void AdjustTargets()
    {
        Debug.Log("Adjusting targets " + count);
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
            body.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            body.GetComponent<Rigidbody2D>().angularVelocity = 0;
            //rotationRate = -rotationRate;
            targetOffset.x = -targetOffset.x;
            jumpDirection.x = -jumpDirection.x;
            body.transform.rotation = Quaternion.Euler(body.transform.rotation.eulerAngles.x, flippedLeft ? 180 : 0, body.transform.rotation.eulerAngles.z);
            float frontFootTargetOffset = body.transform.position.x - frontFootTarget.transform.position.x;
            frontFootTarget.transform.position = new Vector3(body.transform.position.x + frontFootTargetOffset, frontFootTarget.transform.position.y, frontFootTarget.transform.position.z);
            float backFootTargetOffset = body.transform.position.x - backFootTarget.transform.position.x;
            backFootTarget.transform.position = new Vector3(body.transform.position.x + backFootTargetOffset, backFootTarget.transform.position.y, backFootTarget.transform.position.z);
            
            frontFoot.transform.localScale = new Vector3(-frontFoot.transform.localScale.x, frontFoot.transform.localScale.y, frontFoot.transform.localScale.z);
            backFoot.transform.localScale = new Vector3(-backFoot.transform.localScale.x, backFoot.transform.localScale.y, backFoot.transform.localScale.z);
            this.flippedLeft = flippedLeft;
        }
    }

    private void FixedUpdate()
    {
        count++;
        if (count < 50)
        {
            return;
        }
        if (state == FrogState.LANDING)
        {
            AdjustTargets();
        }
        
        

        if (state == FrogState.IDLE) //&& (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.A)))
        {
            if (Input.GetKey(KeyCode.D))
            {
                SetFlip(false);
            }
            else if (Input.GetKey(KeyCode.A))
            {
                SetFlip(true);
            }
            state = FrogState.JUMPING;
            Debug.Log("Starting jump coroutine " + count);
            StartCoroutine(Jump());
        }

        Time.timeScale = this.timeScale;
    }
}
