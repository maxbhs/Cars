using UnityEngine;
using System.Collections;

public class Ball : MonoBehaviour {

    Rigidbody ball;
    Rigidbody car;
    float speed;
    Vector3 heading;
    float distance;
    Vector3 direction;
    int delay = 5;
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
            Physics.IgnoreCollision(car.GetComponent<Collider>(), ball.GetComponent<Collider>(), true);

           
            speed = (car.velocity - ball.velocity).magnitude;
            heading = ball.position - car.position;
            distance = heading.magnitude;
            direction = heading / distance;
            ball.velocity += (direction * speed * 0.5f);
        }        
    }

	// Update is called once per frame
	void FixedUpdate () {
        car = CarController.body;
        Physics.IgnoreCollision(car.GetComponent<Collider>(), ball.GetComponent<Collider>(), false);
      
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
