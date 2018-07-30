using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using System.IO;
using UnityEngine.SceneManagement;

public class StudyMatchingController : MonoBehaviour {

	[SerializeField] private GameObject player;
	private GameObject playerInstance;
	private Transform[] pathWaypoints;
	private Vector3[] pathPositions;
	static public ExperimentSettings _expInstance;
	private bool hasStarted = false;
	[SerializeField] private float tourDuration;
	[SerializeField] private GameObject[] spheres;
	[SerializeField] private int pauseBeforeStart;
	[SerializeField] private int pauseAfterFinish;
	[SerializeField] Canvas canvas;
	[SerializeField] Text text;
    [SerializeField] Text triviaText;
    [SerializeField] private GameObject triviaPanel;
	private bool triviaStarted = false;
	private GameObject activeSpheres;
    private string[] matchingQuestions = new string[5] {
        "Did the color of the first and last sphere match?",
        "Were there more than 8 spheres?",
        "Were the last two spheres the same color?",
        "Were there more blue spheres than red spheres?",
        "Was there a pyramid instead of a sphere at any point?",
    };
    private bool[] matchingAnswers = new bool[5] { true, true, true, false, false };

	void Start () {
		_expInstance = ExperimentSettings.GetInstance ();

		if (_expInstance.Phase == PhaseEnum.StudyVideoMatching) {
			Debug.Log("StudyVideoMatching ongoing, starting trial: " + _expInstance.VideoMatchingTrialIndex.ToString() );
		}
		GameObject path = GameObject.Find (spheres [_expInstance.MatchingTrialIndex % 5].name [0].ToString());
		Debug.Log ("path gameObject name: " + path.ToString ());
		Transform[] pathWaypoints = path.GetComponentsInChildren<Transform>();
		Debug.Log("pathWayoints length: " + pathWaypoints.Length);
		pathPositions = new Vector3[pathWaypoints.Length - 1];
		for (int i = 1; i <= pathPositions.Length; i++) {
			pathPositions [i - 1] = new Vector3(pathWaypoints [i].position.x,pathWaypoints [i].position.y+1,pathWaypoints [i].position.z);
		}
		activeSpheres = spheres[_expInstance.MatchingTrialIndex % 5];
		activeSpheres.SetActive (true);

		playerInstance = Instantiate (player, new Vector3(pathPositions[0].x,pathPositions[0].y,pathPositions[0].z), pathWaypoints[1].rotation);
	}

	void Update () {
        if (Input.GetKeyDown(KeyCode.E))
        {
            SceneManager.LoadScene(0);
        }

        if (Input.GetKeyDown ("space") && !hasStarted) {
			Image img = canvas.GetComponent<Image>();
			img.color = UnityEngine.Color.clear;
			text.gameObject.SetActive (false);
			StartCoroutine (PauseBeforeStart ());
			hasStarted = true;
		}
	}

	IEnumerator PauseBeforeStart (){
		yield return new WaitForSeconds (pauseBeforeStart);
		StartPath ();
	}

	void StartPath(){
		DOTween.Init();
		playerInstance.transform.DOPath (pathPositions, tourDuration, PathType.Linear)
			.SetLookAt (.03f)
			.SetEase (Ease.Linear)
			.OnComplete(StartPauseAfterFinishCoroutine);
		DOTween.Play (player);
	}

	void StartPauseAfterFinishCoroutine(){
		StartCoroutine (PauseAfterFinish ());
	}


	IEnumerator PauseAfterFinish (){
		yield return new WaitForSeconds (pauseAfterFinish);
		triviaPanel.SetActive (true);


        //WRITE TO FILE MATCHING ANSWER - INCLUDING WHAT PATH, ANSWER GIVEN, CORRECT ANSWER.
        if (_expInstance.Phase == PhaseEnum.StudyMatching)
        {
            triviaText.text = matchingQuestions[UnityEngine.Random.Range(0, 4)];
        }
        else {
            triviaText.text = matchingQuestions[_expInstance.MatchingTrialIndex % 5];
        }

        while (!Input.GetKeyDown (KeyCode.Alpha5) && !Input.GetKeyDown (KeyCode.Alpha9))
			yield return null;
        
		if (_expInstance.Phase == PhaseEnum.StudyVideoMatching) {

            if (!File.Exists(_expInstance.FileName + "_data.txt"))
            {

                List<string> experimentHeader = GetExperimentHeader();
                System.IO.File.WriteAllLines(_expInstance.FileName + "_data.txt", experimentHeader.ToArray());
                System.IO.File.WriteAllLines(_expInstance.FileName + "_data_path.txt", experimentHeader.ToArray());
            }
            
            System.IO.File.AppendAllText(_expInstance.FileName + "_data.txt",
                "Phase: Study" + "\r\n" +
                "Matching Path No.: " + (_expInstance.MatchingTrialIndex % 5) + "\r\n" +
                "Matching Question: " + matchingQuestions[_expInstance.MatchingTrialIndex % 5] + "\r\n");


            System.IO.File.AppendAllText(_expInstance.FileName + "_data_path.txt",
                "Phase: Study" + "\r\n" +
                "Matching Path No.: " + (_expInstance.MatchingTrialIndex % 5) + "\r\n" +
                "Matching Question: " + matchingQuestions[_expInstance.MatchingTrialIndex % 5] + "\r\n");


            if (Input.GetKeyDown (KeyCode.Alpha5)) { //TRUE, or YES
				_expInstance.MatchingAnswersGiven [_expInstance.MatchingTrialIndex % 5] = "Yes";
                if (matchingAnswers[_expInstance.MatchingTrialIndex % 5] == true)
                {
                    _expInstance.MatchingScore++;
                    System.IO.File.AppendAllText (_expInstance.FileName + "_data.txt", "Answer Given: Yes, correct\r\n\r\n");
                    System.IO.File.AppendAllText (_expInstance.FileName + "_data_path.txt", "Answer Given: Yes, correct\r\n\r\n");
                }
                else
                {
                    System.IO.File.AppendAllText (_expInstance.FileName + "_data.txt", "Answer Given: Yes, incorrect\r\n\r\n");
                    System.IO.File.AppendAllText (_expInstance.FileName + "_data_path.txt", "Answer Given: Yes, incorrect\r\n\r\n");
                }
				
			} else if (Input.GetKeyDown (KeyCode.Alpha9) && _expInstance.Phase == PhaseEnum.StudyVideoMatching)
            { //TRUE, or YES
                _expInstance.MatchingAnswersGiven [_expInstance.MatchingTrialIndex % 5] = "No";
				if (matchingAnswers[_expInstance.MatchingTrialIndex % 5] == true) {
					_expInstance.MatchingScore++;
					System.IO.File.AppendAllText (_expInstance.FileName + "_data.txt", "Answer Given: No, correct\r\n\r\n");
                    System.IO.File.AppendAllText (_expInstance.FileName + "_data_path.txt", "Answer Given: No, correct\r\n\r\n");
                }
				else 
					System.IO.File.AppendAllText (_expInstance.FileName + "_data.txt", "Answer Given: No, incorrect\r\n\r\n");
                    System.IO.File.AppendAllText (_expInstance.FileName + "_data_path.txt", "Answer Given: No, incorrect\r\n\r\n");
            }

			_expInstance.VideoMatchingTrialIndex++;
			_expInstance.MatchingTrialIndex++;

			if (_expInstance.VideoMatchingTrialIndex > 13) {
				Text text = GameObject.Find ("Text").GetComponent<Text> ();
				text.text = "You have completed the trials.";

				System.IO.File.AppendAllText (_expInstance.FileName + "_data.txt", "Total Matching Score: " + _expInstance.MatchingScore + " out of 5 \r\n\r\n");
                System.IO.File.AppendAllText(_expInstance.FileName + "_data_path.txt", "Total Matching Score: " + _expInstance.MatchingScore + " out of 5 \r\n\r\n");
                _expInstance.VideoMatchingTrialIndex = 0;
			} else {
				//Debug.Log ("StudyVideoMatching ongoing, starting trial: " + _expInstance.VideoMatchingTrialIndex.ToString ());
				SceneManager.LoadScene (_expInstance.VideoMatchingOrder [_expInstance.VideoMatchingTrialIndex].ToString ());
			}
		}
        

        //Debug.Log ("MatchingTrialIndex: " + (_expInstance.MatchingTrialIndex).ToString());
        //Debug.Log ("MatchingScore: " + _expInstance.MatchingScore.ToString());
        //Debug.Log ("VideoMatchingTrialIndex: " + (_expInstance.VideoMatchingTrialIndex).ToString());

        if (_expInstance.Phase == PhaseEnum.StudyMatching) {
			SceneManager.LoadScene (0);
		}
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