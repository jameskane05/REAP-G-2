using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MazeNameEnum { Practice, Visuomotor, Learning, Matching }
public enum VideoMatchingPhaseEnum { Learning, Matching }
public enum PhaseEnum { Practice, StudyVideo, StudyMatching, StudyVideoMatching, TestTrials }
public enum MatchingPathEnum { A, B, C }
public enum SphereColor { Red, Blue }

public class MazeSettings {
	public MazeNameEnum MazeName;
	public MatchingPathEnum MatchingPath;
	public int SpherePattern = 0;
    public string TrialName;
}

public class ExperimentSettings {
	public string ParticipantID;
	public string ExperimenterInitials;
	public DateTime Date;
    public IDictionary<string, int> TrialTracker = new Dictionary<string, int>();
    public MazeSettings MazeSettings;
	public string FileDir;
	public string FileName;
	public PhaseEnum Phase;
	public int VideoMatchingTrialIndex = 0;
	public int MatchingScore = 0;
	public int MatchingTrialIndex = 0;
	public int VideoTrialIndex = 0;
	public int TestTrialIndex = 0;
    public string[] TestTrialTypes = new string[24] {"shortcut", "shortcut", "shortcut", "shortcut", "shortcut", "shortcut", "shortcut", "shortcut",
        "shortcut", "shortcut", "shortcut", "shortcut", "shortcut", "shortcut", "shortcut", "shortcut",
        "novel-same", "novel-same", "novel-same", "novel-same",
        "novel-longer", "novel-longer", "novel-longer", "novel-longer"};
    public int TestTrialCtr = 0;
	public string[] MatchingAnswersGiven;
	public VideoMatchingPhaseEnum[] VideoMatchingOrder;
	public MatchingPathEnum[] MatchingPathOrder;
	public int[] SpherePatternOrder;
	public string [] PathAndSphereList;
    public int videoTrialCount = 9;
    public int matchingTrialCount = 5;

    //singleton:	
    private static ExperimentSettings _instance;

	public static ExperimentSettings GetInstance() {
		if (_instance == null) _instance = new ExperimentSettings();
		return _instance;
	}
		
	private ExperimentSettings(){}

    /*public static void NameAndCountTrial()
    {
        _instance.MazeSettings.TrialName = _instance.MazeSettings.MazeName.ToString();
        if (_instance.Phase == PhaseEnum.Practice) _instance.MazeSettings.TrialName += "_LearnT";

        else if (!ExperimentSettings.IsStudy() && !ExperimentSettings.IsPractice())
        {

        }

        if (_instance.TrialTracker.ContainsKey(_instance.MazeSettings.TrialName)) _instance.TrialTracker[_instance.MazeSettings.TrialName] += 1;
        else _instance.TrialTracker[_instance.MazeSettings.TrialName] = 1;
    }*/
}



