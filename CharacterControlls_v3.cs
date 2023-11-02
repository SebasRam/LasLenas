using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterControlls_v3 : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed;
    public Transform orientation;
    public float groundDrag;
    public float maxSpeed;
    public float gravity;

    [Header("Booster")]
    public float boosterCoolDown;
    public float boostForce;
    public bool readyToBoost;

    [Header("Jumping")]
    public float jumpForce;
    public float jumpCoolDown;
    public float airMultiplier;
    public bool readyToJump;

    [Header("Ground Check")]
    public float playerHeight;
    public LayerMask whatIsGround;
    bool grounded;

    [Header("KeyBindings")]
    public KeyCode jumpKey = KeyCode.Space;
    public KeyCode boosterKey = KeyCode.LeftShift;

    [Header("GameObjects")]
    public GameObject camControls;
    private CameraControl camObj;
    public KeyCode camControl = KeyCode.C;

    [Header("PlayerStats Object")]
    private CharacterStats charStatsObj;

    [Header("Hovering")]
    public float hoverHeight = 1.0f;
    public float terrainHeight;
    private Vector3 tempPos;

    //These variables are used to controll the character's animations
    [Header("CharacterMesh")]
    public GameObject charMesh;
    private Animator charAnimator;

    float horizontalInput;
    float verticalInput;

    Vector3 moveDirection;

    Rigidbody rb;

    Quaternion targetRotation;


    //Ground align variables
    private RaycastHit hit;
    public Transform raycastPoint;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation=true;
        this.readyToJump = true;
        this.readyToBoost = true;
        this.camObj = this.camControls.GetComponent<CameraControl>();
        this.charStatsObj = this.GetComponent<CharacterStats>();
        this.charAnimator = this.charMesh.GetComponent<Animator>();
        //this.raycastPoint = this.transform;
        Application.targetFrameRate = 35;
        QualitySettings.vSyncCount = 0;

    }

    //rotate the character's normal to aling with the terrain's normal
    public void groundAlign()
    {
        Physics.Raycast(raycastPoint.position, Vector3.down, out hit);
        //Debug.Log(hit.normal);
        transform.up -= (transform.up - hit.normal) * 0.01f;
        
    }
    public void groundAlign2()
    {
        Physics.Raycast(raycastPoint.position, Vector3.down, out hit);
        Debug.Log(hit.normal);
        //transform.up -= (transform.up - hit.normal) * 0.01f;
        this.targetRotation = Quaternion.FromToRotation(transform.up, hit.normal) * transform.rotation;
        Debug.Log(this.targetRotation);
        transform.rotation = Quaternion.Lerp(transform.rotation, this.targetRotation, 0.0001f / Time.deltaTime);
    }
    public void groundAlign3()
    {
        Physics.Raycast(raycastPoint.position, Vector3.down, out hit);
        Debug.Log("normal"+hit.normal);
        //transform.up -= (transform.up - hit.normal) * 0.01f;
        this.targetRotation = Quaternion.FromToRotation(transform.up, hit.normal) * transform.rotation;
        //Debug.Log(this.targetRotation);
        Quaternion temp = Quaternion.Lerp(transform.rotation, this.targetRotation, 0.001f / Time.deltaTime);
        //this.charMesh.transform.rotation = temp;
        Vector3 tempv3 = temp.eulerAngles;
        Debug.Log(tempv3);
        this.charMesh.transform.rotation = Quaternion.Euler(temp.eulerAngles.x, this.charMesh.transform.eulerAngles.y, temp.eulerAngles.z);
    }


    public void groundHug()
    {
        // Keep at specific height above terrain
        this.tempPos = this.transform.position;
        float terrainHeight = Terrain.activeTerrain.SampleHeight(this.tempPos);
        this.transform.position = new Vector3(this.tempPos.x,terrainHeight + hoverHeight, this.tempPos.z);
    }

    private void myInput()
    {
        this.horizontalInput = Input.GetAxis("Horizontal");
        this.verticalInput = Input.GetAxis("Vertical");
        //Debug.Log("hi:"+this.horizontalInput+" vi:"+this.verticalInput);
        if (Input.GetKey(this.jumpKey) && this.readyToJump && this.grounded)
        {
            
            this.jump();
            FindObjectOfType<AudioManager>().Play("Jump");

            //the invoke function is used to call a method after a certain amount of time
            Invoke(nameof(this.resetJump),this.jumpCoolDown);
        }
        if (Input.GetKey(this.boosterKey) && this.readyToBoost)
        {
            this.boost();
            FindObjectOfType<AudioManager>().Play("Boost");

            Invoke(nameof(this.resetBoost),this.boosterCoolDown);
        }
        
        

    }


    private void movePlayer()
    {
        //calculate movement direction
        //this.moveDirection = orientation.forward * -this.horizontalInput + orientation.right * this.verticalInput;
        this.moveDirection = orientation.forward * this.verticalInput  + orientation.right * this.horizontalInput;
        
        if (this.horizontalInput<-0.3)
        {
            this.charAnimator.SetBool("IsMovingLeft",true);
        }
        if (this.horizontalInput>0.3)
        {
            this.charAnimator.SetBool("IsMovingRight", true);
        }
        if (this.horizontalInput <=0.3 && this.horizontalInput >= -0.3)
        {
            this.charAnimator.SetBool("IsMovingRight", false);
            this.charAnimator.SetBool("IsMovingLeft", false);
        }
        //applies a different amount of force to the character if it is touching the ground or when it's jumping.
        if (this.grounded)
        {
            this.rb.AddForce(this.moveDirection * this.moveSpeed * 10f, ForceMode.Force);
            
            
        }
        else if (!this.grounded)
        {
            this.rb.AddForce(this.moveDirection * this.moveSpeed * 10f * this.airMultiplier, ForceMode.Force);
            this.rb.AddForce(transform.up * -this.gravity, ForceMode.Force);
        }
        //Debug.Log("MoveDir:"+this.moveDirection+" velocity:"+this.rb.velocity);

    }

    private void speedControl()
    {
        Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
        this.charStatsObj.setSpeed(flatVel.magnitude);
        //Debug.Log(flatVel.magnitude);
        //limit velocity if needed
        if (flatVel.magnitude > this.maxSpeed)
        {
            Vector3 limitedVel = flatVel.normalized * this.maxSpeed;
            rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
        }
    }

    private void jump()
    {
        this.readyToJump = false;
        //reset y velocity to ensure that the character always jumps the same hieght
        rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);

        //the forcemode.impuse is used to only apply the force once
        rb.AddForce(transform.up * this.jumpForce, ForceMode.Impulse);
    }

    private void resetJump()
    {
        this.readyToJump = true;
    }

    private void boost()
    {
        this.readyToBoost = false;
        //the forcemode.impuse is used to only apply the force once
        this.rb.AddForce(this.moveDirection * this.boostForce, ForceMode.Impulse);
        this.charAnimator.SetTrigger("Boost");
        //this.camObj.cameraZoomOut();
    }

    private void resetBoost()
    {
        this.readyToBoost = true;
        this.charAnimator.ResetTrigger("Boost");
    }

    private void FixedUpdate()
    {
        //this.groundHug();
        this.movePlayer();
        
    }

    

    // Update is called once per frame
    void Update()
    {
        
        this.myInput();
        

        //ground check;
        this.grounded = Physics.Raycast(transform.position, Vector3.down, this.playerHeight + 0.1f, this.whatIsGround);
        //Debug.Log(this.grounded);
        Debug.DrawRay(transform.position, Vector3.down, Color.red);
        if (this.grounded)
        {
            rb.drag = this.groundDrag;
        }
        else
        {
            rb.drag = 0;
        }
        this.speedControl();

        float terrainHeight = Terrain.activeTerrain.SampleHeight(this.transform.position);
        float normalizedX = this.transform.position.x / Terrain.activeTerrain.terrainData.size.x;
        float normalizedY = this.transform.position.z / Terrain.activeTerrain.terrainData.size.z;

        //Debug.Log(Terrain.activeTerrain.terrainData.GetSteepness(normalizedX,normalizedY));
        //this.groundAlign3();
        //this.Align();
    }

    private void LateUpdate()
    {
        
    }
}
