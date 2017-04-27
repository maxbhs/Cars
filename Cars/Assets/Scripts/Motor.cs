using UnityEngine;
using System.Collections;

public class Motor : MonoBehaviour
{
    public Vector3 certerOfMass = Vector3.zero;
    public WheelCollider frontLeftWheel;
    public WheelCollider frontRightWheel;

    public float speed = 100;
    public float maxSteerAngle = 30;

    private float motorForce = 0;

    void Start() {
        gameObject.GetComponent<Rigidbody>().centerOfMass = certerOfMass;
    }

    void FixedUpdate()
    {
        motorForce = Input.GetAxis("Vertical") * speed;
        frontLeftWheel.motorTorque = motorForce;
        frontRightWheel.motorTorque = motorForce;


        float rotation = Input.GetAxis("Horizontal") * maxSteerAngle;
        frontLeftWheel.steerAngle = rotation;
        frontLeftWheel.steerAngle = rotation;
        frontLeftWheel.transform.localEulerAngles = new Vector3(0, rotation, 90);
        frontRightWheel.transform.localEulerAngles = new Vector3(0, rotation, 90);
    }
}