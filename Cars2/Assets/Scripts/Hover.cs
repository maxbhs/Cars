using UnityEngine;
using System.Collections;

public class Hover : MonoBehaviour {
	
	public float fMag = 10.0f;
	public float impulseMag = 1.0f;
	public float rotationMag = 3.0f;

	// Use this for initialization
	void Start () {
		GetComponent<Rigidbody>().centerOfMass = new Vector3(0.0f, -0.3f, 0.0f);
	}
	
	// Update is called once per frame
	void Update () {
		
		Vector3 leftRear = transform.TransformPoint(new Vector3(-0.5f, -0.5f, -0.5f));
		Vector3 rightRear = transform.TransformPoint(new Vector3(0.5f, -0.5f, -0.5f));
		Vector3 leftFront = transform.TransformPoint(new Vector3(-0.5f, -0.5f, 0.5f));
		Vector3 rightFront = transform.TransformPoint(new Vector3(0.5f, -0.5f, 0.5f));
		
		RaycastHit hLeftRear, hRightRear, hLeftFront, hRightFront;
		
		Physics.Raycast(leftRear + 0.1f * transform.up, -transform.up, out hLeftRear);
		Physics.Raycast(rightRear + 0.1f * transform.up, -transform.up, out hRightRear);
		Physics.Raycast(leftFront + 0.1f * transform.up, -transform.up, out hLeftFront);
		Physics.Raycast(rightFront + 0.1f * transform.up, -transform.up, out hRightFront);
		
		Debug.DrawRay(leftRear, -transform.up, (hLeftRear.distance < 1.0f)?Color.red:Color.black);
		Debug.DrawRay(rightRear, -transform.up, (hRightRear.distance < 1.0f)?Color.red:Color.black);
		Debug.DrawRay(leftFront, -transform.up, (hLeftFront.distance < 1.0f)?Color.red:Color.black);
		Debug.DrawRay(rightFront, -transform.up, (hRightFront.distance < 1.0f)?Color.red:Color.black);
		
		// Suspension
		if(hLeftRear.distance < 1.0f)
			GetComponent<Rigidbody>().AddForceAtPosition((1.0f - hLeftRear.distance) * fMag * hLeftRear.normal, leftRear);
		if(hRightRear.distance < 1.0f)
			GetComponent<Rigidbody>().AddForceAtPosition((1.0f - hRightRear.distance) * fMag * hRightRear.normal, rightRear);
		if(hLeftFront.distance < 1.0f)
			GetComponent<Rigidbody>().AddForceAtPosition((1.0f - hLeftFront.distance) * fMag * hLeftFront.normal, leftFront);
		if(hRightFront.distance < 1.0f)
			GetComponent<Rigidbody>().AddForceAtPosition((1.0f - hRightFront.distance) * fMag * hRightFront.normal, rightFront);
		
		// Impulse
		if((hLeftFront.distance < 1.0f) && (hRightFront.distance < 1.0f))
			GetComponent<Rigidbody>().AddForceAtPosition(impulseMag * Input.GetAxis("Vertical") * transform.forward, 
														transform.position - 0.5f * transform.up);
													 
		// Rotation
		GetComponent<Rigidbody>().AddTorque(rotationMag * Input.GetAxis("Horizontal") * transform.up);
		
		// Traction
		GetComponent<Rigidbody>().AddForce(-0.1f * Vector3.Dot(GetComponent<Rigidbody>().velocity, transform.right) * transform.right);
	
	}
}
