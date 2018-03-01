using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MazeNameEnum { Practice, Visuomotor, Learning, Matching }
public enum VideoMatchingPhaseEnum { Learning, Matching }
public enum PhaseEnum { StudyVideo, StudyMatching, StudyVideoMatching, TestTrials }
public enum MatchingPathEnum { A, B, C }
public enum SphereColor { Red, Blue }

public class MazeSettings {
	public MazeNameEnum MazeName;
	public MatchingPathEnum MatchingPath;
	public int SpherePattern = 0;
}

public class ExperimentSettings {
	public string ParticipantID;
	public string ExperimenterInitials;
	public DateTime Date;
	public MazeSettings MazeSettings;
	public string FileDir;
	public string FileName;
	public PhaseEnum Phase;
	public int VideoMatchingTrialIndex = 0;
	public int MatchingScore = 0;
	public int MatchingTrialIndex = 0;
	public int VideoTrialIndex = 0;
	public int TestTrialIndex = 0;
	public int TestTrialCtr = 0;
	public string[] MatchingAnswers;
	public VideoMatchingPhaseEnum[] VideoMatchingOrder;
	public MatchingPathEnum[] MatchingPathOrder;
	public int[] SpherePatternOrder;
	public string [] PathAndSphereList;

	//singleton:	
	private static ExperimentSettings _instance;

	public static ExperimentSettings GetInstance() {
		if (_instance == null) _instance = new ExperimentSettings();
		return _instance;
	}
		
	private ExperimentSettings(){}
}



