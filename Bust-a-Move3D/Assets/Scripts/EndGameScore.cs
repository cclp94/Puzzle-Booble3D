using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndGameScore : MonoBehaviour {

    private int currentScore;
    private int highscore;
    private GameScore gameScore;

    [SerializeField]
    public Text scoreText;
    public Text highScoreText;
    public GameObject newHighScoreText;

	int currentScoreCounter;
	int highScoreCounter;

	// Use this for initialization
	void Start () {
		currentScoreCounter = 0;
		highScoreCounter = 0;
        gameScore = GameScore.Instance;
        currentScore = gameScore.getScore();
        highscore = gameScore.getHighScore();
        print(currentScore);
        print(highscore);
        if (currentScore == highscore)
            newHighScoreText.SetActive(true);
	}
	
	// Update is called once per frame
	void Update () {
        
        if (currentScoreCounter < currentScore){
            currentScoreCounter+=10;
            scoreText.text = currentScoreCounter.ToString("D9");
        }
		if (highScoreCounter < currentScore)
		{
			highScoreCounter+=10;
			highScoreText.text = highScoreCounter.ToString("D9");
		}
        
	}

    public void BackToMenu(){
        LevelManager.Instance.onMainMenuLevelChoose("Menu");
    }
}
