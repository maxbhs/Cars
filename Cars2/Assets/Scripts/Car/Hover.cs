using UnityEngine;
using System.Collections;

public class Hover : MonoBehaviour {

    public float SuspensionStiffness = 1.0f;
    static public bool onAir = false;

	// Use this for initialization
	void Start () {
		GetComponent<Rigidbody>().centerOfMass = new Vector3(0.0f, -0.7f, 0.0f);
	}
	
	// Update is called once per frame
    void FixedUpdate()
    {

        //Position
        Vector3 leftRear = transform.TransformPoint(new Vector3(-0.5f, -0.5f, -0.5f));
        Vector3 rightRear = transform.TransformPoint(new Vector3(0.5f, -0.5f, -0.5f));
        Vector3 leftFront = transform.TransformPoint(new Vector3(-0.5f, -0.5f, 0.5f));
        Vector3 rightFront = transform.TransformPoint(new Vector3(0.5f, -0.5f, 0.5f));

        //Velocity at Position
        Vector3 vLeftRear = GetComponent<Rigidbody>().GetPointVelocity(leftRear);
        Vector3 vRightRear = GetComponent<Rigidbody>().GetPointVelocity(rightRear);
        Vector3 vLeftFront = GetComponent<Rigidbody>().GetPointVelocity(leftFront);
        Vector3 vRightFront = GetComponent<Rigidbody>().GetPointVelocity(rightFront);

        //Suspension force at position
        Vector3 sLeftRear = Vector3.Project(vLeftRear, transform.up);
        Vector3 sRightRear = Vector3.Project(vRightRear, transform.up);
        Vector3 sLeftFront = Vector3.Project(vLeftFront, transform.up);
        Vector3 sRightFront = Vector3.Project(vRightFront, transform.up);

        //Ratcast to determine compress ratio
        RaycastHit hLeftRear, hRightRear, hLeftFront, hRightFront;

        Physics.Raycast(leftRear + 0.1f * transform.up, -transform.up, out hLeftRear);
        Physics.Raycast(rightRear + 0.1f * transform.up, -transform.up, out hRightRear);
        Physics.Raycast(leftFront + 0.1f * transform.up, -transform.up, out hLeftFront);
        Physics.Raycast(rightFront + 0.1f * transform.up, -transform.up, out hRightFront);

        Debug.DrawRay(leftRear, -transform.up, (hLeftRear.distance < 1.0f) ? Color.red : Color.black);
        Debug.DrawRay(rightRear, -transform.up, (hRightRear.distance < 1.0f) ? Color.red : Color.black);
        Debug.DrawRay(leftFront, -transform.up, (hLeftFront.distance < 1.0f) ? Color.red : Color.black);
        Debug.DrawRay(rightFront, -transform.up, (hRightFront.distance < 1.0f) ? Color.red : Color.black);

        if (((hLeftFront.distance < 1.0f) && (hLeftFront.distance > 0.0f)) || ((hRightFront.distance < 1.0f) && (hRightFront.distance > 0.0f)) || ((hLeftRear.distance < 1.0f) && (hLeftRear.distance > 0.0f)) || ((hRightRear.distance < 1.0f) && (hRightRear.distance > 0.0f)))
        {
            onAir = false;
            GetComponent<Rigidbody>().drag = 0.5f;
        }
        else
        {
            GetComponent<Rigidbody>().drag = 0.0f;
            onAir = true;
        }

        //Compression ratio
        float crLeftRear = (1.0f - hLeftRear.distance) / 1.0f;
        float crRightRear = (1.0f - hRightRear.distance) / 1.0f;
        float crLeftFront = (1.0f - hLeftFront.distance) / 1.0f;
        float crRightFront = (1.0f - hRightFront.distance) / 1.0f;

        //New suspension
        Vector3 nsLeftRear = transform.up * crLeftRear * SuspensionStiffness;
        Vector3 nsRightRear = transform.up * crRightRear * SuspensionStiffness;
        Vector3 nsLeftFront = transform.up * crLeftFront * SuspensionStiffness;
        Vector3 nsRightFront = transform.up * crRightFront * SuspensionStiffness;

        //Delta suspension
        Vector3 dLeftRear = nsLeftRear - sLeftRear;
        Vector3 dRightRear = nsRightRear - sRightRear;
        Vector3 dLeftFront = nsLeftFront - sLeftFront;
        Vector3 dRightFront = nsRightFront - sRightFront;

        if (hLeftRear.distance < 1.0f)
        {
            GetComponent<Rigidbody>().AddForceAtPosition((1.0f - hLeftRear.distance) * hLeftRear.normal, leftRear);
            GetComponent<Rigidbody>().AddForceAtPosition(dLeftRear, leftRear);
        }
        if (hRightRear.distance < 1.0f)
        {
            GetComponent<Rigidbody>().AddForceAtPosition((1.0f - hRightRear.distance) * hRightRear.normal, rightRear);
            GetComponent<Rigidbody>().AddForceAtPosition(dRightRear, rightRear);
        }
        if (hLeftFront.distance < 1.0f)
        {
            GetComponent<Rigidbody>().AddForceAtPosition((1.0f - hLeftFront.distance) * hLeftFront.normal, leftFront);
            GetComponent<Rigidbody>().AddForceAtPosition(dLeftFront, leftFront);
        }
        if (hRightFront.distance < 1.0f)
        {
            GetComponent<Rigidbody>().AddForceAtPosition((1.0f - hRightFront.distance) * hRightFront.normal, rightFront);
            GetComponent<Rigidbody>().AddForceAtPosition(dRightFront, rightFront);
        }
    }
}
