using UnityEngine;
using System.Collections;

public class cameraScript : MonoBehaviour {
	Transform leader;
	Transform looser;
	Transform dnf;

	public float maxSize = 5;
	public float minSize = 3.5f;

	public float xMax;
	public float xMin;
	public float yMax;
	
	public float cameraZoomModifier = 1.8f;
	public float xOffset;

	public float playerDist;

	Vector3 cameraCenter;
	Vector3 dnfPos;

	PositionChecker posCheck;


	void Start(){
		posCheck = this.GetComponent<PositionChecker> ();
	}
	

	void Update () {
		leader = posCheck.GetPos (1);
		looser = posCheck.GetPos (posCheck.GetLength(0));

		if (dnf != null) {
			dnfPos = Vector3.Lerp (dnfPos, looser.position, Time.deltaTime);
			playerDist = Vector2.Distance(leader.transform.position, dnfPos);
		}
		else
			playerDist = Vector2.Distance(leader.transform.position, looser.transform.position);

		cameraCenter = Vector3.Lerp (leader.transform.position, looser.transform.position, 0f);


		this.transform.position = new Vector3(Mathf.Clamp(cameraCenter.x, xMin, xMax) + xOffset, Mathf.Clamp(cameraCenter.y, -yMax, yMax), -10); 

		this.GetComponent<Camera>().orthographicSize = playerDist/cameraZoomModifier;
		this.GetComponent<Camera>().orthographicSize = Mathf.Clamp(this.GetComponent<Camera>().orthographicSize, minSize, maxSize);
	}

	public void AddDNF(){
		dnf = posCheck.GetPos (posCheck.GetLength(1));
		dnfPos = dnf.position;
	}
}
