using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class TypeOfGame : MonoBehaviour {

    public int GameMode; //1 = 1vs1, 2 = 2vs2, 3 = 3vs3
    public int yourModel, enemyModel;

    public static TypeOfGame instance;


	public void SetGameMode (int gm)
    {
        GameMode = gm;
    }
	
    public void SetYourModel (int ym)
    {
        yourModel = ym;
    }

    public void SetenemyModel(int em)
    {
        enemyModel = em;
    }

    public int GM()
    {
        return GameMode;
    }
    public int YM()
    {
        return yourModel;
    }
    public int EM()
    {
        return enemyModel;
    }

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this);
        }
        else if (instance != this)
        {
            Destroy(this.gameObject);
        }
    }
}
