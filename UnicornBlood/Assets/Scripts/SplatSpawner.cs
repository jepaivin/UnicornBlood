using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;


public class SplatSpawner : MonoBehaviour//, IDragHandler
{
	public int BloodAmount;
	public GameObject SplatPrefab;

	private GameObject CurrentSplat;
	private Vector3 lastPosition;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void OnBeginDrag()
	{
		Debug.Log ("Begin");
		GameObject.FindObjectOfType<PaintController> ().PlacingSplat = true;
		CurrentSplat = GameObject.Instantiate (SplatPrefab) as GameObject;
		CurrentSplat.transform.parent = GameObject.Find ("BloodContainer").transform;
		CurrentSplat.transform.position = transform.position;
		lastPosition = Input.mousePosition;
	}

	public void OnEndDrag()
	{
	//	GameObject.Destroy (CurrentSplat);
		GameObject.FindObjectOfType<PaintController> ().PlacingSplat = false;

		CurrentSplat = null;
	}

	public void OnDrag()
	{
		var delta = (Input.mousePosition - lastPosition)*0.02f;
		CurrentSplat.transform.position = new Vector3 (CurrentSplat.transform.position.x + delta.x,
		                                              CurrentSplat.transform.position.y + delta.y,
		                                              CurrentSplat.transform.position.z);
		lastPosition = Input.mousePosition;
	}

	public void Spawn()
	{
		for (int i = 0; i < BloodAmount; i++)
		{
			
		}
	}
}
