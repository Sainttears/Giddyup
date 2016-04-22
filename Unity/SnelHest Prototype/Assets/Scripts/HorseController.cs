using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class HorseController : MonoBehaviour {
	PositionChecker pc;

	public Slider staminaBar;

	public float stamina = 1;
	public float speed = 0;
	public Vector2 jumpForce;
	public float baseMove;
	public float baseMoveMod;

	float slowValue = 1;
	float crash = 0;

	float speedMod = 0;
	float speedInp = 1;
	float jumpInp = 0;

	void Awake(){
		pc = GameObject.Find ("Main Camera").GetComponent<PositionChecker> ();

		stamina = 1;
		speed = 0;
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetButton (this.name)) {
			speedInp -= Time.deltaTime;
		}
		if (Input.GetButtonUp (this.name)) {
			speedMod = speedInp;
			speedInp = 1;
		}
		if (Input.GetAxis (this.name) < 0) {
			jumpInp += Time.deltaTime;
		} else {
			jumpInp = 0;
		}

		speedMod -= 0.5f * Time.deltaTime;
		stamina -= (speedMod / 500);
		stamina += 0.1f * Time.deltaTime * pc.GetLead (this.transform);

		speedInp = Mathf.Clamp (speedInp, 0, 1);
		speedMod = Mathf.Clamp (speedMod, 0, 1);
		stamina = Mathf.Clamp (stamina, 0, 1);

		if (stamina > 0.5f)
			speed = (baseMove + speedMod) * slowValue * baseMoveMod;
		else if (stamina > 0.1f)
			speed = (baseMove + speedMod) * 0.5f * slowValue * baseMoveMod;
		else {
			speed = 0;
			print (this.name + "! Your Horse Need To Rest");
		}

		this.transform.position += new Vector3 (speed / 10, 0, 0);
		if (jumpInp >= 1) {
			this.GetComponent<Rigidbody2D> ().AddForce (jumpForce);
			jumpInp = 0;
		}


		staminaBar.value = stamina;
		staminaBar.transform.position = new Vector3(this.transform.position.x, this.transform.position.y + 1, 0);
	}

	void OnCollisionEnter2D(Collision2D other){
		jumpInp = 0;
	}
	void OnCollisionExit2D(Collision2D other){
		jumpInp = -10;
	}

	void OnTriggerEnter2D(Collider2D other)
	{
		if (other.tag == "Slow")
			slowValue = other.GetComponent<SlowValue> ().GetSlow ();
		if (other.tag == "Stamina")
			print ("Deplete Stamina");
		if (other.tag == "Crash")
			slowValue = crash;
	}
	void OnTriggerExit2D(Collider2D other)
	{
		slowValue = 1;
	}
}