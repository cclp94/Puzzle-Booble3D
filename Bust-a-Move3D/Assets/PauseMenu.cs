using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour {
    private static PauseMenu _instance;
    public Text highScoreText;

	public static PauseMenu Instance
	{
		get
		{
			if (_instance == null)
			{
				_instance = new PauseMenu();
			}
			return _instance;
		}
	}

	void Awake()
	{
		if (PauseMenu.Instance == null)
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
	void Start () {
        gameObject.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () {
		highScoreText.text = GameScore.Instance.getScore().ToString("D9");
	}
}
