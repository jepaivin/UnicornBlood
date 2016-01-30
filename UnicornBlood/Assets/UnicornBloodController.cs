using UnityEngine;
using System.Collections;

public class UnicornBloodController : MonoBehaviour {
	float phase = 0;
	SpriteRenderer spriteRenderer = null;
	// Use this for initialization
	void Start () {
		phase = 0;//transform.position.x + transform.position.y;
		spriteRenderer = GetComponent<SpriteRenderer> ();
	}

	Color GetRainbow(float f)
	{

		if (f < 0.25f) 
			return Color.Lerp (Color.red, Color.yellow, (f-0)*4);
		
		if (f < 0.5f)
			return Color.Lerp (Color.yellow, Color.green, (f-0.25f)*4);
		if (f < 0.75f)
			return Color.Lerp (Color.green, Color.blue, (f-0.5f)*4);
		if (f < 1.0)
			return Color.Lerp (Color.blue, Color.red, (f-0.75f)*4);
		return Color.black;

	}
	// Update is called once per frame
	void Update ()
	{
		var pos = transform.position;
		phase += Time.deltaTime;
		/*
		float r = phase - (pos.x + pos.y)*5.777f;
		float g = phase*0.7f + pos.x*5.111f+0.177f;
		float b = phase*1.11f + pos.y*2 * 1.333f;*/
		float ph = phase + pos.x - pos.y;
		float m = ph % 1.0f;
		spriteRenderer.color = GetRainbow (m);
	}
}
