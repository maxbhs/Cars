﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CarController : MonoBehaviour {

    static public Rigidbody body;
    float deadZone = 0.2f;

    public Text turboText;
    public Text speedText;

    static public bool grounded = true;
    public float groundedDrag = 3f;
    public float maxVelocity = 50;
    public float hoverForce = 1000;
    public float gravityForce = 1000f;
    public float hoverHeight = 1f;

    public float boostFactor = 1.0f;
    public float boostImpulse = 100000f;
    private bool max = false;

    public float forwardAcceleration = 8000f;
    public float reverseAcceleration = 4000f;
    float thrust = 0f;

    public float turnStrength = 1000f;
    float turnValue = 0f;
    private int cont = 0;
    private int turbo; //va de 0 a 100
    private float speed;
    bool restarTurbo;

    //int layerMask;

	// Use this for initialization
	void Start () {
        body = GetComponent<Rigidbody>();
        body.centerOfMass = new Vector3(0.0f,-0.5f,0.0f);
        turbo = 40;
        SetTurboText();
        restarTurbo = true;

        //layerMask = 1 << LayerMask.NameToLayer("Vehicle");
        //layerMask = ~layerMask;
	}

    void FixedUpdate(){

       
        // Main Thrust
        thrust = 0.0f;
        float acceleration = Input.GetAxis("Vertical");
        if (acceleration > deadZone)
            thrust = acceleration * forwardAcceleration;
        else if (acceleration < -deadZone)
            thrust = acceleration * reverseAcceleration;

        // Turning
        turnValue = 0.0f;
        float turnAxis = Input.GetAxis("Horizontal");
        if (Mathf.Abs(turnAxis) > deadZone){
            if (Input.GetKey(KeyCode.LeftShift) ){
                turnValue = turnAxis*2;
            }
            else{
                turnValue = turnAxis;
            }
        }
       

        
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

        Physics.Raycast(leftRear + 0.2f * transform.up , -transform.up, out hLeftRear);
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

        cont = 0;

        if (hLeftRear.distance < hoverHeight + 0.2f)
        {
            body.AddForceAtPosition(hLeftRear.normal * hoverForce * crLeftRear, leftRear);
            body.AddForceAtPosition(dLeftRear, leftRear);
            grounded = true;
        }
        else 
        {
             cont = cont + 1;
        }


        if (hRightRear.distance < hoverHeight + 0.2f)
        {
            body.AddForceAtPosition(hRightRear.normal * hoverForce * crRightRear, rightRear);
            body.AddForceAtPosition(dRightRear, rightRear);
            grounded = true;
        }
        else
        {
            cont = cont + 1;
        }

        if (hLeftFront.distance < hoverHeight + 0.2f)
        {
            body.AddForceAtPosition(hLeftFront.normal * hoverForce * crLeftFront, leftFront);
            body.AddForceAtPosition(dLeftFront, leftFront);
            grounded = true;
        }
        else
        {
            cont = cont + 1;
        }


        if (hRightFront.distance < hoverHeight + 0.2f)
        {
            body.AddForceAtPosition(hRightFront.normal * hoverForce * crRightFront, rightFront);
            body.AddForceAtPosition(dRightFront, rightFront);
            grounded = true;
        }
        else
        {
            cont = cont + 1;
        }


        if (cont == 4) {
            grounded = false;
                body.AddForceAtPosition(-Vector3.up * gravityForce * 0.25f, leftRear);
                body.AddForceAtPosition(-Vector3.up * gravityForce * 0.25f, rightRear);
                body.AddForceAtPosition(-Vector3.up * gravityForce * 0.25f, leftFront);
                body.AddForceAtPosition(-Vector3.up * gravityForce * 0.25f, rightFront);
            
        }
        else if (cont == 1 || cont == 2 || cont == 3) {
            
                body.AddForceAtPosition(-Vector3.up * gravityForce * hLeftRear.distance * 0.25f, leftRear);
                body.AddForceAtPosition(-Vector3.up * gravityForce * hRightRear.distance * 0.25f, rightRear);
                body.AddForceAtPosition(-Vector3.up * gravityForce * hLeftFront.distance * 0.25f, leftFront);
                body.AddForceAtPosition(-Vector3.up * gravityForce * hRightFront.distance * 0.25f, rightFront);
            
        }
        else if (cont == 0)
        {
                body.AddForceAtPosition(-transform.up * gravityForce * 0.25f , leftRear);
                body.AddForceAtPosition(-transform.up * gravityForce * 0.25f , rightRear);
                body.AddForceAtPosition(-transform.up * gravityForce * 0.25f , leftFront);
                body.AddForceAtPosition(-transform.up * gravityForce * 0.25f , rightFront);
        }

            
        if (grounded)
        {
            body.drag = groundedDrag;
            // Handle Forward and Reverse forces
            if (Mathf.Abs(thrust) > 0)
               body.AddForceAtPosition(transform.forward * thrust, transform.position - 0.6f * transform.up);

          
            // Rotation
		    body.AddTorque(turnValue * turnStrength * transform.up);
		
		    // Traction
            body.AddForce(-0.1f*Vector3.Dot(body.velocity, transform.right) * transform.right);
            
            
        }
        else
        {
                body.drag = 0.1f;
           
        }

        if (Input.GetMouseButton(0) && turbo > 0)
        {
            boostFactor = 2.0f;
            GetComponent<Rigidbody>().AddForceAtPosition(boostFactor * boostImpulse * transform.forward,
                                                       transform.position - 0.6f * transform.up);

            if (restarTurbo) //si usamos este booleano, el turbo dura el doble
            {
                restarTurbo = false;
                turbo = turbo - 1;
                SetTurboText();
            }
            else restarTurbo = true;
            
        }
        else boostFactor = 1.0f;

        speed = body.velocity.sqrMagnitude;
        SetSpeedText();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Turbo"))
        {
            if (turbo < 100)
            {   
                other.gameObject.SetActive(false);
                if (turbo <= 80)
                {
                    turbo = turbo + 20;
                }
                else
                {
                    turbo = turbo + (100 - turbo);
                }
                SetTurboText();
            }
        }
    }

    void SetTurboText ()
    {
       turboText.text = "Turbo: " + turbo.ToString();
    }
    void SetSpeedText()
    {
        speedText.text = "Speed: " + (body.velocity.sqrMagnitude).ToString("#.##");
    }
}