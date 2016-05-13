using UnityEngine;
using System.Collections;

public class DontDestroy : MonoBehaviour {
	public GameObject StartScreen;

	private static DontDestroy _instance;
	// Use this for initialization
	void Awake () {
		if (!_instance)
			_instance = this;
		else
			Destroy (this.gameObject);
		
		DontDestroyOnLoad (this.gameObject);

		//if (!StartScreen == null) {
			StartScreen.SetActive (true);
			Time.timeScale = 0;
		//} else
		//	Debug.LogError ("No Startscreen assigned to DontDestroy.cs");
	}

	public void Begin(){
		Time.timeScale = 1;
		StartScreen.SetActive (false);
	}
}
