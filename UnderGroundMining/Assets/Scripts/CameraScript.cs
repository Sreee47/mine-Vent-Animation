using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour {
    private float minFov  = 15f;
    private float maxFov  = 10000f;
    private float sensitivity = 10f;
    private float speedH = 2.0f;
    private float speedV = 2.0f;
    private float yaw = 0.0f;
    private float pitch = 0.0f;
    private Rect screenRect;



    public Camera mainCamera;



	// Use this for initialization
	void Start () {
         screenRect = new Rect(0, 0, Screen.width, Screen.height);
        
    }
	
	// Update is called once per frame
	void Update () {

        if (screenRect.Contains(Input.mousePosition))
        {
            MoveCamera();
            CameraZoom();

        }

    }
    
    void MoveCamera()
    {
        if (Input.GetAxis("Fire1")> 0f)
        {
            yaw += speedH * Input.GetAxis("Mouse X");
            pitch += speedV * Input.GetAxis("Mouse Y");
            transform.eulerAngles = new Vector3(pitch, yaw, 0.0f);
        }
    }
     
    void CameraZoom()
    {
        float fov = mainCamera.fieldOfView;
        fov += Input.GetAxis("Mouse ScrollWheel") * sensitivity;
        fov = Mathf.Clamp(fov,minFov,maxFov);
        mainCamera.fieldOfView = fov;
    }
}
