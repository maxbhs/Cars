using UnityEngine;
using System.Collections;

public class Ball : MonoBehaviour {

    float speed;
    Vector3 heading;
    float distance;
    Vector3 direction;
    
    Vector3 originalP;
    Quaternion originalR;

    public GameObject car;
    public GameObject[] carIA;
    public GameObject[] carIA2;
    public LevelManager LM;

    // Use this for initialization
    void Start()
    {
        originalP = transform.position;
        originalR = transform.rotation;


    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name == "Car")
        {
            //Physics.IgnoreCollision(car.GetComponent<Collider>(), GetComponent<Collider>(), true);

          
            //speed = (car.GetComponent<Rigidbody>().velocity - GetComponent<Rigidbody>().velocity).magnitude;
            //heading = transform.position - car.transform.position;
            //distance = heading.magnitude;
            //direction = heading / distance;
            //GetComponent<Rigidbody>().velocity -= speed * direction;
            
        }        
    }

	// Update is called once per frame
	void FixedUpdate () {
        
        if (transform.position.z >= 336.76)
        {
            Reset();
            LM.orangeGol();
            
        }
        else if (transform.position.z <= 43.16)
        {
            Reset();
            LM.blueGol();
        }
	}

    void Reset ()
    {
        transform.position = originalP;
        transform.rotation = originalR;
        GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);
        GetComponent<Rigidbody>().angularVelocity = new Vector3(0, 0, 0);
        car.GetComponent<CarController>().Reset();
        for (int i = 0; i < carIA.Length; i++ )
            carIA[i].GetComponent<CarControllerIA>().Reset();

        if (carIA2 != null)
            for (int i = 0; i < carIA2.Length; i++)
                carIA2[i].GetComponent<CarControllerIA2>().Reset();
    }
}
