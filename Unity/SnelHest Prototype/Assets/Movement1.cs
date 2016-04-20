using UnityEngine;
using System.Collections;

public class Movement1 : MonoBehaviour {
 
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        print(Input.GetAxis("Horizontal"));
        transform.Translate(1f * Time.deltaTime, 0f, 0f);
	
	}
}
