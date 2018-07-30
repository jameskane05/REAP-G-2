using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class LandmarkPresentationController : MonoBehaviour {

	[SerializeField] private Sprite[] IntroImages;
	[SerializeField] private int SlideDuration;
	[SerializeField] private int OccursThisManyTimes;
	[SerializeField] private GameObject StudyTestOptionsPanel;
	[SerializeField] private Text text;
	private Image img;
	private bool hasStarted = false;
	public ExperimentSettings _expInstance;

	void Start () {
		img = GameObject.Find("Canvas").GetComponent<Image>();
		_expInstance = ExperimentSettings.GetInstance ();
	}

	void Update() {
		if (Input.GetKeyDown ("space") && !hasStarted) {
			hasStarted = true;
			StartCoroutine (CycleLandmarks ());
		}
	}

	public void LandmarkPresentation () {		
		text.gameObject.SetActive (true);
		StudyTestOptionsPanel.SetActive (false);
		img.color = UnityEngine.Color.black;
	}

	IEnumerator CycleLandmarks()
	{
		
		int[] randomOrder = RandomizeOrder ();
		for (int n = 0; n < OccursThisManyTimes; n++) {
			text.gameObject.SetActive (false);
			int picCounter = 0;
			while (picCounter < randomOrder.Length )
			{
				img.color = UnityEngine.Color.white;
				img.sprite = IntroImages[randomOrder[picCounter] - 1];
				yield return new WaitForSeconds(SlideDuration);
				picCounter++;
			}
			text.gameObject.SetActive (true);
			img.sprite = null;
			img.color = UnityEngine.Color.black;
			if (n < OccursThisManyTimes - 1)
				text.text = "Please pay attention to the names of the objects again.";
			else
				text.text = "";
			yield return new WaitForSeconds(SlideDuration);
		}
		text.gameObject.SetActive (false);
		img.color = UnityEngine.Color.clear;
		StudyTestOptionsPanel.SetActive (true);
		LandmarkPresentationComplete ();
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

	public void LandmarkPresentationComplete () {
        List<string> experimentHeader = GetExperimentHeader();

        if (!File.Exists(_expInstance.FileName + "_path.txt"))
        {
            System.IO.File.WriteAllLines(_expInstance.FileName + "_path.txt", experimentHeader.ToArray());
            System.IO.File.WriteAllLines(_expInstance.FileName + "_path_data.txt", experimentHeader.ToArray());
        }

        System.IO.File.AppendAllText (_expInstance.FileName + "_path.txt", "Landmark Presentation completed.\r\n");
        System.IO.File.AppendAllText (_expInstance.FileName + "_path_data.txt", "Landmark Presentation completed.\r\n");
    }

    static private List<string> GetExperimentHeader()
    {
        List<string> experimentHeader = new List<string>();

        ExperimentSettings _expInstance = ExperimentSettings.GetInstance();
        experimentHeader.Add("Participant ID: " + _expInstance.ParticipantID);
        experimentHeader.Add("Experimenter Initials: " + _expInstance.ExperimenterInitials);
        experimentHeader.Add("Date: " + _expInstance.Date);
        experimentHeader.Add("\r\n");
        return experimentHeader;
    }
}
