using UnityEngine;
using System.Collections;

public class BloodDrop : MonoBehaviour {

	public static float MaxMove = 2;
	public float MoveLeft;
	public float MoveMultiplier = 1;
	public bool TrailMode = false;

	private SpriteRenderer spriteRenderer;
	private Color startColor;
	private Color endColor;

	// Use this for initialization
	void Start () 
	{
		MoveLeft = MaxMove * MoveMultiplier;
		spriteRenderer = GetComponent<SpriteRenderer> ();
		startColor = spriteRenderer.color;
		endColor = Color.Lerp (spriteRenderer.color, Color.black, 0.25f * MoveMultiplier);
	}
	
	// Update is called once per frame
	void Update () 
	{
	}

	public void Move(Vector3 dir, float mag)
	{
		float dist = Mathf.Min (mag * MoveMultiplier, MoveLeft);

		if (dist < MoveLeft) 
		{
			transform.position = transform.position + dir * dist;
		}
		MoveLeft -= dist;

		spriteRenderer.color = Color.Lerp (startColor, endColor, (MaxMove-MoveLeft / MaxMove));
	}
}
