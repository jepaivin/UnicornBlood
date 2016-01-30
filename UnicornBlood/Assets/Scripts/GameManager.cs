using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameManager : MonoBehaviour
{
	public GameObject menu;
	public GameObject game;
	public GameObject gameOver;


	public static GameManager instance;

	void Awake()
	{
		instance = this;
	}

	// Use this for initialization
	void Start () 
	{
		game.SetActive (false);
		gameOver.SetActive (false);

		menu.SetActive (true);
		
		GameObject FunFactoid = game.GetComponent<GameController> ().funFactText;
		int numberOfFacts = game.GetComponent<GameController> ().FunFacts.Count;
		FunFactoid.GetComponent<Text> ().text = game.GetComponent<GameController> ().FunFacts[(int)Random.Range (0, numberOfFacts)];

	}
	

	// Update is called once per frame
	void Update () {
	
	}

	public void StartGame()
	{
		menu.SetActive (false);

		game.SetActive (true);
		game.GetComponent<GameController> ().StartGame ();
	}

	public void RestartGame()
	{
		game.SetActive (false);
		game.SetActive (true);

		game.GetComponent<GameController> ().StartGame ();

	}

	public void EndGame(float score)
	{
		Debug.Log ("END GAME");
		game.SetActive (false);
		gameOver.SetActive (true);
		gameOver.GetComponent<GameOverController> ().ShowScore (score);
	}

	public void GoToMenu()
	{
		game.SetActive (false);
		gameOver.SetActive (false);
		menu.SetActive (true);

	}
}
