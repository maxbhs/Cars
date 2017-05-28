using UnityEngine;
using System.Collections;

public class CarControllerIA : MonoBehaviour {


    public float thrust = 0.0f;
    public float turnValue = 0.0f;
    public float boostFactor = 0.0f;
    public bool JumpToGoal = false;
    bool grounded;
    bool fliping;

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

        forwardAcceleration = GetComponent<CarPhysicsIA>().forwardAcceleration;
        reverseAcceleration = GetComponent<CarPhysicsIA>().reverseAcceleration;
        grounded = GetComponent<CarPhysicsIA>().grounded;
        fliping = GetComponent<JumpIA>().fliping;

        originalP = transform.position;
        originalR = transform.rotation;

        delayencallat = 0;
	}
	
	// Update is called once per frame
	void FixedUpdate () {

        acceleration = 0.0f;
        turnAxis = 0.0f;


        //Direccio coche centre porteria casa
       /* homenetposition = homenet.transform.position + new Vector3(7.0f, 30.0f, 20.0f);
        hhomenet = homenetposition - transform.position;
        dhomenet = hhomenet.magnitude;
        directionhomenet = hhomenet / dhomenet;*/

        float dir = Random.Range(0, 20);

        homenetposition = homenet.transform.position;
        if (homenetposition.z < 45) homenetposition += new Vector3(dir, 30.0f, 30.0f);
        else homenetposition += new Vector3(dir, 30.0f, -30.0f);


        //Direccio bola, distancia bola
        hball = ball.transform.position - transform.position;
        dball = hball.magnitude;
        directionball = hball / dball;

        
        /*netposition = net.transform.position + new Vector3(7.0f, 30.0f, -20.0f);
        netpositionleft = net.transform.position + new Vector3(-8.0f, 30.0f, -20.0f);
        netpositionright = net.transform.position + new Vector3(20.0f, 30.0f, -20.0f);*/

        netposition = net.transform.position;
        if (netposition.z < 100) netposition += new Vector3(7.0f, 30.0f, 20.0f);
        else netposition += new Vector3(7.0f, 30.0f, -30.0f);

        netpositionleft = net.transform.position;
        if (netpositionleft.z < 100) netpositionleft += new Vector3(-8.0f, 30.0f, 20.0f);
        else netpositionleft += new Vector3(-8.0f, 30.0f, -30.0f);

        netpositionright = net.transform.position;
        if (netpositionright.z < 100) netpositionright += new Vector3(20.0f, 30.0f,20.0f);
        else netpositionright += new Vector3(20.0f, 30.0f, -20.0f);

        //Direccio coche centre porteria contraria
        hnet = netposition - transform.position;
        dnet = hnet.magnitude;
        directionnet = hnet / dnet;

        //Distancia bola i centre, esquerra, dreta portaria contraria
        hballnet = netposition - ball.transform.position;
        hballnet.y = 0.0f;
        dballnet = hballnet.magnitude;
        directionballnet = hballnet / dballnet;


        hballnetleft = netpositionleft - ball.transform.position;
        hballnetleft.y = 0.0f;
        dballnetleft = hballnetleft.magnitude;
        directionballnetleft = hballnetleft / dballnetleft;

        
        hballnetright = netpositionright - ball.transform.position;
        hballnetright.y = 0.0f;
        dballnetright = hballnetright.magnitude;
        directionballnetright = hballnetright / dballnetright;

        if (!fliping)
        {
            if (dballnet < dnet)
            {
                if (dnet > 150)
                {
                    //AL MEU CAMP
                    Vector3 position = ball.transform.position;
                    if (ball.transform.position.y < 5.0f)
                    {
                        getPosition(position);
                    }
                    else acceleration = 0.0f;
  
                }
                else
                {
                    //AL CAMP CONTRARI
                    if (ball.transform.position.y < 5.0f)
                    {
                        if (isAGoalPosition())
                        {
                            //if (dball > 30)
                                //JumpToGoal = true;

                            Vector3 position = ball.transform.position;
                            getPosition(position);
                        }
                        else
                        {
                            getPosition(findGoalPosition());
                        }
                    }
                    else
                    {
                        getPosition(findGoalPosition());
                    }
                }
            }
            else
            {
                //BOLA DARRERA!!! cap a casa
                if (dnet > 20)
                {
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

            bool b = isAGoalPosition();

            Debug.DrawRay(ball.transform.position, hballnetleft, b ? Color.green : Color.red);
            Debug.DrawRay(ball.transform.position, hballnetright, b ? Color.green : Color.red);
            Debug.DrawRay(ball.transform.position, -hballnetleft, b ? Color.green : Color.red);
            Debug.DrawRay(ball.transform.position, -hballnetright, b ? Color.green : Color.red);
        }
        else JumpToGoal = false;
        int cont = 0;
        //Debug.Log(ball.transform.position+" "+BallLanding(cont)+" "+cont);

	}

    public bool isAGoalPosition() {

        Vector3 pLeft = ball.transform.position - hballnetleft * dball/hballnetleft.z;
        Vector3 pRight = ball.transform.position - hballnetright * dball/ hballnetright.z;

        if (transform.position.x < pLeft.x && transform.position.x > pRight.x && transform.position.z < ball.transform.position.z)
        {
            return true;
        }
        else if (transform.position.x > pLeft.x)
        {
            return false;
        }
        else if (transform.position.x < pRight.x)
        {
            return false;
        }
        else return false;
    }

    public Vector3 findGoalPosition() {
        Vector3 pos = ball.transform.position - hballnet;
        pos.y = 0.0f;
        if (pos.x > 465 || pos.x < 279 || pos.z > 335 || pos.z < 45)
            pos = ball.transform.position;
        return pos;
    }

    public Vector3 BallLanding(int cont) {
        Vector3 posball = ball.transform.position;
        while (posball.y> 0.0) {
            posball += ball.GetComponent<Rigidbody>().velocity;
            ++cont;     
        }
        return posball;
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
                delayencallat = 50;
            }
            else
            {
                acceleration = 1.0f;
            }
        }
        else
        {
            if (grounded)
                acceleration = -1.0f;
            else
            {
                GetComponent<Rigidbody>().AddTorque(500000 * -transform.forward);
            }
        }

        if (transform.position == position) acceleration = 0.0f;

        if (angle > 0.0f)
        {
            if (angle > 100)
                acceleration = 0.0f;
            turnAxis = -3.0f;
        }
        else if (angle < 0.0f)
        {
            if (angle > 100)
                acceleration = 0.0f;
            turnAxis = 3.0f;
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
