using UnityEngine;
using System.Collections;

public class Jumpscript : MonoBehaviour {

	public float playerSpeed;
	public Vector2 jumpHeight;

	void Start () {

	}
	void Update ()
	{

		if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space))
		{
			GetComponent<Rigidbody2D>().AddForce(jumpHeight, ForceMode2D.Impulse);
		}
	}
}