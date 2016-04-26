using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class HorseController : MonoBehaviour {
	PositionChecker pc;

	public Slider staminaBar;

	public float stamina = 100;
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

		stamina = 100;
		speed = 0;

		this.GetComponent<Animator> ().SetBool ("grounded", true);
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetButton (this.name)) {
			speedInp -= Time.deltaTime;
		}
		if (Input.GetButtonUp (this.name)) {
			speedMod += (speedInp);
			stamina -= speedInp * 2;
			speedInp = 1;
		}
		if (Input.GetAxis (this.name) < 0) {
			jumpInp += Time.deltaTime;
		} else {
			jumpInp = 0;
		}

		if (stamina <= 15)
			speedMod -= 100 * Time.deltaTime;
		else
			speedMod -= 2.5f * Time.deltaTime;
		stamina += 10 * Time.deltaTime * (1 + pc.GetLead (this.transform) / 10);

		speedInp = Mathf.Clamp (speedInp, 0, 1);
		speedMod = Mathf.Clamp (speedMod, 0, 100);
		stamina = Mathf.Clamp (stamina, 0, 100);

		speed = (baseMove + (speedMod / 10)) * slowValue * baseMoveMod * (stamina/100);
		this.GetComponent<Animator> ().SetFloat ("speed", speed);

		this.transform.position += new Vector3 (speed / 10, 0, 0);
		if (jumpInp >= 0.4) {
			this.GetComponent<Rigidbody2D> ().AddForce (jumpForce);
			jumpInp = -10;
			stamina -= 10;
			this.GetComponent<Animator> ().SetBool ("grounded", false);
		}


		staminaBar.value = stamina;
		staminaBar.transform.position = new Vector3(this.transform.position.x, this.transform.position.y + 1, 0);
	}

	void OnCollisionEnter2D(Collision2D other){
		jumpInp = 0;
		this.GetComponent<Animator> ().SetBool ("grounded", true);
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
		speedMod = speedMod * slowValue;
		slowValue = 1;
	}
}