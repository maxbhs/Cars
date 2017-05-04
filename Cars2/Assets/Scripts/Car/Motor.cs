using UnityEngine;
using System.Collections;

public class Motor : MonoBehaviour {

    public float impulseMag = 20.0f;
    public float rotationMag = 3.0f;
    public float boostMag = 1.0f;
    private float speed;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        // Impulse

        speed = Vector3.Dot(GetComponent<Rigidbody>().velocity, transform.forward);
        Debug.LogWarning(speed);

        if (!Hover.onAir)
            GetComponent<Rigidbody>().AddForceAtPosition(impulseMag * Input.GetAxis("Vertical") * transform.forward,
                                                        transform.position - 0.7f * transform.up);

        // Rotation
        if (!Hover.onAir)
            GetComponent<Rigidbody>().AddTorque(rotationMag * boostMag * Input.GetAxis("Horizontal") * transform.up);


        // Traction
        GetComponent<Rigidbody>().AddForce(-0.1f * Vector3.Dot(GetComponent<Rigidbody>().velocity, transform.right) * transform.right);

        if (Input.GetMouseButton(0))
        {
            boostMag = 2.0f;
            GetComponent<Rigidbody>().AddForceAtPosition(boostMag * impulseMag * transform.forward,
                                                       transform.position - 0.6f * transform.up);
        }
        else boostMag = 1.0f;
	
	}
}
