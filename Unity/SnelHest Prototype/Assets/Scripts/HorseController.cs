using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class HorseController : MonoBehaviour {
	PositionChecker pc;

	public Slider staminaBar;
	public Vector2 jumpForce;

	public ParticleSystem ps;

	float stamina = 100;
	float speed = 0;
	float baseMoveMod = 20;
	float slowValue = 1;
	float crash = 0;
	float speedMod = 0;
	float speedInp = 1;
	float jumpInp = 0;

	bool grounded;
	bool resting;
	bool callOnce;
	bool dnf = false;

	Camera cam;


	void Awake(){
		cam = Camera.main;

		pc = GameObject.Find ("Main Camera").GetComponent<PositionChecker> ();

		stamina = 100;
		speed = 0;

		grounded = true;
		this.GetComponent<Animator> ().SetBool ("grounded", true);

		resting = false;
		callOnce = false;
	}
	
	// Update is called once per frame
	void Update () {
		if (!dnf) {
			this.GetComponent<Animator> ().SetBool ("grounded", grounded);

			if (!resting) {
				StopCoroutine (waitFor (2));

				if (grounded)
					ParticleSystemExtension.SetEmissionRate (ps, 100);
				else
					ParticleSystemExtension.SetEmissionRate (ps, 0);

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
				
				speedMod -= 2.5f * Time.deltaTime;
				stamina += 10 * Time.deltaTime * (1 + pc.GetLead (this.transform) / 10);

				speed = (speedMod / 10) * slowValue * baseMoveMod * Time.deltaTime;
				this.GetComponent<Animator> ().SetFloat ("speed", speed);

				this.transform.position += new Vector3 (speed / 10, 0, 0);
				if (jumpInp >= 0.4 && stamina >= 10 && grounded) {
					this.GetComponent<Rigidbody2D> ().AddForce (jumpForce);
					jumpInp = -10;
					stamina -= 10;
					grounded = false;
				}

				if (stamina <= 10)
					resting = true;

			} else if (resting) {
				stamina -= 10 * Time.deltaTime;
				StartCoroutine (waitFor (3));
				if (!callOnce) {
					speedMod = speedMod / 4;
					callOnce = true;
				}
			}

			speedInp = Mathf.Clamp (speedInp, 0, 1);
			speedMod = Mathf.Clamp (speedMod, 0, 100);
			stamina = Mathf.Clamp (stamina, 0, 100);

			staminaBar.value = stamina;
			staminaBar.transform.position = new Vector3 (this.transform.position.x, this.transform.position.y + 1, 0);
		}
	}


	public IEnumerator waitFor (float i){
		yield return new WaitForSeconds (i);
		stamina += 50 * Time.deltaTime;
		if (stamina >= 90) {
			callOnce = false;
			resting = false;
		}
	}


	void OnCollisionEnter2D(Collision2D other){
		jumpInp = 0;
		grounded = true;
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

	void OnBecameInvisible(){
		dnf = true;
		this.gameObject.SetActive (false);
		staminaBar.gameObject.SetActive (false);
		cam.GetComponent<PositionChecker> ().DNF ();
	}
}

public static class ParticleSystemExtension{
	public static void EnableEmission(this ParticleSystem particleSystem, bool enabled){
		var emission = particleSystem.emission;
		emission.enabled = enabled;
	}

	public static float GetEmissionRate(this ParticleSystem particleSystem){
		return particleSystem.emission.rate.constantMax;
	}

	public static void SetEmissionRate(this ParticleSystem particleSystem, float emissionRate){
		var emission = particleSystem.emission;
		var rate = emission.rate;
		rate.constantMax = emissionRate;
		emission.rate = rate;
	}
}