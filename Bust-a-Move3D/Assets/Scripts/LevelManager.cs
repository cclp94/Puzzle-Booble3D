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
        bool isMenu = name == "Menu";
        if(isMenu)
            GameScore.Instance.resetScore();
        UnityEngine.SceneManagement.SceneManager.LoadScene(name);
        gameObject.SetActive(isMenu);
        scoreBoard.SetActive(!isMenu);
    }

    public void gameWon(string sceneLoaded){
            // Show score
            scoreBoard.SetActive(false);

            GameScore.Instance.checkHighScore();
            loadScene("ScoreScene");
    }

    public void gameLost()
    {
        // return to main menu
        scoreBoard.SetActive(false);
        
        loadScene("Menu");
        gameObject.SetActive(true);
    }
}
