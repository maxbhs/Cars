using UnityEngine;
using System.Collections;

public class CarControllerIA : MonoBehaviour {


    static public float thrust = 0.0f;
    static public float turnValue = 0.0f;
    static public float boostFactor = 0.0f;

    public GameObject ball;
    public GameObject net;
    public GameObject homenet;

    private float deadZone = 0.0f;

    float forwardAcceleration;
    float reverseAcceleration;

    Vector3 originalP;
    Quaternion originalR;

    //target Position
    Vector3 target;

    Vector3 hnet;
    Vector3 hball;
    Vector3 hballnet;
    Vector3 htarget;

    float dball, dnet, dballnet, dtarget;

    Vector3 directionball;
    Vector3 directionnet;
    Vector3 directarget;

    private float acceleration;
    private float turnAxis;

	// Use this for initialization
	void Start () {
        forwardAcceleration = CarPhysicsIA.forwardAcceleration;
        reverseAcceleration = CarPhysicsIA.reverseAcceleration;

        originalP = transform.position;
        originalR = transform.rotation;
	}
	
	// Update is called once per frame
	void FixedUpdate () {

        acceleration = 0.0f;
        turnAxis = 0.0f;

        //Direccio porteria, distancia porteria
        hnet = net.transform.position - transform.position;
        dnet = hnet.magnitude;
        directionnet = hnet / dnet;
        directionnet.y = 0.0f;

        //Direccio bola, distancia bola
        hball = ball.transform.position - transform.position;
        dball = hball.magnitude;
        directionball = hball / dball;
        directionball.y = 0.0f;

        //Distancia entre portaria i bola
        hballnet = net.transform.position - ball.transform.position;
        dballnet = hballnet.magnitude;
 

        if (dballnet+10 < dnet) {
            if (dnet > 150)
            {
                target = ball.transform.position;
            }
            else {
                if (goalPosition(transform.position))
                {
                    target = ball.transform.position;
                }
                else {
                    target = findPosition();
                }
            }
        }
        else {
            target = homenet.transform.position;
        }

        getTarget(target);

        if (acceleration >= 0.0f)
            thrust = acceleration * forwardAcceleration;
        else if (acceleration < 0.0f)
            thrust = acceleration * reverseAcceleration;

        
        turnValue = 0.0f;
        if (Mathf.Abs(turnAxis) > deadZone)
        {
                turnValue = turnAxis;
        }
     
	}

    public bool goalPosition(Vector3 position) {
        return false;
    }

    public Vector3 findPosition() {
        return ball.transform.position;
    }

    public void getTarget(Vector3 position) {
        htarget = position - transform.position;
        dtarget = htarget.magnitude;
        directarget = htarget / dtarget;
        directarget.y = 0.0f;

        //acceleration = 1.0f;
        float angle = Vector3.Angle(htarget, transform.forward);
        

       /* if (transform.forward.x > directarget.x)
        {
            turnAxis = 1.0f;
        }
        else if (transform.forward.z < directarget.z)
            turnAxis = -1.0f;
        else turnAxis = 0.0f;*/
    }

    public void Reset()
    {
        transform.position = originalP;
        transform.rotation = originalR;
        GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);
        GetComponent<Rigidbody>().angularVelocity = new Vector3(0, 0, 0);

    }
}
