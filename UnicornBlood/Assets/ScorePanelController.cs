using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ScorePanelController : MonoBehaviour 
{

	public Text Completion;
	public Text Accuracy;

	public Text Economy;

	public Text Gods;

	public Text Push;

	void Clear()
	{
		Completion.text = "";
		Accuracy.text = "";
		Economy.text = "";
		Gods.text = "";
		Push.text = "";
	}

	public void ShowScore(float completion, float accuracy, float economy, int pushBack)
	{
		Clear ();
		gameObject.SetActive (true);
		StartCoroutine(ShowScoreAsync(completion, accuracy, economy, pushBack));

	}

	IEnumerator ShowScoreAsync(float completion, float accuracy, float economy, int pushBack)
	{
		Completion.text = "Completion: " + (int)(completion * 100) + " %";
		yield return new WaitForSeconds (1.0f);
		Accuracy.text = "Accuracy: " + (int)(accuracy * 100) + " %";
		yield return new WaitForSeconds (1.0f);
		if (economy > 0) {
			Economy.text = "Economy Bonus: " + (int)(economy * 100) + " %";
		}
		yield return new WaitForSeconds (1.0f);

		if (completion > 0.5f) {
			Gods.text = "Gods are pleased";
		} else
		{
			Gods.text = "Gods are indifferent";
		}
		yield return new WaitForSeconds (1.0f);

		Push.text = "Armageddon pushed back 2 days";
		yield return new WaitForSeconds (1.0f);
		gameObject.SetActive (false);
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
