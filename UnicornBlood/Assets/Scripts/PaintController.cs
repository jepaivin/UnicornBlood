using UnityEngine;
using System.Collections.Generic;
using System.Linq;
public class PaintController : MonoBehaviour 
{
	public float CaptureInnerRadius = 20;
	public float CaptureOuterRadius = 40;
	public float DragSpeed = 1;
	public bool TestMode;
	private  List<GameObject>  BloodDrops = new List<GameObject> ();
	private List<GameObject> CurrentDragDrops = new  List<GameObject> ();
	private List<float> CurrentDragWeights = new List<float>();
	private List<float> DragStartTime = new List<float> ();


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
			FindBloodDrops();
			CurrentDragDrops = new List<GameObject>();
			CurrentDragWeights = new List<float>();

			foreach(var obj in BloodDrops)
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
				Debug.Log ("Compare " + obj.transform.position   + " to " + mousePosition + " D="+d + " W="+dragWeight);

			}
			previousMousePosition = mousePosition;
			Debug.Log ("Drag drop count " +CurrentDragDrops.Count);
		}

		if (Input.GetMouseButtonUp (0))
		{
			CurrentDragDrops.Clear ();
			CurrentDragWeights.Clear ();
		}

		if (Input.GetMouseButton (0) && CurrentDragDrops != null)
		{
			Vector3 scroll = mousePosition - previousMousePosition;
			for(int i = 0; i < CurrentDragDrops.Count; i++)
			{
				var drop = CurrentDragDrops[i];
				Vector3 newPosition = scroll * CurrentDragWeights[i] * DragSpeed;
				drop.transform.position =  drop.transform.position + newPosition;
			}
			previousMousePosition = mousePosition;
		}

	}
}
