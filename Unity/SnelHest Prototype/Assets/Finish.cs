using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Finish : MonoBehaviour {
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

	private string Screen_Shot_File_Name;

	Sprite sprite;

	// Use this for initialization
	void Start () {
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
	}
	
	// Update is called once per frame
	void Update () {
		if (playersFinnished == activePlayers)
			StartCoroutine (ShowScore());

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

		//timeScore.text = "Race Finished!\nPlayer One Time: " + pOneTime.ToString ("F2") + "\nPlayer Two Time: " + pTwoTime.ToString ("F2") + "\nPlayer Three Time: " + pThreeTime.ToString ("F2") + "\nPlayer Four Time: " + pFourTime.ToString ("F2");
	}
	void OnTriggerEnter2D(Collider2D other)
	{
		if (playersFinnished < 1) {
			TakeScreenshot ();
		}

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
		yield return new WaitForSeconds (2);
		endScreen.SetActive (true);
	}
}
