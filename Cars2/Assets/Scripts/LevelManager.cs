using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LevelManager : MonoBehaviour {

    public Text blueM;
    public Text orangeM;

    int blue, orange;

    // Use this for initialization
    void Start () {
        blue = 0;
        orange = 0;

        blueM.text = blue.ToString();
        orangeM.text = orange.ToString();
    }
	
	public void blueGol ()
    {
        ++blue;
        blueM.text = blue.ToString();
    }

    public void orangeGol()
    {
        ++orange;
        orangeM.text = orange.ToString();
    }

}
