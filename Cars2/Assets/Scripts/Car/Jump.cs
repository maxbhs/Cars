using UnityEngine;
using System.Collections;

public class Jump : MonoBehaviour {
	
	public float jumpMag = 400.0f;
    public float rotationMag = 30.0f;
    public int njumps = 1;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void FixedUpdate() {


        //JUMP AND DOUBLE JUMP
        if (Input.GetMouseButtonDown(1) && njumps > 0)
        {
            gameObject.GetComponent<Rigidbody>().AddForceAtPosition(jumpMag * Vector3.up, transform.position);
            --njumps;
            
        }
        //FREESTYLA
        if (Hover.onAir && Input.GetKey(KeyCode.Space)) {
            if (Input.GetAxis("Horizontal") != 0) {
                if (Input.GetKey(KeyCode.LeftShift))
                {
                    gameObject.GetComponent<Rigidbody>().AddTorque(rotationMag * Input.GetAxis("Horizontal") * - transform.forward);
                }
                else
                {
                    gameObject.GetComponent<Rigidbody>().AddTorque(rotationMag * Input.GetAxis("Horizontal") * transform.up);
                }
            }
                //FLIP
            if (Input.GetAxis("Vertical") != 0) {
                gameObject.GetComponent<Rigidbody>().AddTorque(rotationMag * Input.GetAxis("Vertical") * transform.right);
            }
        }
        //LAND
        else if (!Hover.onAir)
        {
            njumps = 1;
        }
	}
}
