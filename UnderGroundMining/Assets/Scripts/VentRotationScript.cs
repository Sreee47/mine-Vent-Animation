using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VentRotationScript : MonoBehaviour {
    public GameObject fan;
    private int speed=100;
    int noOfPeople = 4;
    // Use this for initialization
    void Start () {
        ControleSpeed(noOfPeople);
	}
	
	// Update is called once per frame
	void Update () {
        
        
        fan.transform.Rotate(0,0,speed*Time.deltaTime);
	}
    public void ControleSpeed(int people)
    {
        speed = 100 * people;
    }
    
}
