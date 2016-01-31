using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Collections;
using System.Linq;
using System;

public class GameController : MonoBehaviour 
{
	public List<GameObject> Symbols;
	public List<GameObject> Animals;
	public Text PromptText;
	public Text LivesText;

	public Text TimeText;
	public int AnimalsUsed = 0;
	public int LivesLeft = 3;

	public ScorePanelController ScorePanel;
	private float InitialTurnTime = 30;
	private float MinTurnTime = 10;
	private float TurnTimeDecreesSpeed = 2;
	private int GameRound = 0;

	private float TurnTime = 15;
	private float TurnStartTime;
	public bool TurnActive = false;


	public GameObject CheckMarkPrefab;
	private List<GameObject> AnimalInstances = new List<GameObject>();
	private GameObject CurrentSymbol;
	private float Score = 0.0f;

	public List<String> FunFacts = new List<String>();
	public GameObject funFactText;

	public List<GameObject> LifeHorns;

	// Use this for initialization
	
	void Awake()
	{

	}

	public void StartGame () 
	{
		ShowPrompt("GAME STARTING");
		Score = 0.0f;
		ShowScore (Score);
		ScorePanel.gameObject.SetActive (false);
		TurnTime = InitialTurnTime  + 1; // decrease per turn, hence +1
		GameRound = 0;
		StartNewTurn ();
	}

	void StartNewTurn()
	{
		StartCoroutine (StartNewTurnAsync ());
	}

	IEnumerator StartNewTurnAsync()
	{
		yield return null;	
		while (ScorePanel.gameObject.activeSelf) {
			yield return null;
		}
		GameRound ++;
		if (TurnTime > MinTurnTime) {
			TurnTime -= TurnTimeDecreesSpeed;
		}
		ClearAnimals ();
		FindObjectOfType<PaintController> ().Clear ();

		yield return new WaitForSeconds(0.1f);
		SpawnSymbol ();
		yield return new WaitForSeconds(0.1f);

		for (int i = 0; i < 4; i++) {
			SpawnAnimal (i);
			yield return new WaitForSeconds(0.1f);
		}
		yield return new WaitForSeconds(0.1f);
		TurnStartTime = Time.realtimeSinceStartup;
		TurnActive = true;
		AnimalsUsed = 0;

	}

	void SpawnSymbol()
	{
		int index = UnityEngine.Random.Range(0, Symbols.Count);
		if (CurrentSymbol != null)
		{
			GameObject.Destroy(CurrentSymbol);
		}
		CurrentSymbol = GameObject.Instantiate (Symbols [index]) as GameObject;
		CurrentSymbol.transform.parent = transform;
	}
		
	void ClearAnimals()
	{
		var canvas = GetComponentInChildren<Canvas> ();
		foreach(var go in AnimalInstances)
		{
			GameObject.Destroy(go);
		}
		
		AnimalInstances.Clear ();
	}

	void SpawnAnimal(int i)
	{
		var canvas = GetComponentInChildren<Canvas> ();

		int indexi = 0;
		bool goodToGo = false;

		while (!goodToGo)
		{
			indexi = UnityEngine.Random.Range(0, Animals.Count);

			if (Animals[indexi].GetComponent<SplatSpawner>().introDifficulty <= GameRound && Animals[indexi].GetComponent<SplatSpawner>().exitDifficulty >= GameRound)
			{
				goodToGo = true;
			}

		}

		var animal = GameObject.Instantiate(Animals[indexi]) as GameObject;


		animal.transform.parent = canvas.transform;
		animal.GetComponent<RectTransform>().localPosition = new Vector3(i*130-200,-300,0);
		animal.GetComponent<RectTransform>().localScale = Vector3.one;

		AnimalInstances.Add(animal);
	}

	void ShowPrompt(string text)
	{
		StopAllCoroutines ();
		PromptText.text = "";
		StartCoroutine (ShowPromptAsync (text));
	}

	IEnumerator ShowPromptAsync(string text)
	{
		Debug.Log ("Done");
		float time = Time.realtimeSinceStartup;
		PromptText.text = text;

		float fadeIn = 0.25f;
		float fadeOut = 3;
		while (true) {
			float t = Time.realtimeSinceStartup - time;
			if (t < (fadeIn))
				PromptText.color = Color.Lerp (Color.clear, Color.white, t/fadeIn);
			else if (t < (fadeIn+fadeOut))
				PromptText.color = Color.Lerp(Color.clear, Color.white, ((fadeIn+fadeOut)-t)/fadeOut);
			else
				 break;
			yield return null;
		}
		PromptText.text = "";
	}

	void ShowScore(float score)
	{
		int year = DateTime.Now.Year;
		int armageddon = (year + Mathf.FloorToInt (score));
	//	ScoreText.text = "World ends in " + armageddon;
	}

	// Update is called once per frame
	void Update ()
	{
		if (Application.isEditor)
		{
			if (Input.GetKeyUp (KeyCode.Space))
			{
				Application.LoadLevel (Application.loadedLevelName);
				return;
			}

			if (Input.GetKeyUp (KeyCode.C)) {
				CheckScore ();
			}
			if (Input.GetKeyUp (KeyCode.E)) {
				TurnStartTime = Time.realtimeSinceStartup - TurnTime;
			}
			if (Input.GetKeyUp (KeyCode.L)) {
				LoseLife ();
			}
		}

		if (TurnActive) {
			float timeLeft = (TurnStartTime + TurnTime) - Time.realtimeSinceStartup;
			if (timeLeft < 0)
			{
				timeLeft = 0;
				TurnActive = false;
				CheckScore ();

			}
			TimeText.text = string.Format("00:{0:D2}", (int)timeLeft);
		}
		else
		{
			TimeText.text = "";
		}

	}

	void CheckScore()
	{
		int totalSamples = 0;
		var symbol = GameObject.FindObjectOfType<Symbol> ();
		BloodDrop[] bloodDropObjects = GameObject.FindObjectsOfType<BloodDrop> ();
		Vector2[] bloodDrops = bloodDropObjects.Select(x => new Vector2(x.transform.position.x, x.transform.position.y)).ToArray();

		List<Vector2> checkPoints = new List<Vector2> ();
		List<bool> CheckPointStatus = new List<bool> ();
		for (int i = 0; i < symbol.Polygons.Count; i++)
		{
			var poly = symbol.Polygons[i];
			for (int j= 0; j < poly.Points.Count-1; j++)
			{
				Vector2 a = (Vector2)poly.Points[j].transform.position;
				Vector2 b = (Vector2)poly.Points[j+1].transform.position;
			
				int samplesPerSegment =  Mathf.Max (2, (int)((b - a).magnitude*4.0f));
			
				for (int k = 0; k < samplesPerSegment; k++) 
				{
					Vector2 point = Vector2.Lerp (a, b, (float)k / samplesPerSegment);
					checkPoints.Add (point);
					Debug.DrawLine (new Vector3(point.x,point.y,0), new Vector3(point.x+0.1f, point.y, 0));
				}
			}
		}
		Debug.Log ("Checking " + bloodDrops.Length + " drops vs " + checkPoints.Count + " checkpoints");
        
        const float THRESHOLD = 0.20f;
		int checkPointsFilled = 0;
		bool found = false;
		for (int j = 0; j < checkPoints.Count; j++)
		{
			found = false;
			for (int di = 0; di < bloodDrops.Length; di++)
			{
				if (Vector2.SqrMagnitude(bloodDrops[di] - checkPoints[j]) < (THRESHOLD*THRESHOLD))
				{
					found = true;
					break;
				}
			}
			CheckPointStatus.Add (found);
			if (found)checkPointsFilled++;
		}
		StopAllCoroutines ();
		float percentage = (checkPointsFilled / (float)checkPoints.Count * 100.0f);
        
        StartCoroutine (ShowResult (checkPoints, CheckPointStatus));
		float totalScore = percentage / 100.0f + (4 - AnimalsUsed) * 0.1f;

		Score += percentage / 100.0f;

		ShowScore (Score);
		ScorePanel.ShowScore (percentage/100.0f, 0.1f, (4 - AnimalsUsed) * 0.05f, (int)(totalScore*365.0f));

	}

	public void LoseLife()
	{
		LivesLeft--;
		for (int i = 0; i < 3; i++) 
		{
			LifeHorns[i].SetActive(i < LivesLeft);
		
		}
		if (LivesLeft == 0) 
		{
			ShowGameOver();
//			StartCoroutine(ShowGameOver());
		}
	}
	private void ShowGameOver()
	{
		GameManager.instance.EndGame (Score);

	}
	private IEnumerator ShowResult(List<Vector2> pts, List<bool> status)
	{
			List<GameObject> created = new List<GameObject> ();
		for (int i = 0; i < pts.Count; i++) {
			var go = GameObject.Instantiate(CheckMarkPrefab) as GameObject;
			go.transform.parent = transform;
			go.transform.position = new Vector3(pts[i].x, pts[i].y, 1);
			if (status[i])
			{
				go.GetComponent<SpriteRenderer>().color = Color.yellow;
			}
			else
			{
				go.GetComponent<SpriteRenderer>().color = Color.blue;
			}
			created.Add (go);
			if (i%2==0)
				yield return null;
		}
        
        yield return new WaitForSeconds (2);
		for (int i = 0; i < created.Count; i++) {
			GameObject.Destroy(created[i]);
			if (i%2==0)
				yield return null;
        }
		PromptText.text = "";
		StartNewTurn ();
    }
}
