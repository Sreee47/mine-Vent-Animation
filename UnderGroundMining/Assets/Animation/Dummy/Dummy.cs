using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dummy : MonoBehaviour {
    Animator anim;
    public int val = 0;
	// Use this for initialization
	void Start () {
        anim = GetComponent<Animator>();

	}
	
	// Update is called once per frame
	void Update () {
		if(val == 1)
        {
            anim.SetBool("Cube", true);
            anim.SetBool("Sphere", false);
           
        }
        if(val == 2)
        {
            anim.SetBool("Cube", false);
            anim.SetBool("Sphere", true);
        }
       
	}
    
}
