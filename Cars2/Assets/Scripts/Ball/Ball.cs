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

    public CarController other;
    public LevelManager LM;


    // Use this for initialization
    void Start()
    {
        ball = GetComponent<Rigidbody>();
        car = CarPhysics.body;
        originalP = transform.position;
        originalR = transform.rotation;


    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name == "Car")
        {
            //Physics.IgnoreCollision(car.GetComponent<Collider>(), ball.GetComponent<Collider>(), true);

            
            //heading = ball.position - car.position;
            //distance = heading.magnitude;
            //direction = heading / distance;
            //ball.velocity = ball.velocity.magnitude * direction;  
        }        
    }

	// Update is called once per frame
	void FixedUpdate () {
        car = CarPhysics.body;
        Physics.IgnoreCollision(car.GetComponent<Collider>(), ball.GetComponent<Collider>(), false);
      
        if (ball.position.z >= 336.76)
        {
            Reset();
            LM.blueGol();
        }
        else if (ball.position.z <= 43.16)
        {
            Reset();
            LM.orangeGol();
        }
	}

    void Reset ()
    {
        transform.position = originalP;
        transform.rotation = originalR;
        ball.velocity = new Vector3 (0,0,0);
        ball.angularVelocity = new Vector3(0, 0, 0);

        other = GameObject.FindObjectOfType(typeof(CarController)) as CarController;

        other.Reset();

        /*other.GetComponent("CarController") = 40;
        car.transform.position = new Vector3(373.8f, 1.07f, 277.1f); //se tiene que poner a mano, nose porque...
        car.transform.rotation = new Quaternion(0, 180, 0, 0);
        car.velocity = new Vector3(0, 0, 0);
        car.angularVelocity = new Vector3(0, 0, 0);*/

    }
}
