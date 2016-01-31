using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

public class GameOverController : MonoBehaviour
{
	public Text ScoreText;
	public Text ScoreText2;

	private static string[] MONTHS = {"Jan","Feb","Mar", "Apr","May","Jun", "Jul","Aug","Sep","Oct","Nov","Dec"};
	public void ShowScore(float score, float highScore)
	{
		float highscore = PlayerPrefs.GetFloat ("HighScore");
		if (score > highscore)
		{
			PlayerPrefs.SetFloat("HighScore", score);
			highscore = score;
		}

		DateTime EndTime = DateTime.Now.AddDays (score * 365.0f);
		int year = EndTime.Year;
		int month = EndTime.Month;

		DateTime HighEndTime = DateTime.Now.AddDays (highscore * 365.0f);
		int highYear = HighEndTime.Year;
		int highMonth = HighEndTime.Month;

		ScoreText.text = "Armageddon pushed back to " + MONTHS[month] + " " + year + " (Best: " + MONTHS[highMonth]+ " " + highYear + ")";

		/*
		if (score > highScore)
		{
			ScoreText2.text = ScoreText.text;
		}*/

	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetMouseButtonUp (0)) {
			GameObject.FindObjectOfType<GameManager>().GoToMenu();
		}
	
	}
}
