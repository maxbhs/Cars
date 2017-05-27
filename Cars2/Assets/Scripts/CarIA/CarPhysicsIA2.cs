using UnityEngine;
using System.Collections;

public class CarPhysicsIA2 : MonoBehaviour
{

    public bool grounded = true;
    public float groundedDrag = 3f;

    public float hoverForce = 2000;
    public float gravityForce = 2000f;
    public float hoverHeight = 1f;

    float boostFactor;
    public float boostImpulse = 100000f;

    public float turnStrength = 1000f;
    public float thrust;
    public float turnValue;
    public float forwardAcceleration = 8000f;
    public float reverseAcceleration = 4000f;


    private int rodas = 0;

    // Use this for initialization
    void Start()
    {

        GetComponent<Rigidbody>().centerOfMass = new Vector3(0.0f, -0.5f, 0.0f);
    }

    void FixedUpdate()
    {

        thrust = GetComponent<CarControllerIA2>().thrust;
        turnValue = GetComponent<CarControllerIA2>().turnValue;
        boostFactor = GetComponent<CarControllerIA2>().boostFactor;

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

        ///Suspension force at position
        Vector3 sLeftRear = Vector3.Project(vLeftRear, transform.up);
        Vector3 sRightRear = Vector3.Project(vRightRear, transform.up);
        Vector3 sLeftFront = Vector3.Project(vLeftFront, transform.up);
        Vector3 sRightFront = Vector3.Project(vRightFront, transform.up);

        //Ratcast to determine compress ratio
        RaycastHit hLeftRear, hRightRear, hLeftFront, hRightFront;

        Physics.Raycast(leftRear + 0.2f * transform.up, -transform.up, out hLeftRear);
        Physics.Raycast(rightRear + 0.2f * transform.up, -transform.up, out hRightRear);
        Physics.Raycast(leftFront + 0.2f * transform.up, -transform.up, out hLeftFront);
        Physics.Raycast(rightFront + 0.2f * transform.up, -transform.up, out hRightFront);



        //Compression ratio
        float crLeftRear = (1.0f - hLeftRear.distance) / hoverHeight;
        float crRightRear = (1.0f - hRightRear.distance) / hoverHeight;
        float crLeftFront = (1.0f - hLeftFront.distance) / hoverHeight;
        float crRightFront = (1.0f - hRightFront.distance) / hoverHeight;

        //New suspension
        Vector3 nsLeftRear = transform.up * crLeftRear;
        Vector3 nsRightRear = transform.up * crRightRear;
        Vector3 nsLeftFront = transform.up * crLeftFront;
        Vector3 nsRightFront = transform.up * crRightFront;

        //Delta suspension
        Vector3 dLeftRear = nsLeftRear - sLeftRear;
        Vector3 dRightRear = nsRightRear - sRightRear;
        Vector3 dLeftFront = nsLeftFront - sLeftFront;
        Vector3 dRightFront = nsRightFront - sRightFront;

        Debug.DrawRay(leftRear, -transform.up, (hLeftRear.distance < hoverHeight) ? Color.red : Color.black);
        Debug.DrawRay(rightRear, -transform.up, (hRightRear.distance < hoverHeight) ? Color.red : Color.black);
        Debug.DrawRay(leftFront, -transform.up, (hLeftFront.distance < hoverHeight) ? Color.red : Color.black);
        Debug.DrawRay(rightFront, -transform.up, (hRightFront.distance < hoverHeight) ? Color.red : Color.black);

        rodas = 0;

        if (hLeftRear.distance < hoverHeight + 0.25f)
        {
            GetComponent<Rigidbody>().AddForceAtPosition(hLeftRear.normal * hoverForce * crLeftRear, leftRear);
            GetComponent<Rigidbody>().AddForceAtPosition(dLeftRear, leftRear);
            grounded = true;
        }
        else
        {
            rodas = rodas + 1;
        }


        if (hRightRear.distance < hoverHeight + 0.25f)
        {
            GetComponent<Rigidbody>().AddForceAtPosition(hRightRear.normal * hoverForce * crRightRear, rightRear);
            GetComponent<Rigidbody>().AddForceAtPosition(dRightRear, rightRear);
            grounded = true;
        }
        else
        {
            rodas = rodas + 1;
        }

        if (hLeftFront.distance < hoverHeight + 0.25f)
        {
            GetComponent<Rigidbody>().AddForceAtPosition(hLeftFront.normal * hoverForce * crLeftFront, leftFront);
            GetComponent<Rigidbody>().AddForceAtPosition(dLeftFront, leftFront);
            grounded = true;
        }
        else
        {
            rodas = rodas + 1;
        }


        if (hRightFront.distance < hoverHeight + 0.25f)
        {
            GetComponent<Rigidbody>().AddForceAtPosition(hRightFront.normal * hoverForce * crRightFront, rightFront);
            GetComponent<Rigidbody>().AddForceAtPosition(dRightFront, rightFront);
            grounded = true;
        }
        else
        {
            rodas = rodas + 1;
        }


        if (rodas == 4)
        {
            grounded = false;
            GetComponent<Rigidbody>().AddForceAtPosition(-Vector3.up * gravityForce * 0.25f, leftRear);
            GetComponent<Rigidbody>().AddForceAtPosition(-Vector3.up * gravityForce * 0.25f, rightRear);
            GetComponent<Rigidbody>().AddForceAtPosition(-Vector3.up * gravityForce * 0.25f, leftFront);
            GetComponent<Rigidbody>().AddForceAtPosition(-Vector3.up * gravityForce * 0.25f, rightFront);

        }
        else if (rodas == 1 || rodas == 2 || rodas == 3)
        {

            GetComponent<Rigidbody>().AddForceAtPosition(-Vector3.up * gravityForce * hLeftRear.distance * 0.2f, leftRear);
            GetComponent<Rigidbody>().AddForceAtPosition(-Vector3.up * gravityForce * hRightRear.distance * 0.2f, rightRear);
            GetComponent<Rigidbody>().AddForceAtPosition(-Vector3.up * gravityForce * hLeftFront.distance * 0.2f, leftFront);
            GetComponent<Rigidbody>().AddForceAtPosition(-Vector3.up * gravityForce * hRightFront.distance * 0.2f, rightFront);

        }
        else if (rodas == 0)
        {
            GetComponent<Rigidbody>().AddForceAtPosition(-transform.up * gravityForce * 0.25f, leftRear);
            GetComponent<Rigidbody>().AddForceAtPosition(-transform.up * gravityForce * 0.25f, rightRear);
            GetComponent<Rigidbody>().AddForceAtPosition(-transform.up * gravityForce * 0.25f, leftFront);
            GetComponent<Rigidbody>().AddForceAtPosition(-transform.up * gravityForce * 0.25f, rightFront);
        }


        if (grounded)
        {
            GetComponent<Rigidbody>().drag = groundedDrag;
            // Handle Forward and Reverse forces
            if (Mathf.Abs(thrust) > 0)
                GetComponent<Rigidbody>().AddForceAtPosition(transform.forward * thrust, transform.position - 0.6f * transform.up);

            // Rotation
            GetComponent<Rigidbody>().AddTorque(turnValue * turnStrength * transform.up);

            // Traction
            GetComponent<Rigidbody>().AddForce(-0.4f * Vector3.Dot(GetComponent<Rigidbody>().velocity, transform.right) * transform.right);


        }
        else
        {
            GetComponent<Rigidbody>().drag = 0.1f;

        }

        GetComponent<Rigidbody>().AddForceAtPosition(boostFactor * boostImpulse * transform.forward,
                                                           transform.position - 0.6f * transform.up);
    }
}
