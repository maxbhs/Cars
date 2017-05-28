using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CarController : MonoBehaviour {

    public Text speedText;
    public Text turboText;
    public ParticleEmitter flame1;
    public ParticleEmitter flame2;
    public GameObject audioObj;

    static public float thrust = 0.0f;
    static public float turnValue = 0.0f;
    static public float boostFactor = 0.0f;
    

    float forwardAcceleration;
    float reverseAcceleration;
    
    static public int turbo; //va de 0 a 100
    bool restartTurbo;

    private float deadZone = 0.0f;
    private bool fliping;
    private float speed;
    
    Vector3 originalP;
    Quaternion originalR;

    // Use this for initialization
    void Start () {
        turbo = 40;
        SetTurboText();
        restartTurbo = true;
        forwardAcceleration = CarPhysics.forwardAcceleration;
        reverseAcceleration = CarPhysics.reverseAcceleration;
        
        originalP = transform.position;
        originalR = transform.rotation;

        audioObj.SetActive(false);
        flame1.emit = false;
        flame2.emit = false;
    }
	
	// Update is called once per frame
	void FixedUpdate () {
        
        

        fliping = Jump.fliping;
        thrust = 0.0f;
        turnValue = 0.0f;
        if (!fliping)
        {
            float acceleration = Input.GetAxis("Vertical");
            if (acceleration > deadZone)
                thrust = acceleration * forwardAcceleration;
            else if (acceleration < -deadZone)
                thrust = acceleration * reverseAcceleration;

            // Turning
            float turnAxis = Input.GetAxis("Horizontal");
            if (Mathf.Abs(turnAxis) > deadZone)
            {
                if (Input.GetKey(KeyCode.LeftShift))
                {
                    turnValue = turnAxis * 2;
                }
                else
                {
                    turnValue = turnAxis;
                }
            }

            if (Input.GetMouseButton(0) && turbo > 0)
            {
                boostFactor = 2.0f;
                flame1.emit = true;
                flame2.emit = true;
                audioObj.SetActive(true);
                if (restartTurbo) //si usamos este booleano, el turbo dura el doble
                {
                    restartTurbo = false;
                    turbo = turbo - 1;
                    SetTurboText();
                }
                else restartTurbo = true;

            }
            else
            {
                boostFactor = 0.0f;
                audioObj.SetActive(false);
                flame1.emit = false;
                flame2.emit = false;
            }
        }
        else {
            boostFactor = 0.0f;
        }

        speed = GetComponent<Rigidbody>().velocity.sqrMagnitude;
        SetSpeedText();
	}

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Turbo"))
        {

            if (turbo < 100)
            {
                other.gameObject.SetActive(false);
                if (turbo <= 80)
                {
                    turbo = turbo + 20;
                }
                else
                {
                    turbo = turbo + (100 - turbo);
                }
                SetTurboText();
            }
        }
    }

    void SetSpeedText()
    {
        speedText.text = "Speed: " + (GetComponent<Rigidbody>().velocity.magnitude).ToString("#.##");
    }

    void SetTurboText()
    {
        turboText.text = "Turbo: " + turbo.ToString();
    }

    public void Reset()
    {
        turbo = 40;
        SetTurboText();
        transform.position = originalP;
        transform.rotation = originalR;
        GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);
        GetComponent<Rigidbody>().angularVelocity = new Vector3(0, 0, 0);

    }
}
