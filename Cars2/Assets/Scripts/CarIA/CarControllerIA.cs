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
    Vector3 netpositionleft;
    Vector3 netpositionright;
    Vector3 homenetposition;

    Vector3 hball;
    Vector3 hnet;
    Vector3 hhomenet;
    Vector3 hballnet;
    Vector3 hballnetleft;
    Vector3 hballnetright;
    Vector3 htarget;

    int delayencallat;
    Vector3 prevpos;

    float dball, dnet, dhomenet, dballnet, dballnetleft, dballnetright, dtarget;

    Vector3 directionball;
    Vector3 directionnet;
    Vector3 directionhomenet;
    Vector3 directarget;
    Vector3 directionballnet;
    Vector3 directionballnetleft;
    Vector3 directionballnetright;

    private float acceleration;
    private float turnAxis;

	// Use this for initialization
	void Start () {
        forwardAcceleration = CarPhysicsIA.forwardAcceleration;
        reverseAcceleration = CarPhysicsIA.reverseAcceleration;

        originalP = transform.position;
        originalR = transform.rotation;

        delayencallat = 0;
	}
	
	// Update is called once per frame
	void FixedUpdate () {

        acceleration = 0.0f;
        turnAxis = 0.0f;


        //Direccio coche centre porteria casa
        homenetposition = homenet.transform.position + new Vector3(7.0f, 30.0f, 20.0f);
        hhomenet = homenetposition - transform.position;
        dhomenet = hhomenet.magnitude;
        directionhomenet = hhomenet / dhomenet;


        //Direccio bola, distancia bola
        hball = ball.transform.position - transform.position;
        dball = hball.magnitude;
        directionball = hball / dball;

        
        netposition = net.transform.position + new Vector3(7.0f, 30.0f, -20.0f);
        netpositionleft = net.transform.position + new Vector3(-8.0f, 30.0f, -20.0f);
        netpositionright = net.transform.position + new Vector3(20.0f, 30.0f, -20.0f);

        //Direccio coche centre porteria contraria
        hnet = netposition - transform.position;
        dnet = hnet.magnitude;
        directionnet = hnet / dnet;

        //Distancia bola i centre, esquerra, dreta portaria contraria
        hballnet = netposition - ball.transform.position;
        dballnet = hballnet.magnitude;
        directionballnet = hballnet / dballnet;

        hballnetleft = netpositionleft - ball.transform.position;
        dballnetleft = hballnetleft.magnitude;
        directionballnetleft = hballnetleft / dballnetleft;

        
        hballnetright = netpositionright - ball.transform.position;
        dballnetright = hballnetright.magnitude;
        directionballnetright = hballnetright / dballnetright;
        
        if (dballnet < dnet) {
            if (dnet > 150)
            {
                Vector3 position = ball.transform.position;
                if (ball.transform.position.y < 5.0f)
                    getPosition(position);
                else
                    getPosition(findGoalPosition());
            }
            else {
                if (isAGoalPosition())
                {
                    //JUMP TO GOAL
                    Vector3 position = ball.transform.position;
                    getPosition(position);
                }
                else {
                    Vector3 position = ball.transform.position;
                    if (ball.transform.position.y < 5.0)
                        getPosition(position);
                    else
                        getPosition(findGoalPosition());
                }
            }
        }
        else {
            if (dnet > 20){
                getPosition(homenetposition);
            }
            else
            {
                Vector3 position = ball.transform.position;
                getPosition(position);
            }
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

        if (delayencallat > 0) delayencallat -= 1;
        prevpos = transform.position;

        Debug.DrawRay(ball.transform.position, hballnet, Color.green);
        Debug.DrawRay(ball.transform.position, hballnetleft, Color.green);
        Debug.DrawRay(ball.transform.position, hballnetright, Color.green);
        Debug.DrawRay(ball.transform.position, -hballnet, Color.green);
        Debug.DrawRay(ball.transform.position, -hballnetleft, Color.green);
        Debug.DrawRay(ball.transform.position, -hballnetright, Color.green);
        Debug.Log(-hballnetleft + " " + -hballnetright+" "+ transform.position+" "+ball.transform.position);
	}

    public bool isAGoalPosition() {

        return false;
    }

    public Vector3 findGoalPosition() {

        return transform.position;
    }

    public void getPosition(Vector3 position) { 

        htarget = position - transform.position;
        dtarget = htarget.magnitude;
        directarget = htarget / dtarget;
        directarget.y = 0.0f;

        float angle = Mathf.DeltaAngle(Mathf.Atan2(transform.forward.z, transform.forward.x) * Mathf.Rad2Deg,
                                Mathf.Atan2(directarget.z, directarget.x) * Mathf.Rad2Deg);


        if (delayencallat == 0)
        {
            if (prevpos == transform.position)
            {
                acceleration = -1.0f;
                delayencallat = 50;
            }
            else
            {
                acceleration = 1.0f;
            }
         }
        else acceleration = -1.0f;

        if (transform.position == position) acceleration = 0.0f;

        if (angle > 0.0f)
        {
            turnAxis = -3.0f;
        }
        else if (angle < 0.0f)
        {
            turnAxis = 3.0f;
        }
        else turnAxis = 0.0f;

        Debug.Log(acceleration+" "+delayencallat);
    }

    public void Reset()
    {
        transform.position = originalP;
        transform.rotation = originalR;
        GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);
        GetComponent<Rigidbody>().angularVelocity = new Vector3(0, 0, 0);

    }
}
