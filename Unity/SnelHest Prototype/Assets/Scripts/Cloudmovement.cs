using UnityEngine;
using System.Collections;

public class Cloudmovement : MonoBehaviour {
	public Vector3 targetPosition = new Vector3 (0.26f, 0.35f, 0);
	public float speed;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

	transform.localPosition = Vector3.Lerp(transform.localPosition, targetPosition, Time.deltaTime * speed);
	
	}
}
