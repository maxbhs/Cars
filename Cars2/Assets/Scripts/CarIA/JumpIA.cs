using UnityEngine;
using System.Collections;

public class JumpIA : MonoBehaviour {


    public float rotationMag = 30.0f;
    public float impulseFlip = 25f;

    Vector3 direction;

    private bool goalPosition;
    private bool grounded;
    private float dir;
    private float contadortemps;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void FixedUpdate () {

        goalPosition = CarControllerIA.goalPosition;
        grounded = CarPhysicsIA.grounded;
        
        dir = Random.Range(-1, 1);

        if (goalPosition){
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
        }

        contadortemps += 1;
        if (contadortemps > 34 && grounded)
        {
            contadortemps = 0;
        }

        Debug.Log(goalPosition);
	}
}
