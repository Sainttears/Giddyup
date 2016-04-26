using UnityEngine;
using System.Collections;

public class PositionChecker : MonoBehaviour {
	
	public string[] objectNames;


	Transform[] players;


	// Use this for initialization
	void Start () {
		players = new Transform[objectNames.Length];

		for (int i = 0; i < objectNames.Length; i++) {
			players [i] = GameObject.Find (objectNames [i]).transform;
		}
	}
	
	// Update is called once per frame
	void Update () {
		for (int i = 0; i < players.Length; i++) {
			if (i == 0) {
			
			} else if (players [i].position.x > players [i - 1].position.x){
				Transform j = players[i];
				Transform k = players[i - 1];
				players [i - 1] = j;
				players [i] = k;
			}
		}
	}

	//Currently used in the rubber banding to give more stamina to the last players
	public float GetLead(Transform obj){
		float lead = players[0].position.x - obj.position.x;

		/*if (lead < 1)
			lead = 1;
		Mathf.Clamp (lead, 1, 10);*/

		return Mathf.Abs(lead) / 2;
	}

	//Get the Transform of a player (1 - Max Length)
	public Transform GetPos(int pos){
		return players [pos - 1];
	}

	//Get Lenght of players[]
	public int GetLength(){
		return players.Length;
	}
}
