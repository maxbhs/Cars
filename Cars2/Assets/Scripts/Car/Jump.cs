using UnityEngine;
using System.Collections;

public class Jump : MonoBehaviour {

    Rigidbody body;
	public float jumpMag = 3000.0f;
    public float rotationMag = 30.0f;
    public int airJumps = 2;
    public float impulseFlip = 25f;
    static public bool fliping = false;
    int delay;
    int delaydoublejump;
    bool firstclick;
    bool haveToJump;
    bool grounded;

    int contadortemps = 0;
    Vector3 direction;
    

	// Use this for initialization
	void Start () {
        body = GetComponent<Rigidbody>();
        delay = 0;
        delaydoublejump = 0;
        firstclick = true;
	}
	
	// Update is called once per frame
	void FixedUpdate() {

        grounded = CarController.grounded;

        //JUMP AND DOUBLE JUMP 

        if (!fliping)
        {
            if (Input.GetMouseButtonDown(1) && airJumps > 0)
            {
                if (firstclick)
                {
                    delaydoublejump = 15;
                    delay = 20;
                    firstclick  = false;
                    haveToJump = true;
                }
                else if (!firstclick && delaydoublejump == 0 && airJumps == 1)
                {
                    body.AddForceAtPosition(jumpMag * transform.up, transform.position);
                    airJumps -= 1;
                }
                else if (!firstclick && delaydoublejump > 0)
                {
                    haveToJump = false;
                    delay = 20;
                    if (Input.GetAxis("Vertical") > 0)
                        fliping = true; //AUTOFLIP
                }
            }

            if (grounded && delay == 0)
            {
                firstclick = true;
                airJumps = 2;

            }

            if (delaydoublejump > 0) delaydoublejump -= 1;
            else if (delaydoublejump == 0 && haveToJump){
                body.AddForceAtPosition(jumpMag * transform.up, transform.position);
                haveToJump = false;
                airJumps -= 1;
            }

            //FREESTYLA
            if (!grounded && Input.GetKey(KeyCode.Space))
            {
                //MANUAL FLIPS
                if (Input.GetAxis("Horizontal") != 0)
                {
                    if (Input.GetKey(KeyCode.LeftShift))
                    {
                        body.AddTorque(rotationMag * Input.GetAxis("Horizontal") * -transform.forward);
                    }
                    else
                    {
                        body.AddTorque(rotationMag * Input.GetAxis("Horizontal") * transform.up);
                    }
                }
                if (Input.GetAxis("Vertical") != 0)
                {
                    body.AddTorque(rotationMag * Input.GetAxis("Vertical") * transform.right);
                }
            }
        }
        else { //AUTOFLIP
            if (contadortemps == 0)
            {
                direction = transform.forward;
            }
            else
            {
                if (contadortemps < 11)body.AddForce(2500f * 2f * Vector3.up);
                if (contadortemps < 34) body.AddForceAtPosition(impulseFlip * impulseFlip * 2.0f * direction, transform.position);
                else body.AddForceAtPosition(impulseFlip * impulseFlip * direction, transform.position);
                if (contadortemps < 34) body.AddTorque(rotationMag * 10 * transform.right);
            }
            contadortemps += 1;
            if (grounded && delay == 0)
            {
                fliping = false;
                contadortemps = 0;
            }
        }
        if (delay > 0) delay -= 1;
	}
}
