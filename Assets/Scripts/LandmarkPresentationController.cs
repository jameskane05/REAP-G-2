using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class LandmarkPresentationController : MonoBehaviour {

	[SerializeField] private Sprite[] IntroImages;
	[SerializeField] private int SlideDuration;
	[SerializeField] private int OccursThisManyTimes;
	[SerializeField] private GameObject StudyTestOptionsPanel;
	[SerializeField] private Text text;
	private Image img;
	private bool hasStarted = false;

	void Start () {
		img = GameObject.Find("Canvas").GetComponent<Image>();
	}

	void Update() {
		if (Input.GetKeyDown ("space") && !hasStarted) {
			StartCoroutine (CycleLandmarks ());
			hasStarted = true;
		}
	}

	public void LandmarkPresentation () {		
		text.gameObject.SetActive (true);
		StudyTestOptionsPanel.SetActive (false);
		img.color = UnityEngine.Color.black;
	}

	IEnumerator CycleLandmarks()
	{
		text.gameObject.SetActive (false);
		int[] randomOrder = RandomizeOrder ();
		for (int n = 0; n < OccursThisManyTimes; n++) {
			int picCounter = 0;
			while (picCounter < randomOrder.Length )
			{
				img.color = UnityEngine.Color.white;
				img.sprite = IntroImages[randomOrder[picCounter] - 1];
				yield return new WaitForSeconds(SlideDuration);
				picCounter++;
			}
		}
		img.color = UnityEngine.Color.clear;
		StudyTestOptionsPanel.SetActive (true);
	}

	public int[] RandomizeOrder(){

		int[] deck = new int[IntroImages.Length];
		for(int i = 0; i < IntroImages.Length; i++)
		{
			deck[i] = 1 + i;
		}

		for(int i = 0; i < IntroImages.Length; i++)
		{
			// Pick an entry no later in the deck, or i itself.
			int j = Random.Range(0, i + 1);

			// Swap the order of the two entries.
			int swap = deck[i];
			deck[i] = deck[j];
			deck[j] = swap;
		}
		return deck;
	}
}
