using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class StaminaPrototype : MonoBehaviour {

	public float stamina;
	public Slider value;
	public bool isOne;


	// Use this for initialization
	void Start () {
		stamina = 1;
	}
	
	// Update is called once per frame
	void Update () {
		/*print (
			"Player One: " + Input.GetButton ("Player One") + "\n" +
			"Player Two: " + Input.GetButton ("Player Two")
		);*/
		//if(Input.GetButton("Player One")) print ("1");

		value.value = stamina;

		if ((Input.GetKeyDown(KeyCode.A) && stamina > 0 && isOne) || (Input.GetKeyDown(KeyCode.L) && stamina > 0 && !isOne)) {
			stamina -= 0.1f;
			this.transform.position += new Vector3 (1, 0, 0);
		} else
			stamina += 0.1f * Time.deltaTime * GameObject.Find("Main Camera").GetComponent<PositionChecker>().GetLead(this.transform);
	}
}
