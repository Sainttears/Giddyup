using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Finish : MonoBehaviour {
	public bool tutorial = false;

	public GameObject pauseScreen;
	public Text timer;
	public GameObject endScreen;
	public Image img;
	public Text timeScore;
	public Text cd;

	public AudioClip getReady;
	public AudioClip setClip;
	public AudioClip gunFire;
	public AudioClip flash;

	float time;

	float pOneTime;
	float pTwoTime;
	float pThreeTime;
	float pFourTime;

	float activePlayers;
	float playersFinnished = 0;

	bool hasBegun = false;

	bool p1Fin = false;
	bool p2Fin = false;
	bool p3Fin = false;
	bool p4Fin = false;

	private string Screen_Shot_File_Name;

	Sprite sprite;

	// Use this for initialization
	void Start () {
		if (!tutorial) {
			time = 0;

			pOneTime = 0;
			pTwoTime = 0;
			pThreeTime = 0;
			pFourTime = 0;

			endScreen.SetActive (false);
			cd.gameObject.SetActive (false);
			pauseScreen.SetActive (false);

			activePlayers = Camera.main.GetComponent<PositionChecker> ().GetLength (0);

			StartCoroutine (CountDown ());
		} else {
			hasBegun = true;
			time = 20;
			timer.text = "Waiting for 2 more people to finish the tutorial...";
			activePlayers = Camera.main.GetComponent<PositionChecker> ().GetLength (0);
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (!tutorial) {
			if (playersFinnished == activePlayers)
				StartCoroutine (ShowScore ());

			if (hasBegun) {
				activePlayers = Camera.main.GetComponent<PositionChecker> ().GetLength (0);
		
				time += Time.deltaTime;
				timer.text = time.ToString ("F2");
			}

			if (Input.GetKeyDown (KeyCode.Escape)) {
				if (Time.timeScale == 1) {
					pauseScreen.SetActive (true);
					Time.timeScale = 0;
				} else {
					pauseScreen.SetActive (false);
					Time.timeScale = 1;
				}
			}
		} else {
			if (playersFinnished == 1) {
				timer.text = "Waiting for 1 more people to finish the tutorial...";
			} else if (playersFinnished >= 2) {
				timer.text = "Race Starting in " + time.ToString ("F0") + " seconds...";
				time -= Time.deltaTime;

				if (!p1Fin && (Input.GetButton ("Player One") || Input.GetButton ("Player One Jump")))
					time = 20;
				if (!p2Fin && (Input.GetButton ("Player Two") || Input.GetButton ("Player Two Jump")))
					time = 20;
				if (!p3Fin && (Input.GetButton ("Player Three") || Input.GetButton ("Player Three Jump")))
					time = 20;
				if (!p4Fin && (Input.GetButton ("Player Four") || Input.GetButton ("Player Four Jump")))
					time = 20;
				if (playersFinnished == 4) {
					time = 5;
					playersFinnished += 1;
				}

				if (time <= 0) {
					Application.LoadLevel (1);
				}
			}
		}

		//timeScore.text = "Race Finished!\nPlayer One Time: " + pOneTime.ToString ("F2") + "\nPlayer Two Time: " + pTwoTime.ToString ("F2") + "\nPlayer Three Time: " + pThreeTime.ToString ("F2") + "\nPlayer Four Time: " + pFourTime.ToString ("F2");
	}
	void OnTriggerEnter2D(Collider2D other)
	{
		if (playersFinnished < 1 && !tutorial) {
			TakeScreenshot ();
		}

		if (other.name == "Player One") {
			pOneTime = time;
			if(!tutorial)
				timeScore.text = timeScore.text + "\nPlayer One Time: " + pOneTime.ToString("F2");
			p1Fin = true;
		}
		if (other.name == "Player Two") {
			pTwoTime = time;
			if(!tutorial)
				timeScore.text = timeScore.text + "\nPlayer Two Time: " + pTwoTime.ToString("F2");
			p2Fin = true;
		}
		if (other.name == "Player Three") {
			pThreeTime = time;
			if(!tutorial)
				timeScore.text = timeScore.text + "\nPlayer Three Time: " + pThreeTime.ToString("F2");
			p3Fin = true;
		}
		if (other.name == "Player Four") {
			pFourTime = time;
			if(!tutorial)
				timeScore.text = timeScore.text + "\nPlayer Four Time: " + pFourTime.ToString("F2");
			p4Fin = true;
		}

		playersFinnished += 1;
	}

	public void Restart(){
		Time.timeScale = 1;
		Application.LoadLevel (Application.loadedLevel);
	}
	public void Quit(){
		Application.Quit ();
	}

	IEnumerator CountDown(){
		yield return new WaitForSeconds (1);
		cd.gameObject.SetActive (true);
		cd.text = "Get Ready!";
		AudioSource.PlayClipAtPoint (getReady, Camera.main.transform.position);
		yield return new WaitForSeconds (1);
		cd.text = "";
		yield return new WaitForSeconds (1);
		cd.text = "Set!";
		AudioSource.PlayClipAtPoint (setClip, Camera.main.transform.position);
		yield return new WaitForSeconds (1);
		cd.text = "";
		yield return new WaitForSeconds (1);
		cd.text = "Go!";
		AudioSource.PlayClipAtPoint (gunFire, Camera.main.transform.position);
		hasBegun = true;
		yield return new WaitForSeconds (1);
		cd.gameObject.SetActive (false);
	}

	public bool HasBegun(){
		return hasBegun;
	}

	void TakeScreenshot(){
		AudioSource.PlayClipAtPoint (flash, Camera.main.transform.position);

		Screen_Shot_File_Name = "FinishImage.png";
		Application.CaptureScreenshot (Application.persistentDataPath + "/" + Screen_Shot_File_Name); //Application.dataPath + "/Resources/" + 
		StartCoroutine (LoadImage ());
	}

	IEnumerator LoadImage(){
		yield return new WaitForSeconds (1);
		WWW www = new WWW ("File://" + Application.persistentDataPath + "/" + Screen_Shot_File_Name);
		print ("File://" + Application.persistentDataPath + "/" + Screen_Shot_File_Name);
		yield return www;

		Texture2D tex = www.texture;
		Rect rect = new Rect (0, 0, Screen.width,Screen.height);
		Sprite spr = Sprite.Create (tex, rect, new Vector2 (0, 0));

		img.sprite = spr;
	}

	IEnumerator ShowScore(){
		yield return new WaitForSeconds (1);
		endScreen.SetActive (true);
		yield return new WaitForSeconds (10);
		Application.LoadLevel (0);
	}
}
