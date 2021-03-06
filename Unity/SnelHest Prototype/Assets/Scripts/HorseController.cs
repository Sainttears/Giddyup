﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class HorseController : MonoBehaviour {
	PositionChecker pc;

	//public Slider staminaBar;
	Vector2 jumpForce;

	public ParticleSystem ps;

	public GameObject finish;

	public AudioClip slowSound;
	public AudioClip splashSound;
	public AudioClip crashSound;

	float floatHeight;
	float liftForce;
	float damping;

	Finish fin;

	float bomAmmount = 0;
	//float stamina = 100;
	float speed = 0;
	float baseMoveMod = 20;
	float slowValue = 1;
	float crash = 0;
	float speedMod = 10;
	float speedInp = 1;
	float jumpInp = 0;

	int lives = 1;

	bool grounded;
	bool crashed;
	bool jumpOnce;
	bool dnf = false;
	bool inBush = false;
	bool inWater = false;
	bool callOnce = true;

	Vector2 crashForce;

	Camera cam;


	void Awake(){
		cam = Camera.main;

		pc = GameObject.Find ("Main Camera").GetComponent<PositionChecker> ();
		fin = finish.GetComponent<Finish> ();

		//stamina = 100;

		grounded = true;
		this.GetComponent<Animator> ().SetBool ("grounded", true);

		crashed = false;
		jumpOnce = false;

		jumpForce = new Vector2 (50, 250);
	}
	
	// Update is called once per frame
	void Update () {
		if (!dnf && fin.HasBegun()) {
			this.GetComponent<Animator> ().SetBool ("grounded", grounded);
			this.GetComponent<Animator> ().SetBool ("crashed", crashed);
			this.GetComponent<Animator> ().SetBool ("inBush", inBush);
			this.GetComponent<Animator> ().SetBool ("inWater", inWater);

			Vector3 viewPos = Camera.main.WorldToViewportPoint(this.transform.position);

			if (viewPos.x <= 0.1f)
				speedMod = Mathf.Lerp(speedMod, pc.GetLeader ().GetComponent<HorseController> ().GetSpeedMod (), Time.deltaTime * 0.5f);
			if (viewPos.x <= -0.1f && callOnce)
				OutOfBounds ();
				//this.transform.position = new Vector3 (Camera.main.transform.position.x, this.transform.position.y, this.transform.position.z);
			if (viewPos.x >= 0)
				callOnce = true;

			if (!crashed) {
				//StopCoroutine (waitFor (2));

				if (grounded) {
					ParticleSystemExtension.SetEmissionRate (ps, speed * 50);
					if(speed > 0.25)
						GetComponent<AudioSource> ().mute = false;
					else
						GetComponent<AudioSource> ().mute = true;
				} else {
					ParticleSystemExtension.SetEmissionRate (ps, 0);
					GetComponent<AudioSource> ().mute = true;
				}

				if (Input.GetButton (this.name)) {
					speedInp -= Time.deltaTime;
				}
				if (Input.GetButtonUp (this.name)) {
					if(speedInp <= 0.9f)
						speedMod += (speedInp * 1.25f);
					speedInp = 1;
				}
				if (Input.GetAxis (this.name) < 0) {
					jumpInp += Time.deltaTime;
				} else {
					jumpInp = 0;
				}
				
				speedMod -= 1.5f * Time.deltaTime;
				//stamina += 10 * Time.deltaTime * (1 + pc.GetLead (this.transform) / 10);

				speed = (speedMod / 5) * slowValue * baseMoveMod * Time.deltaTime * (pc.GetLead(this.transform) / 10 + 1);
				this.GetComponent<Animator> ().SetFloat ("speed", speed);

				this.transform.position += new Vector3 (speed / 10, 0, 0);
				if (!Input.GetButton(this.name + " Jump") && grounded && !jumpOnce) {
					this.GetComponent<Rigidbody2D> ().AddForce (jumpForce);
					jumpOnce = true;
					//stamina -= 10;
					//grounded = false;
				}
				if(Input.GetButtonDown(this.name + " Jump"))
					jumpOnce = false;

				//if (stamina <= 10)
					//crashed = true;

			} else if (crashed) {
				speedMod -= Time.deltaTime * 50;
				//stamina -= 10 * Time.deltaTime;
				//StartCoroutine (waitFor (3));
				//if (!callOnce) {
				//	speedMod = speedMod / 4;
				//	callOnce = true;
				//}
			}

			speedInp = Mathf.Clamp (speedInp, 0, 1);
			speedMod = Mathf.Clamp (speedMod, 0.025f, 100);
			speed = Mathf.Clamp (speed, 0.025f, 100);

			this.GetComponent<Animator> ().SetBool ("grounded", grounded);
			this.GetComponent<Animator> ().SetBool ("crashed", crashed);
			this.GetComponent<Animator> ().SetBool ("inBush", inBush);
			this.GetComponent<Animator> ().SetBool ("inWater", inWater);
			//stamina = Mathf.Clamp (stamina, 0, 100);

			//staminaBar.value = stamina;
			//staminaBar.transform.position = new Vector3 (this.transform.position.x, this.transform.position.y + 1, 0);
		}
	}


	//public IEnumerator waitFor (float i){
		//yield return new WaitForSeconds (i);
		//stamina += 50 * Time.deltaTime;
		//if (stamina >= 90) {
		//	callOnce = false;
		//	crashed = false;
		//}
	//}


	void OnCollisionEnter2D(Collision2D other){
		if (other.transform.tag == "Lane") {
			jumpInp = 0;
			grounded = true;
		}
	}
	void OnCollisionExit2D(Collision2D other){
		if (other.transform.tag == "Lane") {
			jumpInp = -10;
			grounded = false;
		}
	}

	void OnTriggerEnter2D(Collider2D other)
	{
		if (other.tag == "Slow") {
			slowValue = other.GetComponent<SlowValue> ().GetSlow ();
			inBush = true;
			AudioSource.PlayClipAtPoint (slowSound, Camera.main.transform.position);
		}
		if (other.tag == "Water") {
			slowValue = other.GetComponent<SlowValue> ().GetSlow ();
			inWater = true;
			AudioSource.PlayClipAtPoint (splashSound, Camera.main.transform.position);
		}
		if (other.tag == "Bom") {
			bomAmmount = bomAmmount + other.GetComponent<SlowValue> ().GetSlow ();
			slowValue = 0.5f - (bomAmmount / 10);
			other.GetComponentInParent<Rigidbody2D> ().isKinematic = false;
			other.GetComponentInParent<Rigidbody2D> ().AddForce (new Vector2(100, 50));
			AudioSource.PlayClipAtPoint (crashSound, Camera.main.transform.position);
		}
		if (other.tag == "Crash") {
			speedMod = speedMod / 2;
			crashForce = new Vector2 (speedMod * 10, 0);
			crashed = true;
			StartCoroutine (OnCrash ());
			AudioSource.PlayClipAtPoint (crashSound, Camera.main.transform.position);
		}
	}
	void OnTriggerExit2D(Collider2D other)
	{
		speedMod = speedMod * slowValue;
		slowValue = 1;
		bomAmmount = 0;
		inWater = false;
		inBush = false;
	}

	void OutOfBounds(){
		callOnce = false;

		StopCoroutine (OnCrash ());

		if (lives > 0) {
			lives--;
			speedMod = pc.GetLeader ().GetComponent<HorseController> ().GetSpeedMod () * 1.25f;
			//speedMod = Mathf.Lerp(speedMod, pc.GetLeader ().GetComponent<HorseController> ().GetSpeedMod () * 2f, 0.5f);
			slowValue = 1;
			bomAmmount = 0;
			inWater = false;
			inBush = false;
			crashed = false;
		} else {
			dnf = true;
			this.gameObject.SetActive (false);
			//staminaBar.gameObject.SetActive (false);
			cam.GetComponent<PositionChecker> ().DNF ();
		}
	}

	IEnumerator OnCrash(){
		this.GetComponent<Rigidbody2D> ().AddForce (crashForce);
		yield return new WaitForSeconds (1);
		crashed = false;
	}

	public float GetSpeedMod(){
		return speedMod;
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