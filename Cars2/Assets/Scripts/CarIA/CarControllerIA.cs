using UnityEngine;
using System.Collections;

public class CarControllerIA : MonoBehaviour {


    static public float thrust = 0.0f;
    static public float turnValue = 0.0f;
    static public float boostFactor = 0.0f;
    static public bool goalPosition = false;

    public GameObject ball;
    public GameObject net;
    public GameObject homenet;

    private float deadZone = 0.0f;

    float forwardAcceleration;
    float reverseAcceleration;

    Vector3 originalP;
    Quaternion originalR;

    Vector3 netposition;
    Vector3 homenetposition;

    Vector3 hball;
    Vector3 hnet;
    Vector3 hhomenet;
    Vector3 hballnet;
    Vector3 htarget;

    float dball, dnet, dhomenet, dballnet, dtarget;

    Vector3 directionball;
    Vector3 directionnet;
    Vector3 directionhomenet;
    Vector3 directarget;
    Vector3 directionballnet;

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
        
        //Direccio coche centre porteria contraria
        netposition = net.transform.position + new Vector3(0.0f, 25.6f, 0.0f);
        hnet = netposition - transform.position;
        dnet = hnet.magnitude;
        directionnet = hnet / dnet;

        //Direccio coche centre porteria casa
        homenetposition = homenet.transform.position + new Vector3(0.0f, 25.6f, 20.0f);
        hhomenet = homenetposition - transform.position;
        dhomenet = hhomenet.magnitude;
        directionhomenet = hhomenet / dhomenet;

        //Direccio bola, distancia bola
        hball = ball.transform.position - transform.position;
        dball = hball.magnitude;
        directionball = hball / dball;

        //Distancia bola i centre portaria contraria
        hballnet = netposition - ball.transform.position;
        dballnet = hballnet.magnitude;
        directionballnet = hballnet / dballnet;
        
        if (dballnet < dnet) {
            if (dnet > 150)
            {
                //Vector3 position = ball.transform.position + ball.GetComponent<Rigidbody>().velocity;
                getPosition(ball.transform.position);
            }
            else {
                if (isAGoalPosition())
                {
                    //JUMP TO GOAL
                    //Vector3 position = ball.transform.position + ball.GetComponent<Rigidbody>().velocity;
                    getPosition(ball.transform.position);
                }
                else {
                    //getPosition(findGoalPosition());
                    getPosition(ball.transform.position);
                }
            }
        }
        else {
            getPosition(homenetposition);
        }


        if (acceleration >= 0.0f)
            thrust = acceleration * forwardAcceleration;
        else if (acceleration < 0.0f)
            thrust = acceleration * reverseAcceleration;

        
        turnValue = 0.0f;
        if (Mathf.Abs(turnAxis) > deadZone)
        {
                turnValue = turnAxis;
        }

        Debug.DrawRay(transform.position, transform.forward * 5.0f, Color.blue);
        Debug.DrawRay(transform.position, directarget * 5.0f, Color.red);
        Debug.DrawRay(ball.transform.position, directionballnet * 10.0f, Color.green);
     
	}

    public bool isAGoalPosition() {
        if (transform.forward == directionball && directionball == directionballnet)
            goalPosition = true;
        else goalPosition = false; ;
        return goalPosition;
    }

    public Vector3 findGoalPosition() {
        Vector3 position = ball.transform.position + ball.GetComponent<Rigidbody>().velocity;
        return position;
    }

    public void getPosition(Vector3 position) { 

        htarget = position - transform.position;
        dtarget = htarget.magnitude;
        directarget = htarget / dtarget;
        directarget.y = 0.0f;

        float angle = Mathf.DeltaAngle(Mathf.Atan2(transform.forward.z, transform.forward.x) * Mathf.Rad2Deg,
                                Mathf.Atan2(directarget.z, directarget.x) * Mathf.Rad2Deg);

       // if (ball.transform.position.y < 5.0f)
        //{
            acceleration = 1.0f;
        //}

        if (angle > 0.0f)
        {
            turnAxis = -2.0f;
            if (angle > 45)
                turnAxis = -2.0f;
        }
        else if (angle < 0.0f)
        {
            turnAxis = 2.0f;
            if (angle < -45)
                turnAxis = 2.0f; 
        }
        else turnAxis = 0.0f;

        
    }

    public void Reset()
    {
        transform.position = originalP;
        transform.rotation = originalR;
        GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);
        GetComponent<Rigidbody>().angularVelocity = new Vector3(0, 0, 0);

    }
}
