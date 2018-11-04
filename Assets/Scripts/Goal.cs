using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void FixedUpdate ()
    {

    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Ball")
        {
            GetComponentInParent<Football>().OnScore(gameObject);
        }
    }
}
