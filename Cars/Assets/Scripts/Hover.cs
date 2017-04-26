using UnityEngine;
using System.Collections;

public class Hover : MonoBehaviour
{

    public float speed = 90f;
    public float turnSpeed = 5f;
    public float hoverForce = 65f;
    public float hoverHeight = 3.5f;
    private float powerInput;
    private float turnInput;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update() {
       
    }

    void FixedUpdate()
    {

        Vector3 leftRear = transform.TransformPoint(new Vector3(-0.5f, -0.5f, -0.5f));
        Vector3 rightRear = transform.TransformPoint(new Vector3(0.5f, -0.5f, -0.5f));
        Vector3 leftFront = transform.TransformPoint(new Vector3(-0.5f, -0.5f, 0.5f));
        Vector3 rightFront = transform.TransformPoint(new Vector3(0.5f, -0.5f, 0.5f));

        bool bLeftRear, bRightRear, bLeftFront, bRightFront;

        RaycastHit hitLeftRear;
        RaycastHit hitRightRear;
        RaycastHit hitLeftFront;
        RaycastHit hitRightFront;

        bLeftRear = Physics.Raycast(leftRear + 0.1f * transform.up, -transform.up, out hitLeftRear, hoverHeight);
        bRightRear = Physics.Raycast(rightRear + 0.1f * transform.up, -transform.up, out hitRightRear, hoverHeight);
        bLeftFront = Physics.Raycast(leftFront + 0.1f * transform.up, -transform.up, out hitLeftFront, hoverHeight);
        bRightFront = Physics.Raycast(rightFront + 0.1f * transform.up, -transform.up, out hitRightFront, hoverHeight);

        Debug.DrawRay(leftRear, -transform.up, bLeftRear ? Color.red : Color.black);
        Debug.DrawRay(rightRear, -transform.up, bRightRear ? Color.red : Color.black);
        Debug.DrawRay(leftFront, -transform.up, bLeftFront ? Color.red : Color.black);
        Debug.DrawRay(rightFront, -transform.up, bRightFront ? Color.red : Color.black);

        // Suspension
        if (bLeftRear)
        {
            float proportionalHeight = (hoverHeight - hitLeftRear.distance) / hoverHeight;
            Vector3 appliedHoverForce = Vector3.up * proportionalHeight * hoverForce;
            GetComponent<Rigidbody>().AddForceAtPosition(appliedHoverForce, leftRear);
        }
        if (bRightRear)
        {
            float proportionalHeight = (hoverHeight - hitRightRear.distance) / hoverHeight;
            Vector3 appliedHoverForce = Vector3.up * proportionalHeight * hoverForce;
            GetComponent<Rigidbody>().AddForceAtPosition(appliedHoverForce, rightRear);
        }
        if (bLeftFront)
        {
            float proportionalHeight = (hoverHeight - hitLeftFront.distance) / hoverHeight;
            Vector3 appliedHoverForce = Vector3.up * proportionalHeight * hoverForce;
            GetComponent<Rigidbody>().AddForceAtPosition(appliedHoverForce, leftFront);
        }
        if (bRightFront)
        {
            float proportionalHeight = (hoverHeight - hitRightFront.distance) / hoverHeight;
            Vector3 appliedHoverForce = Vector3.up * proportionalHeight * hoverForce;
            GetComponent<Rigidbody>().AddForceAtPosition(appliedHoverForce, rightFront);
        }
    }
}
