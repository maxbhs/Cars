using UnityEngine;
using System.Collections;

public class Rotator : MonoBehaviour {
    public GameObject[] turbos;
    public int contvalue = 200;
    private int[] cont;
    

	// Use this for initialization
	void Start () {
        cont = new int[turbos.Length];
        for (int i = 0; i < cont.Length; ++i)
        {
            cont[i] = contvalue;
        }
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        for (int i = 0; i < turbos.Length; ++i)
        {
            turbos[i].transform.Rotate(new Vector3(15, 30, 45) * Time.deltaTime);
            if(!turbos[i].activeSelf)
            {
                cont[i] = cont[i] - 1;
                if (cont[i] == 0)
                {
                    turbos[i].SetActive(true);
                    cont[i] = contvalue;
                }
            }
        }
        


    }
}
