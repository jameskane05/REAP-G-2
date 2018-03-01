using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class StudyTrialController : MonoBehaviour {
	
	[SerializeField] private Transform player;
	[SerializeField] private GameObject[] path;

	// Use this for initialization
	void Start () {
		DOTween.Init();
		Vector3[] waypoints = new Vector3[path.Length];
		for (int i = 0; i < path.Length; i++){
			waypoints.SetValue(path[i].transform.position,i);
		}
		player.transform.DOPath (waypoints, 62).SetLookAt (.03f);
		DOTween.Play (player);
	}}