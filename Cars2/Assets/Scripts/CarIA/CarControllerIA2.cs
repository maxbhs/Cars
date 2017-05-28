using UnityEngine;
using System.Collections;

public class CarControllerIA2 : MonoBehaviour
{


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
    void Start()
    {

        forwardAcceleration = GetComponent<CarPhysicsIA2>().forwardAcceleration;
        reverseAcceleration = GetComponent<CarPhysicsIA2>().reverseAcceleration;
        grounded = GetComponent<CarPhysicsIA2>().grounded;
        fliping = GetComponent<JumpIA>().fliping;

        originalP = transform.position;
        originalR = transform.rotation;

        delayencallat = 0;
    }

    // Update is called once per frame
    void FixedUpdate()
    {

        acceleration = 0.0f;
        turnAxis = 0.0f;


        //Direccio coche centre porteria casa

        homenetposition = homenet.transform.position;

        float dir = Random.Range(0, 20);

        if (homenetposition.z < 45) homenetposition += new Vector3(dir, 30.0f, 30.0f);
        else homenetposition += new Vector3(dir, 30.0f, -30.0f);


        hhomenet = homenetposition - transform.position;
        dhomenet = hhomenet.magnitude;
        directionhomenet = hhomenet / dhomenet;


        //Direccio bola, distancia bola
        hball = ball.transform.position - transform.position;
        dball = hball.magnitude;
        directionball = hball / dball;


        netposition = net.transform.position;
        if (netposition.z < 100)  netposition += new Vector3(7.0f, 30.0f, 20.0f);
        else netposition += new Vector3(7.0f, 30.0f, -20.0f);

        netpositionleft = net.transform.position;
        if (netpositionleft.z < 100) netpositionleft += new Vector3(-8.0f, 30.0f, 20.0f);
        else netpositionleft += new Vector3(-8.0f, 30.0f, -20.0f);

        netpositionright = net.transform.position;
        if (netpositionright.z < 100) netpositionright += new Vector3(20.0f, 30.0f, 20.0f);
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


        if (dballnet > 150  )
        {
                getPosition(homenetposition);
        }
        else {
            if (isAGoalPosition())
            {
                getPosition(ball.transform.position);
                acceleration *= 2;
            }
            else {
                Vector3 pos = netposition;
                if (netposition.z < 100)
                    pos.z += 100;
                else pos.z -= 100;
                getPosition(pos);
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

    public bool isAGoalPosition()
    {

        Vector3 pLeft = ball.transform.position - hballnetleft * dball / hballnetleft.z;
        Vector3 pRight = ball.transform.position - hballnetright * dball / hballnetright.z;

            if (transform.position.x > pLeft.x && transform.position.x < pRight.x && transform.position.z < ball.transform.position.z)
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

    public Vector3 findGoalPosition()
    {
        Vector3 pos = ball.transform.position - hballnet;
        pos.y = 0.0f;
        if (pos.x > 465 || pos.x < -279 || pos.z > 335 || pos.z < 45)
            pos = ball.transform.position;
        return pos;
    }

    public void getPosition(Vector3 position)
    {

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

        if (transform.position == position)
        {
            acceleration = 0.0f;
            transform.forward = directionnet;
        }

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
