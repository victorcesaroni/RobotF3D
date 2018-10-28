using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {
    public GameObject ball;
    public GameObject defend;
    public GameObject goal;

    Rigidbody rigidbody;

	// Use this for initialization
	void Start () {
        rigidbody = GetComponent<Rigidbody>();
	}
    
	// Update is called once per frame
	void Update ()
    {
        bool shouldGo = Random.Range(0, 20) == 0;
        float distanceToBall = (ball.transform.position - transform.position).magnitude;
        float distanceToGoal = (goal.transform.position - transform.position).magnitude;
        float distanceToDefend = (goal.transform.position - transform.position).magnitude;

        if (shouldGo)
        {
            float force = Random.Range(10000.0f, 50000.0f);

            transform.LookAt(ball.transform);
            rigidbody.AddRelativeForce(0, 0, 10000.0f);

            Vector2 forward = (ball.transform.position - transform.position).normalized;
            rigidbody.AddForce(forward * force * Time.smoothDeltaTime);
        }
	}
}
