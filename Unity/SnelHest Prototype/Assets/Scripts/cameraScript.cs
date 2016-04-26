using UnityEngine;
using System.Collections;

public class cameraScript : MonoBehaviour {
	Transform Player1;
	Transform Player2;

	public float maxSize = 5;
	public float minSize = 3.5f;

	public float xMax;
	public float xMin;
	public float yMax;
	
	public float cameraZoomModifier = 1.8f;
	public float xOffset;

	public float playerDist;

	Vector3 cameraCenter;

	PositionChecker posCheck;


	void Start(){
		posCheck = this.GetComponent<PositionChecker> ();
	}
	

	void Update () {
		Player1 = posCheck.GetPos (1);
		Player2 = posCheck.GetPos (posCheck.GetLength());

		playerDist = Vector2.Distance(Player1.transform.position, Player2.transform.position);
		cameraCenter = Vector3.Lerp (Player1.transform.position, Player2.transform.position, 0f);

		this.transform.position = new Vector3(Mathf.Clamp(cameraCenter.x, xMin, xMax) + xOffset, Mathf.Clamp(cameraCenter.y, -yMax, yMax), -10); 

		this.GetComponent<Camera>().orthographicSize = playerDist/cameraZoomModifier;
		this.GetComponent<Camera>().orthographicSize = Mathf.Clamp(this.GetComponent<Camera>().orthographicSize, minSize, maxSize);
	}
}
