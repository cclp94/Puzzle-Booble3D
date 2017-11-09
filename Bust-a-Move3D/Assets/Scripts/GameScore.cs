using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameScore : MonoBehaviour {

    private static GameScore _instance;
    [SerializeField]
    private Text scoreText;

    private int scoreAmount;

    public static GameScore Instance
    {
        get
        {
            if (_instance == null)
                _instance = new GameScore();
            return _instance;
        }
    }

    void Awake()
    {
        if (GameScore.Instance == null)
        {
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
	void Update () {
        scoreText.text = scoreAmount.ToString("D9");
	}

    public void addToScore(int value)
    {
        scoreAmount += value;
    }

    public int getScore()
    {
        return scoreAmount;
    }

    public void resetScore()
    {
        scoreAmount = 0;
    }
}
