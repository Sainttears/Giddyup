﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Finish : MonoBehaviour {
	public Text timer;
	public GameObject endScreen;
	public Text timeScore;

	float time;

	float pOneTime;
	float pTwoTime;
	float pThreeTime;
	float pFourTime;

	float activePlayers;
	float playersFinnished = 0;

	// Use this for initialization
	void Start () {
		time = 0;

		pOneTime = 0;
		pTwoTime = 0;
		pThreeTime = 0;
		pFourTime = 0;

		endScreen.SetActive (false);

		activePlayers = Camera.main.GetComponent<PositionChecker> ().GetLength (0);

		timeScore.text = "Race Finished!";
	}
	
	// Update is called once per frame
	void Update () {
		activePlayers = Camera.main.GetComponent<PositionChecker> ().GetLength (0);

		if (activePlayers == playersFinnished)
			endScreen.SetActive (true);
		
		time += Time.deltaTime;
		timer.text = time.ToString ("F2");

		//timeScore.text = "Race Finished!\nPlayer One Time: " + pOneTime.ToString ("F2") + "\nPlayer Two Time: " + pTwoTime.ToString ("F2") + "\nPlayer Three Time: " + pThreeTime.ToString ("F2") + "\nPlayer Four Time: " + pFourTime.ToString ("F2");
	}
	void OnTriggerEnter2D(Collider2D other)
	{
		if (other.name == "Player One") {
			pOneTime = time;
			timeScore.text = timeScore.text + "\nPlayer One Time: " + pOneTime.ToString("F2");
		}
		if (other.name == "Player Two") {
			pTwoTime = time;
			timeScore.text = timeScore.text + "\nPlayer Two Time: " + pTwoTime.ToString("F2");
		}
		if (other.name == "Player Three") {
			pThreeTime = time;
			timeScore.text = timeScore.text + "\nPlayer Three Time: " + pThreeTime.ToString("F2");
		}
		if (other.name == "Player Four") {
			pFourTime = time;
			timeScore.text = timeScore.text + "\nPlayer Four Time: " + pFourTime.ToString("F2");
		}

		playersFinnished += 1;

		/* To do:
		 * Change these if statemets to somehting like this:
		 * if(...){
		 * 		pXTime = time;
		 * 		timeScore.text = timeScore.text + "\nPlayer X Time: " + pXTime.ToString("F2");
		 * */
	}
}
