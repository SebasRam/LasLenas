using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CameraControl : MonoBehaviour
{

    
    public GameObject characterMesh;
    public GameObject mainCamera;
    public Vector3 relativePosChar;

    [Header("Rotation")]
    public float sensX;
    public float yRotation;
    public float initialCharRotation;
    //public bool controllCam =true;
/*
    [Header("KeyBindings")]
    public KeyCode moveLeft = KeyCode.Q;
    public KeyCode moveRight = KeyCode.E;*/

    //These variables are used to controll the character's animations
    [Header("Animation")]
    public GameObject charparent;
    private Animator charAnimator;
    private CharacterStats charStats;

    public float keyRotationSensitivity = 0.5f;

    


    //Ground align variables
    private RaycastHit hit;
    public Transform raycastPoint;
    Quaternion targetRotation;
    Quaternion tempRotation;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        this.relativePosChar = new Vector3(this.characterMesh.transform.position.x - this.mainCamera.transform.position.x, this.characterMesh.transform.position.y - this.mainCamera.transform.position.y, this.characterMesh.transform.position.z - this.mainCamera.transform.position.z);
        //Debug.Log(this.relativePosChar);
        
        if (SceneManager.GetActiveScene().name=="Game")
        {
            this.yRotation = -160;
        }
        if (SceneManager.GetActiveScene().name == "Track2")
        {
            this.yRotation = -80;
        }

        //this.characterMesh.transform.rotation = Quaternion.Euler(0, -35, 0);
        this.charAnimator = this.characterMesh.GetComponent<Animator>();
        this.charStats = this.charparent.GetComponent<CharacterStats>();
    }

    // Update is called once per frame
    void Update()
    {
        
            float mouseX = Input.GetAxisRaw("Mouse X") * Time.deltaTime * this.sensX;

            this.yRotation += mouseX;
        this.groundAlign3();
        //Modifying the rotation on Z axis dealigns the character with the ground. Leaving this as 0 seems to work better. this.characterMesh.transform.eulerAngles.z
        this.characterMesh.transform.rotation = Quaternion.Euler(this.tempRotation.eulerAngles.x, yRotation+this.initialCharRotation, this.tempRotation.eulerAngles.z);
        
        this.transform.rotation = Quaternion.Euler(0, yRotation, 0);
            if (this.charStats.getSpeed()<=3 && mouseX!=0)
            {
                this.charAnimator.SetBool("LookingAround",true);
            }
            if (this.charStats.getSpeed() > 3 || mouseX == 0)
            {
                this.charAnimator.SetBool("LookingAround", false);
            }
        /*
        if (Input.GetKey(this.moveLeft)) { 
        
            this.yRotation += -1*this.keyRotationSensitivity;
            

            this.characterMesh.transform.rotation = Quaternion.Euler(0, yRotation, 0);
            this.transform.rotation = Quaternion.Euler(0, yRotation, 0);
        }
        if (Input.GetKey(this.moveRight))
        {

            this.yRotation += 1 * this.keyRotationSensitivity;


            this.characterMesh.transform.rotation = Quaternion.Euler(0, yRotation, 0);
            this.transform.rotation = Quaternion.Euler(0, yRotation, 0);
        }
        */
    }

    public void groundAlign3()
    {
        Physics.Raycast(raycastPoint.position, Vector3.down, out hit);
        Debug.Log("normal" + hit.normal);
        //transform.up -= (transform.up - hit.normal) * 0.01f;
        this.targetRotation = Quaternion.FromToRotation(this.characterMesh.transform.up, hit.normal) * this.characterMesh.transform.rotation;
        //Debug.Log(this.targetRotation);
        this.tempRotation = Quaternion.Lerp(this.characterMesh.transform.rotation, this.targetRotation, 7f * Time.deltaTime);
        //this.tempRotation = this.targetRotation;
       
    }

    public void cameraZoomOut()
    {
        Vector3 currentCamPos = this.mainCamera.transform.position;
        this.mainCamera.transform.position = new Vector3(currentCamPos.x -1.26f,currentCamPos.y+2.09f,currentCamPos.z-0.11f);
        Invoke(nameof(this.resetCamPos),3f);
    }

    public void resetCamPos()
    {
        this.mainCamera.transform.position = this.characterMesh.transform.position - this.relativePosChar;
    }

    /*public void toggleCamControl()
    {
        if (!this.controllCam)
        {
            this.controllCam = true;
        }
        else
        {
            this.controllCam = false;
        }
    }*/


}
