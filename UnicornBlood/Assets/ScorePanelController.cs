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
	public Image Background;

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
		Background.color = Color.clear;

		gameObject.SetActive (true);
		StartCoroutine(ShowScoreAsync(completion, accuracy, economy, pushBack));

	}

	IEnumerator ShowScoreAsync(float completion, float accuracy, float economy, int pushBack)
	{
		yield return new WaitForSeconds (2.0f);
		Background.color = Color.black;
		Completion.text = "Completion: " + (int)(completion * 100) + " %";
		yield return new WaitForSeconds (1.0f);
		/*Accuracy.text = "Accuracy: " + (int)(accuracy * 100) + " %";
		yield return new WaitForSeconds (1.0f);*/
		if (economy > 0) {
			Economy.text = "Economy Bonus: " + (int)(economy * 100) + " %";
		}
		yield return new WaitForSeconds (1.0f);

		if (completion > 0.9f) {
			Gods.text = "Gods are ecstatic";
		} else
		if (completion > 0.8f) {
			Gods.text = "Gods are happy";
		} else
		if (completion > 0.7f) {
			Gods.text = "Gods are pleased";
		} else
		if (completion > 0.6f) {
			Gods.text = "Gods are satisfied";
		} else
		if (completion > 0.5f) {
			Gods.text = "Gods are contended";
		} else
		if (completion > 0.4f) {
			Gods.text = "Gods are displeased - Life lost!";
		} else
		if (completion > 0.3f) {
			Gods.text = "Gods are annoyed - Life lost!";
		} else
		if (completion > 0.2f) {
			Gods.text = "Gods are angry - Life lost!";
		} else
		if (completion > 0.1f) {
			Gods.text = "Gods are enraged - Life lost!";
		} else
		{
			Gods.text = "Gods are wrathful - Life lost!";
		}
		yield return new WaitForSeconds (1.0f);

		Push.text = "Armageddon pushed back " + pushBack + " days";

		yield return new WaitForSeconds (1.0f);
		gameObject.SetActive (false);
	}
}
