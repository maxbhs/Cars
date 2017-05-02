using UnityEngine;
using System.Collections;

public class Jump : MonoBehaviour {
	
	public float forceMagnitude = 0.0f;
    public float forceTorque = 0.0f;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKey(KeyCode.Space))
        {
            gameObject.GetComponent<Rigidbody>().AddForceAtPosition(forceTorque* Vector3.up, transform.position);
            gameObject.GetComponent<Rigidbody>().AddTorque(forceMagnitude * transform.right);
        }
	}
}
