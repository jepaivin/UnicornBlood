using UnityEngine;
using System.Collections.Generic;
using System.Linq;
public class PaintController : MonoBehaviour 
{
	public float CaptureInnerRadius = 20;
	public float CaptureOuterRadius = 40;
	public float DragSpeed = 1;
	public bool TestMode;
	public bool ContinuousCapture = true;
	public bool PlacingSplat = false;

	private List<BloodDrop> CurrentDragDrops = new  List<BloodDrop> ();
	private List<float> CurrentDragWeights = new List<float>();
	private List<float> DragStartTime = new List<float> ();


	// Use this for initialization
	void Start () 
	{
#if UNITY_IPHONE
		DragSpeed = 0.5f;
#endif
	
	}
	
    List<BloodDrop> FindBloodDrops()
    {
		return GameObject.FindGameObjectsWithTag ("BloodDrop").Select (x => x.GetComponent<BloodDrop>()).ToList ();
    }

	Vector3 previousMousePosition;

	void StartDragging(Vector3 mousePosition)
	{

		var drops = FindBloodDrops();
		
		CurrentDragDrops = new List<BloodDrop>();
		CurrentDragWeights = new List<float>();
		
		foreach(var obj in drops)
		{
			Vector2 a = obj.transform.position;
			Vector2 b = mousePosition;
			var d = (a - b).magnitude;
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
	}
	// Update is called once per frame
	void Update () 
    {

		if (Input.GetKeyUp (KeyCode.Space)) 
		{
			Application.LoadLevel(Application.loadedLevelName);
			return;
		}
		float mouseX = Input.mousePosition.x;
		float mouseY = Input.mousePosition.y;
		var mousePosition = Camera.main.ScreenToWorldPoint (Input.mousePosition);

		if (Input.GetMouseButtonDown (0))
		{
			StartDragging(mousePosition);
		}

		if (Input.GetMouseButtonUp (0))
		{
			CurrentDragDrops.Clear ();
			CurrentDragWeights.Clear ();
		}

		if (Input.GetMouseButton (0) && CurrentDragDrops != null)
		{
			Vector3 scroll = (mousePosition - previousMousePosition)*DragSpeed;
			float scrollMagnitude = scroll.magnitude;
			Vector3 scrollDirection = scroll.normalized;
			for(int i = 0; i < CurrentDragDrops.Count; i++)
			{
				var drop = CurrentDragDrops[i];
				drop.Move(scrollDirection, scrollMagnitude * CurrentDragWeights[i]);
			}
			previousMousePosition = mousePosition;
			if (ContinuousCapture && !PlacingSplat)
			{
				StartDragging(mousePosition);
			}
		}

	}
}
