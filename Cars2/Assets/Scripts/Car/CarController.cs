using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CarController : MonoBehaviour {

    public Text turboText;

    float deadZone = 0.0f;
    bool fliping;
    static public float thrust = 0.0f;
    static public float turnValue = 0.0f;
    float forwardAcceleration;
    float reverseAcceleration;
    float boostFactor = 1.0f;
    float boostImpulse = 2000f;
    static public int turbo; //va de 0 a 100
    bool restarTurbo;

    Vector3 originalP;
    Quaternion originalR;

    // Use this for initialization
    void Start () {
        turbo = 40;
        SetTurboText();
        restarTurbo = true;
        forwardAcceleration = CarPhysics.forwardAcceleration;
        reverseAcceleration = CarPhysics.reverseAcceleration;

        originalP = transform.position;
        originalR = transform.rotation;
    }
	
	// Update is called once per frame
	void FixedUpdate () {
        fliping = Jump.fliping;
        // Main Thrust
        thrust = 0.0f;
        if (!fliping)
        {
            float acceleration = Input.GetAxis("Vertical");
            if (acceleration > deadZone)
                thrust = acceleration * forwardAcceleration;
            else if (acceleration < -deadZone)
                thrust = acceleration * reverseAcceleration;

            // Turning
            turnValue = 0.0f;
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
                GetComponent<Rigidbody>().AddForceAtPosition(boostFactor * boostImpulse * transform.forward,
                                                           transform.position - 0.6f * transform.up);

                if (restarTurbo) //si usamos este booleano, el turbo dura el doble
                {
                    restarTurbo = false;
                    turbo = turbo - 1;
                    SetTurboText();
                }
                else restarTurbo = true;

            }
            else boostFactor = 1.0f;
        }
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
