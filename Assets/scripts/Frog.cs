/*
@Authors - Craig and Patrick
@Description - Handles frog movement
*/

using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

//!Only works if you don't care about joystick continuous values
/*Input Guide:

Jumping - (Input.GetKeyDown(KeyCode.Space) || Input.GetKey(KeyCode.JoystickButton0)
Left - (Input.GetKey(KeyCode.A) || (Input.GetAxisRaw("Horizontal") < 0 && Input.GetAxisRaw("Horizontal") >= -1) || (Input.GetAxisRaw("Debug Horizontal") == -1))
Right - (Input.GetKey(KeyCode.D) || (Input.GetAxisRaw("Horizontal") > 0 && Input.GetAxisRaw("Horizontal") <= 1) || (Input.GetAxisRaw("Debug Horizontal") == 1))
Teleport to Checkpoint - (Input.GetKeyDown(KeyCode.R) || Input.GetKey(KeyCode.JoystickButton2))

*/

/*CONTROLLER KEYS
Bottom Face Button = Joystick 0
Left Face Button = Joystick 2
D-PAD = Input.GetAxis("Debug Horizontal")
Joystick = Input.GetAxis("Horizontal")
*/


public class Frog : MonoBehaviour
{


    public bool jumpButtonPressed(){
        return (Input.GetKeyDown(KeyCode.Space) || Input.GetKey(KeyCode.JoystickButton0));
    }

    public bool leftButtonPressed(){
        return (Input.GetKey(KeyCode.A) || (Input.GetAxisRaw("Horizontal") < 0 && Input.GetAxisRaw("Horizontal") >= -1) || (Input.GetAxisRaw("Debug Horizontal") == -1));
    }

    public bool rightButtonPressed(){
        return (Input.GetKey(KeyCode.D) || (Input.GetAxisRaw("Horizontal") > 0 && Input.GetAxisRaw("Horizontal") <= 1) || (Input.GetAxisRaw("Debug Horizontal") == 1));
    }

    public bool teleportButtonPressed(){
        return ((Input.GetKeyDown(KeyCode.R) || Input.GetKey(KeyCode.JoystickButton2)));
    }

    public enum FrogState
    {
        JUMPING,
        LANDING,
        MIDAIR,
        IDLE,
        HANGING,
        SPINNING
    }

    [SerializeField] private GameObject bodyBox;
    [SerializeField] private GameObject backFootTarget;
    [SerializeField] private GameObject frontFootTarget;
    [SerializeField] private GameObject body;
    [SerializeField] private float rotationRate;
    [SerializeField] private GameObject frontFoot;
    [SerializeField] private GameObject backFoot;

    [SerializeField] private GameObject handReferencePoint;

    //[SerializeField] private GameObject handRootPoint;
    [SerializeField] private GameObject upperFrontArm;

    [SerializeField] private GameObject upperBackArm;
    //[SerializeField] private GameObject frontHand;

    public const int MAX_JUMP = 1000;

    private Vector3 targetOffset;
    public Vector2 jumpDirection;
    public Vector2 hopDirection;
    public Vector2 airHorizontalVector;

    public FrogState state = FrogState.IDLE;
    private int count = 0;
    public float fixLegsRate;
    public float timeScale;
    public int airtime = 0;

    private bool flippedLeft = false;
    public Checkpoint currentCheckpoint;

    // float DPADHorizontal = Input.GetAxis("Horizontal");

    // public bool leftDPADDown() {
    //     return (DPADHorizontal < 0 && DPADHorizontal >= -1);
    // }

    // public bool rightDPADDown() {
    //     return (DPADHorizontal > 0 && DPADHorizontal <= 1);
    // }

    //(Input.GetAxis("Horizontal") < 0 && Input.GetAxis("Horizontal") >= -1)

    //(Input.GetAxisRaw("Debug Horizontal") == -1)

    public static Frog instance;

    void Awake()
    {
        instance = this;
    }

    private void FreezeTargets()
    {
        backFootTarget.transform.parent.transform.SetParent(transform);
        frontFootTarget.transform.parent.transform.SetParent(transform);
    }

    private void UnlockTargets()
    {
        backFootTarget.transform.parent.transform.SetParent(bodyBox.transform);
        frontFootTarget.transform.parent.transform.SetParent(bodyBox.transform);
    }

    //if hopping is false, we know player is jumping
    void StartJump(Vector2 jumpVector, bool hopping)
    {
        state = FrogState.JUMPING;
        airtime = 0;
        jumpTime = 0;
        this.jumpVector = jumpVector;
        this.hopping = hopping;
        FreezeTargets();
    }

    void Unspin()
    {
        Debug.Log("Unspinning");
        state = FrogState.MIDAIR;
        bodyBox.transform.rotation = Quaternion.Euler(0, bodyBox.transform.rotation.eulerAngles.y, 0);
        bodyBox.GetComponent<Rigidbody2D>().angularVelocity = 0f;
        bodyBox.GetComponent<Rigidbody2D>().constraints |= RigidbodyConstraints2D.FreezeRotation;
        AdjustTargets(instant: true, outstretched:true);
        grabbedVine = null;
        SetArmsAngle(startArmAngle);
    }

    private int jumpTime = 0;
    private Vector2 jumpVector;
    private bool hopping;
    private float lastZRotation = 0f;
    void JumpUpdate()
    {
        // Debug.Log("Hor Vel: " + airHorizontalVector.x);
        //jumping, airtime, and landing
        switch (state)
        {
            case FrogState.JUMPING:
                //cuts jump off early if player isn't holding down jump
                if (!hopping && !jumpButtonPressed()){
                    break;
                }
                
                bodyBox.GetComponent<Rigidbody2D>().AddForce(jumpVector);
                jumpTime++;
                if (jumpTime >= 5)
                {
                    UnlockTargets();
                    state = FrogState.MIDAIR;
                }
                break;
            case FrogState.MIDAIR:
                if(!hopping){
                    airtimeJumpMovement();
                }

                if (bodyBox.GetComponent<BodyBoxTrigger>().floorContacts > 0 && bodyBox.GetComponent<Rigidbody2D>().velocity.y < 0.1f)
                {
                    FreezeTargets();
                    state = FrogState.LANDING;
                }

                break;
            case FrogState.SPINNING:
                spinTime++;

                if (spinTime >= MAX_SPIN_TIME)// && lastZRotation > 0f && bodyBox.transform.rotation.eulerAngles.z < 0f)
                {
                    Unspin();
                }

                //lastZRotation = bodyBox.transform.rotation.eulerAngles.z;

                break;
        }
    }
    
    // Update is called once per frame
    void Start()
    {
        targetOffset = handReferencePoint.transform.position - frontFootTarget.transform.position;
        //StartCoroutine(Jump());
    }

    //tucks legs back
    private void AdjustTargets(bool instant = false, bool outstretched = true)
    {
        //Debug.Log("Adjusting targets " + count);
        Vector3 targetTarget = handReferencePoint.transform.position - targetOffset;
        if (outstretched)
        {
            targetTarget -= targetOffset * 0.7f;
        }
        Vector3 currentKnee = frontFoot.transform.position;
        Vector3 pos = instant ? targetTarget : Vector3.MoveTowards(currentKnee, targetTarget, fixLegsRate);
        frontFootTarget.transform.position = pos;
        backFootTarget.transform.position = pos;
        if (!instant && Vector3.Distance(frontFootTarget.transform.position, targetTarget) < 0.01f)
        {
            state = FrogState.IDLE;
        }
    }

    private void SetFlip(bool flippedLeft)
    {
        if (this.flippedLeft != flippedLeft)
        {
            bodyBox.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            bodyBox.GetComponent<Rigidbody2D>().angularVelocity = 0;
            //rotationRate = -rotationRate;
            targetOffset.x = -targetOffset.x;
            hopDirection.x = -hopDirection.x;
            bodyBox.transform.rotation = Quaternion.Euler(bodyBox.transform.rotation.eulerAngles.x, flippedLeft ? 180 : 0, bodyBox.transform.rotation.eulerAngles.z);
            float frontFootTargetOffset = bodyBox.transform.position.x - frontFootTarget.transform.position.x;
            frontFootTarget.transform.position = new Vector3(bodyBox.transform.position.x + frontFootTargetOffset, frontFootTarget.transform.position.y, frontFootTarget.transform.position.z);
            float backFootTargetOffset = bodyBox.transform.position.x - backFootTarget.transform.position.x;
            backFootTarget.transform.position = new Vector3(bodyBox.transform.position.x + backFootTargetOffset, backFootTarget.transform.position.y, backFootTarget.transform.position.z);
            
            frontFoot.transform.localScale = new Vector3(-frontFoot.transform.localScale.x, frontFoot.transform.localScale.y, frontFoot.transform.localScale.z);
            backFoot.transform.localScale = new Vector3(-backFoot.transform.localScale.x, backFoot.transform.localScale.y, backFoot.transform.localScale.z);
            this.flippedLeft = flippedLeft;
        }
    }

    private const float startArmAngle = -92f;
    private const float grabArmAngle = -35f;

    private VinePoint grabbedVine;

    private void SetArmsAngle(float angle)
    {
        upperBackArm.transform.rotation = Quaternion.Euler(upperBackArm.transform.rotation.eulerAngles.x,
            upperBackArm.transform.rotation.eulerAngles.y, angle);
        upperFrontArm.transform.rotation = Quaternion.Euler(upperFrontArm.transform.rotation.eulerAngles.x,
            upperFrontArm.transform.rotation.eulerAngles.y, angle);
    }
    public void GrabVine(VinePoint vine)
    {
        if (grabbedVine == vine)
        {
            return;
        }

        SetArmsAngle(grabArmAngle);
        bodyBox.GetComponent<Rigidbody2D>().constraints &= ~RigidbodyConstraints2D.FreezeRotation;
        bodyBox.transform.rotation = Quaternion.Euler(0, bodyBox.transform.rotation.eulerAngles.y, 90);
        vine.grabJoint.enabled = true;
        vine.grabJoint.connectedBody = bodyBox.GetComponent<Rigidbody2D>();
        vine.grabJoint.connectedAnchor =bodyBox.transform.InverseTransformPoint(handReferencePoint.transform.position);
        state = FrogState.HANGING;
        grabbedVine = vine;
        /*new Vector2(1.6f,
                -0.35f); //*/
    }

    private int MAX_SPIN_TIME = 25;
    public int spinTime = 0;
    private void ReleaseVine()
    {
        spinTime = 0;
        state = FrogState.SPINNING;
        bodyBox.GetComponent<Rigidbody2D>().angularVelocity = 1000f;
        grabbedVine.grabJoint.enabled = false;
        bodyBox.GetComponent<Rigidbody2D>().AddForce(jumpDirection * 2f);
        AdjustTargets(instant: true);
        hopping = false;
    }

    private void airtimeJumpMovement() {
        Vector2 deltaVec = new Vector2(0, 0);
        
        if(airtime < MAX_JUMP && jumpButtonPressed()){
            airtime++;
            
            //Please don't change this value before talking to me!
            deltaVec.y += 85;
        }
        
        //snappy horiztonal direction changing. If using DPAD, movement will depend on how hard you are pressing.
        if (Input.GetKey(KeyCode.A) || (Input.GetAxisRaw("Horizontal") < 0 && Input.GetAxisRaw("Horizontal") >= -1) || (Input.GetAxisRaw("Debug Horizontal") == -1)){
            //left key and not currently moving right
            if (bodyBox.GetComponent<Rigidbody2D>().velocity.x <= 0 && (Input.GetKey(KeyCode.A) || (Input.GetAxisRaw("Debug Horizontal") == -1))){
                // Debug.Log("We are moving left, and hor vel is " + airHorizontalVector.x);
                deltaVec.x -= airHorizontalVector.x;
            }
            else if (bodyBox.GetComponent<Rigidbody2D>().velocity.x <= 0 && (Input.GetAxisRaw("Horizontal") < 0 && Input.GetAxisRaw("Horizontal") >= -1)){
                //need to add cause it will already be a negative
                deltaVec.x += (airHorizontalVector.x * Input.GetAxisRaw("Horizontal"));
            }
            else{
                Rigidbody2D rb = bodyBox.GetComponent<Rigidbody2D>();
                //sets current x velocity to zero
                rb.velocity = new Vector3(0, rb.velocity.y);
            }
        }
        else if (Input.GetKey(KeyCode.D) || (Input.GetAxisRaw("Horizontal") > 0 && Input.GetAxisRaw("Horizontal") <= 1) || (Input.GetAxisRaw("Debug Horizontal") == 1)){
            //right key and not currently moving left
            if (bodyBox.GetComponent<Rigidbody2D>().velocity.x >= 0 && (Input.GetKey(KeyCode.D) || (Input.GetAxisRaw("Debug Horizontal") == 1))){
                deltaVec.x += airHorizontalVector.x;
            }else if (bodyBox.GetComponent<Rigidbody2D>().velocity.x >= 0 && (Input.GetAxisRaw("Horizontal") > 0 && Input.GetAxisRaw("Horizontal") <= 1)){
                deltaVec.x += (airHorizontalVector.x * Input.GetAxisRaw("Horizontal"));
            }
            else{
                Rigidbody2D rb = bodyBox.GetComponent<Rigidbody2D>();
                //sets current x velocity to zero
                rb.velocity = new Vector3(0, rb.velocity.y);
            }
        }

        bodyBox.GetComponent<Rigidbody2D>().AddForce(deltaVec);
        // Debug.Log("X vel: " + deltaVec.x);
        // Debug.Log(Input.GetAxisRaw("Horizontal").ToString());
    }

    /*private void ProcessArms()
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
    }*/

    private void FixBodyRotation()
    {
        float bodyAngle = bodyBox.transform.rotation.eulerAngles.z;
        if (bodyAngle is < -5 or > 25)
        {
            bodyBox.transform.rotation = Quaternion.Euler(0, bodyBox.transform.rotation.eulerAngles.y, Mathf.LerpAngle(bodyAngle, 0, 0.1f));
        }

        if (Mathf.Abs(bodyBox.GetComponent<Rigidbody2D>().angularVelocity) > 20)
        {
            bodyBox.GetComponent<Rigidbody2D>().angularVelocity *= 0.8f;
        }
    }

    public void TeleportToCheckpoint(Checkpoint checkpoint)
    {
        bodyBox.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        bodyBox.GetComponent<Rigidbody2D>().angularVelocity = 0;
        bodyBox.transform.position = checkpoint.transform.position;
        //bodyBox.transform.rotation = Quaternion.Euler(bodyBox.transform.rotation.eulerAngles.x, bodyBox.transform.rotation.eulerAngles.y, 0);
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
        if (teleportButtonPressed())
        {
            Respawn();
        }
        if (state == FrogState.HANGING && jumpButtonPressed())
        {
            ReleaseVine();
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

        //main input code
        if (state == FrogState.IDLE) //&& (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.A)))
        {
            if (Input.GetKey(KeyCode.D) || (Input.GetAxisRaw("Horizontal") > 0 && Input.GetAxisRaw("Horizontal") <= 1) || (Input.GetAxisRaw("Debug Horizontal") == 1))
            {
                SetFlip(false);
            }
            else if (Input.GetKey(KeyCode.A) || (Input.GetAxisRaw("Horizontal") < 0 && Input.GetAxisRaw("Horizontal") >= -1) || (Input.GetAxisRaw("Debug Horizontal") == -1))
            {
                SetFlip(true);
            }

            if(jumpButtonPressed()){
                StartJump(jumpDirection, false);
                //StartCoroutine(Jump(jumpDirection, false));
            }
            else if (Input.GetKey(KeyCode.D) ||
                    Input.GetKey(KeyCode.A) ||
                    (Input.GetAxisRaw("Horizontal") < 0 && Input.GetAxisRaw("Horizontal") >= -1) ||
                    (Input.GetAxisRaw("Horizontal") > 0 && Input.GetAxisRaw("Horizontal") <= 1) ||
                    (Input.GetAxisRaw("Debug Horizontal") == -1) ||
                    (Input.GetAxisRaw("Debug Horizontal") == 1)) {
                        state = FrogState.JUMPING;
                        Debug.Log("Starting jump coroutine " + count);
                        StartJump(hopDirection, true);
            }
        }

        if (state == FrogState.HANGING)
        {
            airtimeJumpMovement();
        }

        JumpUpdate();
        

        Time.timeScale = this.timeScale;
    }
}
