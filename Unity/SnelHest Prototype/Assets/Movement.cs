using UnityEngine;
using System.Collections;

public class Movement : MonoBehaviour{

	public float slowValue;
	float Speedmod = 1;
	float Crash = 0;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame

	void Update () {
		transform.position += new Vector3 (Input.GetAxis ("Horizontal"), Input.GetAxis ("Vertical"), 0) * Time.deltaTime * 10 * Speedmod;
            
                
            
	
	}
	void OnTriggerEnter2D(Collider2D other)
	{
		if(other.tag == "Slow")
			Speedmod = slowValue;
		if (other.tag == "Stamina")
			print ("Deplete Stamina");
		if (other.tag == "Crash")
			Speedmod = Crash;
	}
	void OnTriggerExit2D(Collider2D other)
	{
		Speedmod = 1;
	}
}
