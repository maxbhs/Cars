using UnityEngine;
using System.Collections;

public class Ball : MonoBehaviour {

    Rigidbody ball;
    Rigidbody car;

	// Use this for initialization
    void Start()
    {
        ball = GetComponent<Rigidbody>();
        car = CarController.body;

    }

    void OnCollisionEnter(Collision collision)
    {
        // Debug-draw all contact points and normals
        if (collision.gameObject.name == "Car")
            {
                //ball.AddForceAtPosition(Vector3.Dot(ball.velocity, car.velocity) * ball.centerOfMass, transform.position, ForceMode.Impulse);
            }
    }

	// Update is called once per frame
	void FixedUpdate () {
        car = CarController.body;
	}
}
