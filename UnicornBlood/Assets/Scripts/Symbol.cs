using UnityEngine;
using System.Collections.Generic;

[ExecuteInEditMode]
public class Symbol : MonoBehaviour 
{
	public List<GameObject> Points;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (!Application.isPlaying) {
		
			for (int i = 0; i < Points.Count; i++)
			{
				int j = (i+1)%Points.Count;
				Debug.DrawLine(Points[i].transform.position, Points[j].transform.position);
			}
		}
	}


}
