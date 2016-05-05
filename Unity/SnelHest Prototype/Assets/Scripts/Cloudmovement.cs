using UnityEngine;
using System.Collections;

public class Cloudmovement : MonoBehaviour {
	//public Vector3 targetPosition = new Vector3 (0.26f, 0.35f, 0);
	public Vector2 speeds;

	float speed;
	// Use this for initialization
	void Start () {
		speed = Random.Range (speeds.x, speeds.y);
	}
	
	// Update is called once per frame
	void Update () {
		transform.position += new Vector3 (speed, 0, 0) * Time.deltaTime;
	//transform.localPosition = Vector3.Lerp(transform.localPosition, targetPosition, Time.deltaTime * speed);
	
	}
}
