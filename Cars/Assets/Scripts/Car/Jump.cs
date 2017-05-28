using UnityEngine;
using System.Collections;

public class Jump : MonoBehaviour {

    
	public float jumpMag = 3000.0f;
    public float rotationMag = 30.0f;
    public int airJumps = 2;
    public float impulseFlip = 25f;
    static public bool fliping = false;
    public int delay = 15;
    bool grounded;

    int contadortemps = 0;
    Vector3 direction;
    float dir;
    

	// Use this for initialization
	void Start () {
        
	}
	
	// Update is called once per frame
	void FixedUpdate() {

        grounded = CarPhysics.grounded;

        //JUMP DOUBLE JUMP AND AUTOFLIP 

        if (!fliping)
        {
            //FREESTYLA
            if (grounded && Input.GetKey(KeyCode.Space) && Input.GetMouseButton(1) && Input.GetAxis("Vertical") > 0)
            {
                dir = Input.GetAxis("Horizontal");
                fliping = true;
            }
            else if (Input.GetKey(KeyCode.Space) && airJumps > 0)
            {

                GetComponent<Rigidbody>().AddForceAtPosition(jumpMag * transform.up, transform.position);
                airJumps -= 1;
            }

            if (grounded)
            {
                airJumps = 2;

            }
            else if (!grounded && Input.GetMouseButton(1))
            {
                //MANUAL FLIPS
                if (Input.GetAxis("Horizontal") != 0)
                {
                    if (Input.GetKey(KeyCode.LeftShift))
                    {
                        GetComponent<Rigidbody>().AddTorque(rotationMag * Input.GetAxis("Horizontal") * -transform.forward);
                    }
                    else
                    {
                        GetComponent<Rigidbody>().AddTorque(rotationMag * Input.GetAxis("Horizontal") * transform.up);
                    }
                }
                if (Input.GetAxis("Vertical") != 0)
                {
                    GetComponent<Rigidbody>().AddTorque(rotationMag * Input.GetAxis("Vertical") * transform.right);
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
                    if (contadortemps < 11) GetComponent<Rigidbody>().AddForce(2500f * 2f * Vector3.up);
                    if (contadortemps < 34) GetComponent<Rigidbody>().AddForceAtPosition(impulseFlip * impulseFlip * 2.0f * direction, transform.position);
                    if (contadortemps < 34) GetComponent<Rigidbody>().AddTorque(rotationMag * 10 * transform.right);
                }
                else
                {
                    if (contadortemps < 11) GetComponent<Rigidbody>().AddForce(3500f * 2f * Vector3.up);
                    if (contadortemps < 34) GetComponent<Rigidbody>().AddForceAtPosition(impulseFlip * impulseFlip * 2.0f * direction, transform.position);
                    if (contadortemps < 34) GetComponent<Rigidbody>().AddTorque(rotationMag * 10 * dir * transform.up);
                    if (contadortemps < 34) GetComponent<Rigidbody>().AddTorque(rotationMag * 10 * transform.right);
                }
            }
            contadortemps += 1;
            if (contadortemps > 34 && grounded)
            {
                fliping = false;
                contadortemps = 0;
            }
        }
	}
}
