using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour {

    public Text blueM;
    public Text orangeM;
    public Text timer;
    public Text ganador;
    public GameObject Mcar;
    public GameObject Menemy;

    public float time;

    private int blue, orange;
    private string min, sec;
    private bool finished;

    public static TypeOfGame tog;

    // Use this for initialization
    void Start () {
        blue = 0;
        orange = 0;

        blueM.text = blue.ToString();
        orangeM.text = orange.ToString();

        finished = false;

        //aqui se elege el modelo de los coches segun el ToG
        tog = GameObject.FindObjectOfType(typeof(TypeOfGame)) as TypeOfGame;
        if (tog.YM() == 2)
        {
            Mcar.transform.GetChild(0).gameObject.SetActive(false);
            Mcar.transform.GetChild(1).gameObject.SetActive(true);
        }
        else if (tog.YM() == 3)
        {
            Mcar.transform.GetChild(0).gameObject.SetActive(false);
            Mcar.transform.GetChild(2).gameObject.SetActive(true);
        }
        if (tog.EM() == 2)
        {
            Menemy.transform.GetChild(0).gameObject.SetActive(false);
            Menemy.transform.GetChild(1).gameObject.SetActive(true);
        }
        else if (tog.EM() == 3)
        {
            Menemy.transform.GetChild(0).gameObject.SetActive(false);
            Menemy.transform.GetChild(2).gameObject.SetActive(true);
        }


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
        timer.text = "0:00.00";
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
