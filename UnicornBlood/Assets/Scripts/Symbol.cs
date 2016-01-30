using UnityEngine;
using System.Collections.Generic;

[ExecuteInEditMode]

public class Symbol : MonoBehaviour 
{
	[System.Serializable]
	public struct PointList
	{
		public List<GameObject> Points;
	}	
	public List<PointList> Polygons;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (!Application.isPlaying && Polygons != null) {
		
			for (int i = 0; i < Polygons.Count; i++)
			{
				var poly = Polygons[i];
				for (int j= 0; j < poly.Points.Count-1; j++)
				{
					Debug.DrawLine(poly.Points[j].transform.position, poly.Points[j+1].transform.position);
				}
			}
		}
	}


}
