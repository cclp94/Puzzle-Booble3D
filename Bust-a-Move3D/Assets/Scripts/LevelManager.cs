using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour {

    private static LevelManager _instance;

    [SerializeField]
    public GameObject scoreBoard;

    public static LevelManager Instance
    {
        get
        {
            return _instance;
        }
    }

	// Use this for initialization
	void Awake () {
        if (LevelManager.Instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(this.gameObject);
            scoreBoard.SetActive(false);
        }
        else
        {
            GameObject.Destroy(this.gameObject);
        }
        
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void onMainMenuLevelChoose(string name)
    {
        loadScene(name);
    }

    void loadScene(string name)
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(name);
        gameObject.SetActive(false);
        if (name != "Menu")
            scoreBoard.SetActive(true);
    }

    public void gameWon(string sceneLoaded){
        if (sceneLoaded.Equals("level1"))
        {
            // Go to level 2
            loadScene("level2");
        }
        else if (sceneLoaded.Equals("level2"))
        {
            // return to main menu
            scoreBoard.SetActive(false);
            loadScene("Menu");
            GameScore.Instance.resetScore();
            gameObject.SetActive(true);
        }
    }

    public void gameLost()
    {
        // return to main menu
        scoreBoard.SetActive(false);
        
        loadScene("Menu");
        gameObject.SetActive(true);
    }
}
