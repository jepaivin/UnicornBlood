using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour
{
	public GameObject menu;
	public GameObject game;

	public static GameManager instance;

	void Awake()
	{
		instance = this;
	}

	// Use this for initialization
	void Start () 
	{
		game.SetActive (false);
		menu.SetActive (true);
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void StartGame()
	{
		menu.SetActive (false);

		game.SetActive (true);
	}

	public void EndGame()
	{
		game.SetActive (false);
		menu.SetActive (true);
	}
}
