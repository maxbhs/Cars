using UnityEngine;
using System.Collections;

public class Jump : MonoBehaviour {

    Rigidbody body;
	public float jumpMag = 3000.0f;
    public float rotationMag = 30.0f;
    public int airJumps = 1;
    public float impulseFlip = 25f;
    static public bool fliping = false;
    public int delay = 15;
    bool grounded;

    int contadortemps = 0;
    Vector3 direction;
    float dir;
    

	// Use this for initialization
	void Start () {
        body = GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void FixedUpdate() {

        grounded = CarController.grounded;

        //JUMP AND DOUBLE JUMP 

        if (!fliping)
        {
            if (Input.GetMouseButtonDown(1) && airJumps > 0)
            {
              
                    body.AddForceAtPosition(jumpMag * transform.up, transform.position);
                    airJumps -= 1;
            }

            if (grounded)
            {
                airJumps = 1;

            }

            


            //FREESTYLA
            if (grounded && Input.GetKey(KeyCode.Space)){
                if (Input.GetAxis("Vertical") > 0)
                {
                    dir = Input.GetAxis("Horizontal");
                    fliping = true;
                }
            }
            else if (!grounded && Input.GetKey(KeyCode.Space))
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
                if (dir == 0)
                {
                    if (contadortemps < 11) body.AddForce(2500f * 2f * Vector3.up);
                    if (contadortemps < 34) body.AddForceAtPosition(impulseFlip * impulseFlip * 2.0f * direction, transform.position);
                    if (contadortemps < 34) body.AddTorque(rotationMag * 10 * transform.right);
                }
                else
                {
                    if (contadortemps < 11) body.AddForce(3500f * 2f * Vector3.up);
                    if (contadortemps < 34) body.AddForceAtPosition(impulseFlip * impulseFlip * 2.0f * direction, transform.position);
                    if (contadortemps < 34) body.AddTorque(rotationMag * 10 * dir * transform.up);
                    if (contadortemps < 34) body.AddTorque(rotationMag * 10 * transform.right);
                }
            }
            contadortemps += 1;
            if (contadortemps > 34)
            {
                fliping = false;
                contadortemps = 0;
            }
        }
	}
}
