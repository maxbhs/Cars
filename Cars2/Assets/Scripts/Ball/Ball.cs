using UnityEngine;
using System.Collections;

public class Ball : MonoBehaviour {

    Rigidbody ball;
    Rigidbody car;
    float speed;
    Vector3 direction;
    bool first;

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
            if (first)
            {
                speed = (-ball.velocity + car.velocity).magnitude;
                direction = (ball.position - (car.position + transform.up * -0.5f)).normalized;
                ball.velocity = (direction * speed);
                first = false;
            }
        }
        else first = true;
    }

	// Update is called once per frame
	void FixedUpdate () {
        car = CarController.body;
	}
}
