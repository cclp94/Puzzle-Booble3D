using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameScore : MonoBehaviour {

    private static GameScore _instance;
    [SerializeField]
    private Text scoreText;

    private int scoreAmount;

    private int highScore;

    public static GameScore Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new GameScore();
            }
            return _instance;
        }
    }

    void Awake()
    {
        if (GameScore.Instance == null)
        {
            highScore = 0;
            _instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            GameObject.Destroy(this.gameObject);
        }

    }

    // Use this for initialization
    void Start() {
        scoreAmount = 0;
    }

    // Update is called once per frame
    private bool mPaused;
	void Update () {
        scoreText.text = scoreAmount.ToString("D9");
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            mPaused = !mPaused;
            if(mPaused){
                Time.timeScale = 0;
                PauseMenu.Instance.gameObject.SetActive(true);
            }else{
				Time.timeScale = 1;
				PauseMenu.Instance.gameObject.SetActive(false);
            }
        }
    }

    public void addToScore(int value)
    {
        scoreAmount += value;
    }

    public int getScore()
    {
        return scoreAmount;
    }

    public int getHighScore(){
        return highScore;
    }

    public void resetScore()
    {
        scoreAmount = 0;
    }

    public void checkHighScore(){
        if (scoreAmount > highScore)
            highScore = scoreAmount;
    }
}
