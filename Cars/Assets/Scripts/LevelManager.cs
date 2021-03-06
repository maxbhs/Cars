﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour {

    public Text blueM;
    public Text orangeM;
    public Text timer;
    public Text ganador;
    public Text overtime;
    public Text exit;
    public GameObject[] AllyCars;
    public GameObject[] EnemyCars;

    public float time;

    private int blue, orange;
    private string min, sec;
    private bool finished, draw;

    public static TypeOfGame tog;

    // Use this for initialization
    void Start () {
        blue = 0;
        orange = 0;

        blueM.text = blue.ToString();
        orangeM.text = orange.ToString();

        draw = false;
        finished = false;

        //aqui se elege el modelo de los coches segun el ToG
        tog = GameObject.FindObjectOfType(typeof(TypeOfGame)) as TypeOfGame;
        if (tog.YM() == 2)
        {
            for (int i = 0; i < AllyCars.Length; i++)
            {
                AllyCars[i].transform.GetChild(0).gameObject.SetActive(false);
                AllyCars[i].transform.GetChild(1).gameObject.SetActive(true);
            }
        }
        else if (tog.YM() == 3)
        {
            for (int i = 0; i < AllyCars.Length; i++)
            {
                AllyCars[i].transform.GetChild(0).gameObject.SetActive(false);
                AllyCars[i].transform.GetChild(2).gameObject.SetActive(true);
            }
        }
        if (tog.EM() == 2)
        {
            for (int i = 0; i < EnemyCars.Length; i++)
            {
                EnemyCars[i].transform.GetChild(0).gameObject.SetActive(false);
                EnemyCars[i].transform.GetChild(1).gameObject.SetActive(true);
            }
        }
        else if (tog.EM() == 3)
        {
            for (int i = 0; i < EnemyCars.Length; i++)
            {
                EnemyCars[i].transform.GetChild(0).gameObject.SetActive(false);
                EnemyCars[i].transform.GetChild(2).gameObject.SetActive(true);
            }
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
        if (!finished || draw)
        {
            ++blue;
            blueM.text = blue.ToString();
            if (draw)
            {
                overtime.text = "";
                ganador.text = "   BLUE TEAM WINS";
                ganador.color = Color.blue;
                exit.text = "Press Esc to exit";
                draw = false;
            }
        }

        
    }

    public void orangeGol()
    {
        if (!finished || draw)
        {
            ++orange;
            orangeM.text = orange.ToString();
            if (draw)
            {
                overtime.text = "";
                ganador.text = "ORANGE TEAM WINS";
                exit.text = "Press Esc to exit";
                draw = false;
            }
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
            exit.text = "Press Esc to exit";
        }
        else if (orange > blue)
        {
            ganador.text = "ORANGE TEAM WINS";
            exit.text = "Press Esc to exit";
        }
        else
        {
            draw = true;
            overtime.text = "Overtime";
        }

    }

}
