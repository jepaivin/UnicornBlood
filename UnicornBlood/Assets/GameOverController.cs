using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

public class GameOverController : MonoBehaviour
{
	public Text ScoreText;
	private static string[] MONTHS = {"Jan","Feb","Mar", "Apr","May","Jun", "Jul","Aug","Sep","Oct","Nov","Dec"};
	public void ShowScore(float score)
	{
		int year = DateTime.Now.Year + Mathf.FloorToInt (score);
		int month = DateTime.Now.Month + Mathf.FloorToInt ((score - Mathf.FloorToInt (score)) * 12);
		if (month > 12) {
			year++;
			month-=12;
		}
		ScoreText.text = "Armageddon pushed back to " + MONTHS[month] + "/" + year;

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
