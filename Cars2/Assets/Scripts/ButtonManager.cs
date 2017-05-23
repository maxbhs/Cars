using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class ButtonManager : MonoBehaviour {

	public void NewGame (string level)
    {
        SceneManager.LoadScene(level);
    }

    public void ExitGame ()
    {
        Application.Quit();
    }

}
