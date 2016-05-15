using UnityEngine;
using System.Collections;

public class DontDestroy : MonoBehaviour {
	public GameObject StartScreen;

	private static DontDestroy _instance;
	// Use this for initialization
	void Awake () {
		if (!_instance) {
			_instance = this;
			StartScreen.SetActive (true);
			Time.timeScale = 0;
			DontDestroyOnLoad (this.gameObject);
		}
		else
			Destroy (this.gameObject);

		//if (!StartScreen == null) {
			
		//} else
		//	Debug.LogError ("No Startscreen assigned to DontDestroy.cs");
	}

	public void Begin(){
		Time.timeScale = 1;
		StartScreen.SetActive (false);
	}
}
