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
	public Text ScoreText;
	public Text TimeText;


	private float TurnTime = 15;
	private float TurnStartTime;
	public bool TurnActive = false;

	public GameObject CheckMarkPrefab;
	private List<GameObject> AnimalInstances = new List<GameObject>();
	private GameObject CurrentSymbol;
	private float Score = 0.0f;


	// Use this for initialization
	
	void Awake()
	{

	}

	public void StartGame () 
	{
		ShowPrompt("GAME STARTING");
		Score = 0.0f;
		ShowScore (Score);

		StartNewTurn ();
	}

	void StartNewTurn()
	{
		StartCoroutine (StartNewTurnAsync ());
	}

	IEnumerator StartNewTurnAsync()
	{
		yield return null;	
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

		int index = UnityEngine.Random.Range(0, Animals.Count);

		var animal = GameObject.Instantiate(Animals[index]) as GameObject;
		animal.transform.parent = canvas.transform;
		animal.GetComponent<RectTransform>().localPosition = new Vector3(i*130-200,-300,0);
		animal.GetComponent<RectTransform>().localScale = Vector3.one;

		AnimalInstances.Add(animal);
	}

	void ShowPrompt(string text)
	{
		StopAllCoroutines ();
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
		ScoreText.text = "World ends in " + armageddon;
	}

	// Update is called once per frame
	void Update ()
	{
		
		if (Input.GetKeyUp (KeyCode.Space)) 
		{
			Application.LoadLevel(Application.loadedLevelName);
			return;
		}
		if (Input.GetKeyUp (KeyCode.C)) 
		{
			CheckScore();
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
		Debug.Log ("Checking " + bloodDrops.Length + " drops vs " + symbol.Points.Count + " segments");

		List<Vector2> checkPoints = new List<Vector2> ();
		List<bool> CheckPointStatus = new List<bool> ();
		for (int i = 0; i < symbol.Points.Count; i++) {
			Vector2 a = (Vector2)symbol.Points [i].transform.position;
			Vector2 b = (Vector2)symbol.Points [(i + 1) % symbol.Points.Count].transform.position;
		
			int samplesPerSegment =  Mathf.Max (3, (int)((b - a).magnitude / 10.0f));

		
			for (int j = 0; j < samplesPerSegment; j++) 
			{
				Vector2 point = Vector2.Lerp (a, b, (float)j / samplesPerSegment);
				checkPoints.Add (point);
				Debug.DrawLine (new Vector3(point.x,point.y,0), new Vector3(point.x+0.1f, point.y, 0));

			}
		}
		float THRESHOLD = 0.15f;
		int checkPointsFilled = 0;
		bool found = false;
		for (int j = 0; j < checkPoints.Count; j++)
		{
			found = false;
			for (int i = 0; i < bloodDrops.Length; i++)
			{
				if (Vector2.SqrMagnitude(bloodDrops[i] - checkPoints[j]) < THRESHOLD)
				{
//					bloodDropObjects[i].GetComponent<SpriteRenderer>().color = Color.white;
//					bloodDropObjects[i].GetComponent<SpriteRenderer>().sortingOrder = 4000;
					found = true;
					break;
				}
			}
			CheckPointStatus.Add (found);
			if (found)checkPointsFilled++;
		}
		StopAllCoroutines ();
		float percentage = (checkPointsFilled / (float)checkPoints.Count * 100.0f);
		ShowPrompt ((int)percentage + " % COMPLETE");
        
        StartCoroutine (ShowResult (checkPoints, CheckPointStatus));
		Score += percentage / 100.0f;
		ShowScore (Score);

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
        
		StartNewTurn ();
    }
}
