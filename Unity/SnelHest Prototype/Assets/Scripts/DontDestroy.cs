using UnityEngine;
using System.Collections;

public class DontDestroy : MonoBehaviour {
	private static DontDestroy _instance;
	// Use this for initialization
	void Awake () {
		if (!_instance)
			_instance = this;
		else
			Destroy (this.gameObject);
		
		DontDestroyOnLoad (this.gameObject);
	}
}
