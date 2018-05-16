using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ExperimentController : MonoBehaviour {

    [SerializeField] private InputField ParticipantIDInput;
	[SerializeField] private InputField ExperimenterInitialsInput;
	[SerializeField] private GameObject FirstPanel;
	[SerializeField] private GameObject MainOptionsPanel;
	public int StudyVideoMatchingTrials;

	public static ExperimentSettings _expInstance;

    private void Start()
    {
		_expInstance = ExperimentSettings.GetInstance ();
		_expInstance.MazeSettings = new MazeSettings ();

		if (!string.IsNullOrEmpty(_expInstance.ExperimenterInitials))  // if this already exists then we're coming out of a maze
			OpenSubmenu ();
    }

    private void Update() {
        if (Input.GetKey(KeyCode.Escape)) 
			Application.Quit();
        if (Input.GetKey(KeyCode.E) && FirstPanel.activeSelf == false)
			SceneManager.LoadScene(0);
    }

	public void EnterExperimentInfo() {
		_expInstance.ParticipantID = ParticipantIDInput.text;
		_expInstance.ExperimenterInitials = ExperimenterInitialsInput.text;
		_expInstance.Date = DateTime.Now;
		_expInstance.VideoMatchingOrder = RandomizeVideoMatchingOrder ();
		SetDir (_expInstance);
		OpenSubmenu();
    }

	void OpenSubmenu() {
		// Cursor is disabled coming out of maze scenes
		Cursor.visible = true;
		Cursor.lockState = CursorLockMode.None;
        
		FirstPanel.SetActive (false);
		MainOptionsPanel.SetActive(true);
	}

	public void SetDir(ExperimentSettings _expInstance)
    {
		string currentDir = System.IO.Directory.GetCurrentDirectory ();
        string newDir = _expInstance.ParticipantID + "_SCM";
		_expInstance.TestTrialIndex = UnityEngine.Random.Range (0, 23);
		_expInstance.MatchingTrialIndex = UnityEngine.Random.Range (0, 8);
		_expInstance.MatchingAnswers = new string[9];
        System.IO.Directory.CreateDirectory(_expInstance.FileDir + "_" + DateTime.Now.ToString("ddHHmm"));
		_expInstance.FileName = _expInstance.FileDir + "\\" + newDir + "_data.txt";
    }
		
	public void LoadMazeJoystickPractice () {
		_expInstance.MazeSettings.MazeName = MazeNameEnum.Practice;
		SceneManager.LoadScene(1);
	}

	public void LoadVisuomotorExpertise () {
		_expInstance.MazeSettings.MazeName = MazeNameEnum.Visuomotor;
		SceneManager.LoadScene(2);
	}
		
	public void LoadStudyPhaseVideo() {
		_expInstance.Phase = PhaseEnum.StudyVideo;
		SceneManager.LoadScene(3);
	}

	public void LoadStudyPhaseMatching() {
		_expInstance.Phase = PhaseEnum.StudyMatching;
		SceneManager.LoadScene(4);
	}

	public void LoadStudyPhaseVideoMatching () {
		_expInstance.Phase = PhaseEnum.StudyVideoMatching;
		SceneManager.LoadScene (_expInstance.VideoMatchingOrder[0].ToString());
	}

	public void LoadTestTrials() {
		_expInstance.Phase = PhaseEnum.TestTrials;
		SceneManager.LoadScene (5);
	}

	public VideoMatchingPhaseEnum[] RandomizeVideoMatchingOrder(){
		int seedCtr = 0;
		int seedOpCtr = 0;
		int i = 0;
		Array values = Enum.GetValues(typeof(VideoMatchingPhaseEnum));
		VideoMatchingPhaseEnum[] deck = new VideoMatchingPhaseEnum[StudyVideoMatchingTrials];
		System.Random random = new System.Random ();
		VideoMatchingPhaseEnum randomMazeSeed = (VideoMatchingPhaseEnum)values.GetValue (random.Next (values.Length));
		VideoMatchingPhaseEnum seedOpposite;
		if (randomMazeSeed == VideoMatchingPhaseEnum.Learning) {
			seedOpposite = VideoMatchingPhaseEnum.Matching;
		}
		else seedOpposite = VideoMatchingPhaseEnum.Learning;
		deck [i] = randomMazeSeed;
		seedCtr++;
		i++;

		int randomSkip1 = UnityEngine.Random.Range (2, 3); 
		int randomSkip2 = UnityEngine.Random.Range (5, 6);
		int randomSkip3 = UnityEngine.Random.Range (9, 10);
		int randomSkip4 = UnityEngine.Random.Range (12, 13);

		while (i < 18) {
			if (i == randomSkip1 || i == randomSkip2 || i == randomSkip3 || i == randomSkip4) {
				deck [i] = deck [i - 1];
				if (deck [i] == randomMazeSeed)
					seedCtr++;
				else
					seedOpCtr++;
				i++;
			}

			else if (deck[i - 1] == randomMazeSeed) {  //Leanring Env
				deck[i] = seedOpposite;
				seedOpCtr++;
				i++;
			} else if (deck[i - 1] == seedOpposite) {
				deck [i] = randomMazeSeed;
				seedCtr++;
				i++;
			}
		}
		string debugString = "";
		foreach (VideoMatchingPhaseEnum name in deck) {
			debugString += name + ", ";
		}
		Debug.Log (debugString);
		return deck;
	}
}