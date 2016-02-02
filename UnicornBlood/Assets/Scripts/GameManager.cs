using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameManager : MonoBehaviour
{
	public GameObject menu;
	public GameObject game;
	public GameObject gameOver;
	public AudioSource audioSource;

	private float highScore = 0;

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

		audioSource.Play ();

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

		AudioManager.Instance.SetGameStarted();
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

		audioSource.Play ();

		Debug.Log ("Score :"+score+ " vs. "+ highScore);

		gameOver.GetComponent<GameOverController> ().ShowScore (score, highScore);
		if (score > highScore)
		{
			highScore = score;
		}
	}

	public void GoToMenu()
	{
		game.SetActive (false);
		gameOver.SetActive (false);
		menu.SetActive (true);

	}
}
