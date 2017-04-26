using UnityEngine;
using System.Collections;

public class Hover : MonoBehaviour
{

    public float fMag = 0f;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

        Vector3 leftRear = transform.TransformPoint(new Vector3(-0.5f, -0.5f, -0.5f));
        Vector3 rightRear = transform.TransformPoint(new Vector3(0.5f, -0.5f, -0.5f));
        Vector3 leftFront = transform.TransformPoint(new Vector3(-0.5f, -0.5f, 0.5f));
        Vector3 rightFront = transform.TransformPoint(new Vector3(0.5f, -0.5f, 0.5f));

        bool bLeftRear, bRightRear, bLeftFront, bRightFront;

        bLeftRear = Physics.Raycast(leftRear + 0.1f * transform.up, -transform.up, 1.0f);
        bRightRear = Physics.Raycast(rightRear + 0.1f * transform.up, -transform.up, 1.0f);
        bLeftFront = Physics.Raycast(leftFront + 0.1f * transform.up, -transform.up, 1.0f);
        bRightFront = Physics.Raycast(rightFront + 0.1f * transform.up, -transform.up, 1.0f);

        Debug.DrawRay(leftRear, -transform.up, bLeftRear ? Color.red : Color.black);
        Debug.DrawRay(rightRear, -transform.up, bRightRear ? Color.red : Color.black);
        Debug.DrawRay(leftFront, -transform.up, bLeftFront ? Color.red : Color.black);
        Debug.DrawRay(rightFront, -transform.up, bRightFront ? Color.red : Color.black);

        // Suspension
        if (bLeftRear)
            GetComponent<Rigidbody>().AddForceAtPosition(fMag * Vector3.up, leftRear);
        if (bRightRear)
            GetComponent<Rigidbody>().AddForceAtPosition(fMag * Vector3.up, rightRear);
        if (bLeftFront)
            GetComponent<Rigidbody>().AddForceAtPosition(fMag * Vector3.up, leftFront);
        if (bRightFront)
            GetComponent<Rigidbody>().AddForceAtPosition(fMag * Vector3.up, rightFront);

    }
}
