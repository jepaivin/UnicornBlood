using UnityEngine;
using System.Collections.Generic;
using System.Linq;
public class PaintController : MonoBehaviour 
{
	public float CaptureInnerRadius = 20;
	public float CaptureOuterRadius = 40;
	public float DragSpeed = 1;
	public bool TestMode;
	private  List<GameObject>  BloodDrops;
	private List<GameObject> CurrentDragDrops;
	private List<float> CurrentDragWeights;

	// Use this for initialization
	void Start () 
	{
	
	}
	
    void FindBloodDrops()
    {
		BloodDrops = GameObject.FindGameObjectsWithTag ("BloodDrop").ToList ();
    }

	Vector3 previousMousePosition;

	// Update is called once per frame
	void Update () 
    {
		float mouseX = Input.mousePosition.x;
		float mouseY = Input.mousePosition.y;
		var mousePosition = Camera.main.ScreenToWorldPoint (Input.mousePosition);

		if (Input.GetMouseButtonDown (0))
		{
			FindBloodDrops();
			CurrentDragDrops = new List<GameObject>();
			CurrentDragWeights = new List<float>();

			foreach(var obj in BloodDrops)
			{
				var d = (obj.transform.position  - mousePosition).magnitude;
				Debug.Log ("Compare " + obj.transform.position   + " to " + mousePosition);
				float dragWeight = 0;

				if (d < CaptureInnerRadius)
				{
					dragWeight = 1;
				}
				else if (d < CaptureOuterRadius)
				{
					dragWeight = (d - CaptureInnerRadius) / (CaptureOuterRadius - CaptureInnerRadius);
				}

				if (dragWeight > 0)
				{
					CurrentDragDrops.Add (obj);
					CurrentDragWeights.Add (dragWeight);
				}
			}
			previousMousePosition = mousePosition;
			Debug.Log ("Drag drop count " +CurrentDragDrops.Count);
		}

		if (Input.GetMouseButtonUp (0))
		{
			CurrentDragDrops.Clear ();
			CurrentDragWeights.Clear ();
		}

		if (Input.GetMouseButton (0))
		{
			Vector3 scroll = mousePosition - previousMousePosition;
			Debug.Log ("SD " + scroll);
			for(int i = 0; i < CurrentDragDrops.Count; i++)
			{
				var drop = CurrentDragDrops[i];
				Vector3 newPosition = scroll * CurrentDragWeights[i] * DragSpeed;
				drop.transform.position =  drop.transform.position + scroll;
			}
		}

	}
}
