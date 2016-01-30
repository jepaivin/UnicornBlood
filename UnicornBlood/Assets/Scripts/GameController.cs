using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Collections;
using System.Linq;

public class GameController : MonoBehaviour 
{
	public List<GameObject> Symbols;
	public List<GameObject> Animals;
	public Text PromptText;
	// Use this for initialization
	
	void Awake()
	{
	}

	public void StartGame () 
	{
	
		ShowPrompt("GAME STARTING");
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

	}

	void CheckScore()
	{
		int samplesPerSegment = 20;
		int totalSamples = 0;
		var symbol = GameObject.FindObjectOfType<Symbol> ();
		BloodDrop[] bloodDropObjects = GameObject.FindObjectsOfType<BloodDrop> ();
		Vector2[] bloodDrops = bloodDropObjects.Select(x => new Vector2(x.transform.position.x, x.transform.position.y)).ToArray();
		Debug.Log ("Checking " + bloodDrops.Length + " drops vs " + symbol.Points.Count + " segments");
		List<Vector2> checkPoints = new List<Vector2> ();

		for (int i = 0; i < symbol.Points.Count; i++) {
			Vector2 a = (Vector2)symbol.Points [i].transform.position;
			Vector2 b = (Vector2)symbol.Points [(i + 1) % symbol.Points.Count].transform.position;
			for (int j = 0; j < samplesPerSegment; j++) {
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
					bloodDropObjects[i].GetComponent<SpriteRenderer>().color = Color.white;
					bloodDropObjects[i].GetComponent<SpriteRenderer>().sortingOrder = 4000;
					found = true;
					break;
				}
			}
			if (found)
				checkPointsFilled++;

		}
		float percentage = (checkPointsFilled / (float)checkPoints.Count * 100.0f);
		ShowPrompt (percentage + " % COMPLETE");
		Debug.Log (percentage);
	}
}
