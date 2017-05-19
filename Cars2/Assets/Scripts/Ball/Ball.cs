using UnityEngine;
using System.Collections;

public class Ball : MonoBehaviour {

    Rigidbody ball;
    Rigidbody car;
    float speed;
    Vector3 direction;
    bool first;
    Vector3 originalP;
    Quaternion originalR;
    CarController resetC = new CarController();


	// Use this for initialization
    void Start()
    {
        ball = GetComponent<Rigidbody>();
        car = CarController.body;
        originalP = transform.position;
        originalR = transform.rotation;


    }

    void OnCollisionEnter(Collision collision)
    {
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

        if (ball.position.z >= 336.76)
        {
            Reset();
            resetC.Reset();
        }
        else if (ball.position.z <= 43.16)
        {
            Reset();
            resetC.Reset();
        }
	}

    void Reset ()
    {
        transform.position = originalP;
        transform.rotation = originalR;
        ball.velocity = new Vector3 (0,0,0);
        ball.angularVelocity = new Vector3(0, 0, 0);

    }
}
