using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour {

    public Text blueM;
    public Text orangeM;
    public Text timer;
    public Text ganador;

    public float time;

    private int blue, orange;
    private string min, sec;
    private bool finished;

    // Use this for initialization
    void Start () {
        blue = 0;
        orange = 0;

        blueM.text = blue.ToString();
        orangeM.text = orange.ToString();

        finished = false;

    }

    void FixedUpdate ()
    {
        if(Input.GetKey(KeyCode.Escape)) SceneManager.LoadScene("Menu");
        if (time <= 0)
        {
            finish();
            return; //quizas a los profes no les mola esto jaja
        }
        time -= Time.deltaTime;
        min = ((int)time / 60).ToString();
        sec = (time % 60).ToString("f2");

        if (time % 60 < 10)
        {
            timer.text = min + ":0" + sec;
        } 
        else timer.text = min + ":" + sec;
        
    }
	
	public void blueGol ()
    {
        if (!finished)
        {
            ++blue;
            blueM.text = blue.ToString();
        }
        
    }

    public void orangeGol()
    {
        if (!finished)
        {
            ++orange;
            orangeM.text = orange.ToString();
        }
        
    }

    private void finish()
    {
        finished = true;
        timer.color = Color.yellow;
        if (blue > orange)
        {
            ganador.text = "   BLUE TEAM WINS";
            ganador.color = Color.blue;
        }
        else if (orange > blue)
        {
            ganador.text = "ORANGE TEAM WINS";
        }
        else
        {
            ganador.text = "             DRAW";
            ganador.color = Color.black;
        }

    }

}
