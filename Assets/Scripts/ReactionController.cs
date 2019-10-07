using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ReactionController : MonoBehaviour {
	public Canvas canvas;
	public Text text;
	public GameObject circle, cross, bottomtText;
	public bool hasStarted = false, trialRunning = false, trialFinished = false;
	public int trialsToRun = 120;
	public float startTime = 0;
	public int currentTrial = 0;
	float[] times;

	static public bool hasEnded;

	// Use this for initialization
	void Start () {
		times = new float[trialsToRun];
		text.gameObject.SetActive(true);
		cross.SetActive(false);
		bottomtText.SetActive(false);
		circle.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () {
		if(!hasStarted)
		{
			if(Input.GetKeyDown("space"))
			{
				StartCoroutine(StartExperiment());
			}
		}
		else
		{
			if (currentTrial < trialsToRun)
			{
				if (trialRunning)
				{
					if (!trialFinished)
					{
						if (Input.GetKeyDown("space"))
						{
							trialFinished = true;
							trialRunning = false;
							times[currentTrial] = Mathf.Round((Time.time - startTime)*1000);
							circle.SetActive(false);
							text.gameObject.SetActive(true);
							text.text = times[currentTrial] + "msec";
							Invoke("StartNextTrial", .5f);
						}
					}
				}
			}
			else
			{
				cross.SetActive(false);
				bottomtText.SetActive(false);
				circle.SetActive(false);
				text.gameObject.SetActive(true);
				text.text = "You have reached the end of this task." + System.Environment.NewLine + System.Environment.NewLine + "Press the space to begin the next task.";
				if(Input.GetKeyDown("space"))
				{
					SceneManager.LoadScene(0);
				}
			}
		}
	}

	IEnumerator StartExperiment()
	{
		hasStarted = true;
		text.gameObject.SetActive(false);
		yield return new WaitForSeconds(.5f);
		cross.SetActive(true);
		bottomtText.SetActive(true);
		yield return new WaitForSeconds(.25f);
		circle.SetActive(false);
		cross.SetActive(false);
		trialFinished = false;
		Invoke("WaitForInput", Random.Range((int)1, (int)9));
	}
	void WaitForInput()
	{
		trialRunning = true;
		startTime = Time.time;
		circle.SetActive(true);
	}

	void StartNextTrial()
	{
		currentTrial += 1;
		if (currentTrial < trialsToRun)
		{
			trialFinished = false;
			StartCoroutine(StartExperiment());
		}
	}
}
