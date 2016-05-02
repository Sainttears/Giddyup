using UnityEngine;
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

	// Use this for initialization
	void Start () {
		time = 0;

		pOneTime = 0;
		pTwoTime = 0;
		pThreeTime = 0;
		pFourTime = 0;

		endScreen.SetActive (false);
	}
	
	// Update is called once per frame
	void Update () {
		if(pOneTime > 0 || pTwoTime > 0 || pThreeTime > 0 || pFourTime > 0)
			endScreen.SetActive (true);
		
		time += Time.deltaTime;
		timer.text = time.ToString ("F2");

		timeScore.text = "Race Finished!\nPlayer One Time: " + pOneTime.ToString ("F2") + "\nPlayer Two Time: " + pTwoTime.ToString ("F2") + "\nPlayer Three Time: " + pThreeTime.ToString ("F2") + "\nPlayer Four Time: " + pFourTime.ToString ("F2");
	}
	void OnTriggerEnter2D(Collider2D other)
	{
		if (other.name == "Player One")
			pOneTime = time;
		if (other.name == "Player Two")
			pTwoTime = time;
		if (other.name == "Player Three")
			pThreeTime = time;
		if (other.name == "Player Four")
			pFourTime = time;

	}
}
