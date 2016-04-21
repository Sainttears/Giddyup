using UnityEngine;
using System.Collections;

public class PlayerlaneCollision : MonoBehaviour {

	// Use this for initialization
	void Start () {}
	// Update is called once per frame
	void Update (){}

		
	void OnCollisonEnter2D(Collider2D other)
	{
		if (collision.gameObject.tag == "Collision")
			Physics2D.IgnoreCollision ();

 }
}
