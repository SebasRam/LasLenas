using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniMapScript : MonoBehaviour
{

    [Header("GameObjects")]
    public GameObject skier;
    public GameObject arrowMiniMap;
    public GameObject mainCam;

    [Header("Rotation")]
    public float delta_yRotation;

    Transform skierPosition;

    float tempRotation;

    // Start is called before the first frame update
    void Start()
    {
        this.skierPosition = this.skier.GetComponent<Transform>();
        this.delta_yRotation = 0;
        this.tempRotation = this.mainCam.GetComponent<CameraControl>().yRotation;
    }

    // Update is called once per frame
    void Update()
    {
        this.arrowMiniMap.transform.position = new Vector3(this.skierPosition.position.x, this.arrowMiniMap.transform.position.y, this.skierPosition.position.z);
        
        if (Mathf.Abs(this.tempRotation- this.mainCam.GetComponent<CameraControl>().yRotation)>2)
        {
            this.delta_yRotation = this.tempRotation - this.mainCam.GetComponent<CameraControl>().yRotation;
            this.tempRotation= this.mainCam.GetComponent<CameraControl>().yRotation;
            this.arrowMiniMap.transform.Rotate(new Vector3(0, 0, -this.delta_yRotation),Space.Self);
            
        }

        
        
    }
}
