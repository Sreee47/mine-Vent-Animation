using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VehicleTracker : MonoBehaviour {
    bool active= true;
    bool RFID1active = false;
    public GameObject sphere;
	// Use this for initialization
	void Start () {
  
        sphere.SetActive(RFID1active);
    }
	
	// Update is called once per frame
	void Update () {
        if (RFID1active)
        {
            active = !active;
            sphere.SetActive(active);
        }
	}

    private void OnCollisionEnter(Collision collision)
    {
        
        if (collision.gameObject.tag == "RFID1")
        {
            RFID1active = true;             
        }
    }
    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "RFID1")
        {
            RFID1active = false;
            sphere.SetActive(false); 
        }
    }
}
