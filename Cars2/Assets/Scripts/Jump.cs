using UnityEngine;
using System.Collections;

public class Jump : MonoBehaviour {
	
	public float jumpForce = 0.0f;
    public float boostForce = 0.0f;
    public float rotationForce = 0.0f;
    public int njumps = 1;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void FixedUpdate() {


        //JUMP AND DOUBLE JUMP
        if (Input.GetMouseButtonDown(1) && njumps > 0)
        {
            gameObject.GetComponent<Rigidbody>().AddForceAtPosition(jumpForce * Vector3.up, transform.position);
            --njumps;
            
        }
        //SPEED BOOST
        else if (Input.GetMouseButton(0))
        {
            gameObject.GetComponent<Rigidbody>().AddForceAtPosition(boostForce * transform.forward,
                                                        transform.position - 1.1f * transform.up);
        }
        //FREESTYLA
        if (Hover.onAir) {
            if (Input.GetAxis("Horizontal") != 0) {
                if (Input.GetKey(KeyCode.LeftShift))
                {
                    gameObject.GetComponent<Rigidbody>().AddTorque(rotationForce * Input.GetAxis("Horizontal") * - transform.forward);
                }
                else
                {
                    gameObject.GetComponent<Rigidbody>().AddTorque(rotationForce * Input.GetAxis("Horizontal") * transform.up);
                }
            }
                //FLIP
            if (Input.GetAxis("Vertical") != 0) {
                gameObject.GetComponent<Rigidbody>().AddTorque(rotationForce * Input.GetAxis("Vertical") * transform.right);
            }
        }
        //LAND
        else
        {
            njumps = 1;
        }
	}
}
