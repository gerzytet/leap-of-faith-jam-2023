/*
@Authors - Craig and Patrick
@Description - Handles frog movement
*/

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
    [SerializeField] private GameObject body;
    [SerializeField] private float rotationRate;
    [SerializeField] private GameObject frontFoot;
    [SerializeField] private GameObject backFoot;
    [SerializeField] private GameObject handReferencePoint;
    [SerializeField] private GameObject handRootPoint;
    [SerializeField] private GameObject upperArm;
    [SerializeField] private GameObject frontHand;

    public const int MAX_JUMP = 500;

    private Vector3 targetOffset;
    public Vector2 jumpDirection;
    public Vector2 hopDirection;

    private FrogState state = FrogState.IDLE;
    private int count = 0;
    public float fixLegsRate;
    public float timeScale;
    public int airtime = 0;

    private bool flippedLeft = false;
    public Checkpoint currentCheckpoint;

    public static Frog instance;

    void Awake()
    {
        instance = this;
    }


    //if hopping is false, we know player is jumping    
    IEnumerator Jump(Vector2 jumpVector, bool hopping)
    {
        state = FrogState.JUMPING;
        float iterationCount = 0;
        airtime = 0;
        
        //!checks if frog body is on the ground

        //jumping
        while (frontFoot.GetComponent<ToggleCollider>().IsColliding() || iterationCount < 5)
        {
            iterationCount++;
            body.GetComponent<Rigidbody2D>().AddForce(jumpVector);
            body.transform.Rotate(0, 0, rotationRate);
            //Debug.Log("Jumping " + count);
            yield return new WaitForFixedUpdate();
        }

        //airtime
        while (!frontHand.GetComponent<ToggleCollider>().IsColliding())
        {
            ProcessArms();
            FixBodyRotation();
            //Debug.Log("waiting to land: " + count);
            if(!hopping){
                airtimeJumpMovement();
            }
            yield return new WaitForFixedUpdate();
        }
        
        //landing
        state = FrogState.LANDING;
    }
    
    // Update is called once per frame
    void Start()
    {
        targetOffset = handReferencePoint.transform.position - frontFootTarget.transform.position;
        //StartCoroutine(Jump());
    }

    //tucks legs back
    private void AdjustTargets(bool instant = false)
    {
        //Debug.Log("Adjusting targets " + count);
        Vector3 targetTarget = handReferencePoint.transform.position - targetOffset;
        Vector3 currentKnee = frontFoot.transform.position;
        Vector3 pos = instant ? targetTarget : Vector3.MoveTowards(currentKnee, targetTarget, fixLegsRate);
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
            hopDirection.x = -hopDirection.x;
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

    private void airtimeJumpMovement() {
        if(airtime < MAX_JUMP && Input.GetKey(KeyCode.Space)){
            airtime++;
            body.GetComponent<Rigidbody2D>().AddForce(new Vector2(0, 30));
        }
    }

    private void ProcessArms()
    {
        float angle = Vector2.SignedAngle(Vector2.right,
            handReferencePoint.transform.position - handRootPoint.transform.position);
        Debug.Log(angle + " " + handRootPoint.transform.position + " " + handReferencePoint.transform.position);
        float newAngle = Mathf.MoveTowardsAngle(angle, -90, 6);
        float offset = newAngle - angle;
        float upperArmAngle = upperArm.transform.rotation.eulerAngles.z;
        float newUpperArmAngle = upperArmAngle + offset;
        newUpperArmAngle = Mathf.Clamp(newUpperArmAngle, 210, 315);
        upperArm.transform.rotation = Quaternion.Euler(0, 0, newUpperArmAngle);
    }

    private void FixBodyRotation()
    {
        float bodyAngle = body.transform.rotation.eulerAngles.z;
        if (bodyAngle is < -5 or > 25)
        {
            body.transform.rotation = Quaternion.Euler(0, body.transform.rotation.eulerAngles.y, Mathf.LerpAngle(bodyAngle, 0, 0.1f));
        }
    }

    public void TeleportToCheckpoint(Checkpoint checkpoint)
    {
        body.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        body.GetComponent<Rigidbody2D>().angularVelocity = 0;
        body.transform.position = checkpoint.transform.position;
        body.transform.rotation = Quaternion.Euler(body.transform.rotation.eulerAngles.x, body.transform.rotation.eulerAngles.y, 0);
        AdjustTargets(instant: true);
        SetFlip(checkpoint.flippedLeft);
    }

    public void Respawn()
    {
        TeleportToCheckpoint(currentCheckpoint);
    }

    public void SetCheckpoint(Checkpoint checkpoint)
    {
        currentCheckpoint = checkpoint;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            Respawn();
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

            if(Input.GetKey(KeyCode.Space)){
                StartCoroutine(Jump(jumpDirection, false));
            }
            else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.A)) {
                state = FrogState.JUMPING;
                Debug.Log("Starting jump coroutine " + count);
                StartCoroutine(Jump(hopDirection, true));
            }
        }

        Time.timeScale = this.timeScale;
    }
}
