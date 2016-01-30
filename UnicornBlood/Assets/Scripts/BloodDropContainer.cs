using UnityEngine;
using System.Collections;

public class BloodDropContainer : MonoBehaviour
{

	public int Copies = 10;
	public bool Rotating = false;
	private float rotation = 0;
	// Use this for initializationsa

	void Start () 
	{
		var drops = GetComponentsInChildren<BloodDrop> ();
		for (int i = 0; i < Copies; i++) 
		{
			float multiplier = (float)(Copies-i-1)/(Copies);
			for (int j = 0; j< drops.Length; j++)
			{
				var copy = GameObject.Instantiate(drops[j].gameObject) as GameObject;
				copy.transform.parent = drops[j].gameObject.transform.parent;
				copy.transform.position = drops[j].gameObject.transform.position;

				var drop =  copy.GetComponent<BloodDrop>();
				drop.MoveMultiplier = multiplier;
			}
		}
	
	}

	// Update is called once per frame
	void Update () 
	{
	if (Rotating) 
		{
			rotation += Time.deltaTime;
			transform.rotation = Quaternion.Euler(0,0, rotation*100.0f);
		}
	}
}
