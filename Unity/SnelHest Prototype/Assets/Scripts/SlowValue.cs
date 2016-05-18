using UnityEngine;
using System.Collections;

public class SlowValue : MonoBehaviour {
	public float slowValue;

	void Update(){
		Vector3 viewPos = Camera.main.WorldToViewportPoint(this.transform.position);
		if (viewPos.x <= 0)
			Destroy (gameObject);
	}

	public float GetSlow(){
		return slowValue;
	}
}
