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

    public ArrayList arrayList = new ArrayList();

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
        homenetposition = homenet.transform.position + new Vector3(0.0f, 25.6f, 0.0f);
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
        
        if (dballnet+10 < dnet) {
            if (dnet > 150)
            {
                getBall();
            }
            else {
                if (goalPosition(transform.position))
                {
                    getBall();
                }
                else {
                    getPosition(findGoalPosition());
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
     
	}

    public bool goalPosition(Vector3 position) {
       
        

        return false;
    }

    public Vector3 findGoalPosition() {
        return ball.transform.position;
    }

    public void getPosition(Vector3 position) { 
    
    }

    public void getBall() {

        Vector3 position = ball.transform.position + ball.GetComponent<Rigidbody>().velocity;

        htarget = ball.transform.position - transform.position;
        dtarget = htarget.magnitude;
        directarget = htarget / dtarget;
        directarget.y = 0.0f;

        Debug.DrawRay(transform.position, transform.forward * 5.0f, Color.blue);
        Debug.DrawRay(transform.position, directarget * 5.0f, Color.red);

        float angle = Mathf.DeltaAngle(Mathf.Atan2(transform.forward.z, transform.forward.x) * Mathf.Rad2Deg,
                                Mathf.Atan2(directarget.z, directarget.x) * Mathf.Rad2Deg);

        if (ball.transform.position.y < 5.0f)
        {
            acceleration = 1.0f;
        }

        if (angle > 0.0f) turnAxis = -1.0f;
        else if (angle < 0.0f) turnAxis = 1.0f;
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
