using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;


public class SplatSpawner : MonoBehaviour//, IDragHandler
{
	public int BloodAmount;
	public GameObject SplatPrefab;

	private GameObject CurrentSplat;
	private Vector3 lastPosition;
	private bool used = false;
	private bool faded = false;
	private Vector3 startPosition;
	private Vector3 initialRotation;
	private float dragStartTime;

	// Use this for initialization
	void Start () {
		GetComponent<UnityEngine.UI.Image>().color = new Color(1,1,1,1);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	IEnumerator FadeOut()
	{
		float time = Time.realtimeSinceStartup;
		while (true) 
		{
			float a = (Time.realtimeSinceStartup - time)*2;
			if (a > 1)
			{
				GetComponent<UnityEngine.UI.Image>().color = Color.clear;
				yield break;
			}
			GetComponent<UnityEngine.UI.Image>().color = new Color(1,1,1,1-a);
			yield return null;
		}
	}

	public void OnBeginDrag()
	{
		if (used)
			return;
		var gameController = FindObjectOfType<GameController> ();

		if (!gameController.TurnActive)
			return;
		gameController.AnimalsUsed++;
		used = true;
		faded = false;
		GameObject.FindObjectOfType<PaintController> ().PlacingSplat = true;
		CurrentSplat = GameObject.Instantiate (SplatPrefab) as GameObject;
		CurrentSplat.GetComponent<BloodDropContainer> ().Rotating = true;
		initialRotation = CurrentSplat.transform.rotation.eulerAngles;
		dragStartTime = Time.realtimeSinceStartup;
		CurrentSplat.transform.parent = GameObject.Find ("BloodContainer").transform;
		CurrentSplat.transform.position = transform.position;
		lastPosition = Input.mousePosition;
		startPosition = Input.mousePosition;

	}

	public void OnEndDrag()
	{
	//	GameObject.Destroy (CurrentSplat);
		GameObject.FindObjectOfType<PaintController> ().PlacingSplat = false;
		if (CurrentSplat != null) {
			CurrentSplat.GetComponent<BloodDropContainer> ().Rotating = false;

			CurrentSplat = null;
		}
	}

	public void OnDrag()
	{
		float mult = 0.02f;
		if (!Application.isEditor)
			mult = 0.005f;


		var delta = (Input.mousePosition - lastPosition) * mult;
		if (CurrentSplat == null)
			return;
		CurrentSplat.transform.position = new Vector3 (CurrentSplat.transform.position.x + delta.x,
		                                              CurrentSplat.transform.position.y + delta.y,
		                                              CurrentSplat.transform.position.z);
		lastPosition = Input.mousePosition;

		if (!faded && (Input.mousePosition - startPosition).magnitude > 0.2f)
		{
			faded = true;
			StartCoroutine(FadeOut());
		}
	}
}
