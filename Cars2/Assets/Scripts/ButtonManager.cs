using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class ButtonManager : MonoBehaviour {

    public static TypeOfGame tog;

	public void NewGame (string level)
    {
        SceneManager.LoadScene(level);
    }


    public void ExitGame ()
    {
        Application.Quit();
    }

    public void Back()
    {
        SceneManager.LoadScene("Menu");
    }

    public void GM_NW(int gm) //puts the game mode in the new game
    {
        tog = GameObject.FindObjectOfType(typeof(TypeOfGame)) as TypeOfGame;
        tog.SetGameMode(gm);
    }

    public void YTM_NW(int ym) //puts your team model in the new game
    {
        tog = GameObject.FindObjectOfType(typeof(TypeOfGame)) as TypeOfGame;
        tog.SetYourModel(ym);
    }

    public void cargarL()
    {
        tog = GameObject.FindObjectOfType(typeof(TypeOfGame)) as TypeOfGame;
        int a = tog.GM();
        if (a == 1) SceneManager.LoadScene("1VS1");
        else if (a == 2) SceneManager.LoadScene("2VS2");  //aqui se tendra que meter las escenas 2vs2 i 3vs3
        else SceneManager.LoadScene("3VS3");
    }

}
