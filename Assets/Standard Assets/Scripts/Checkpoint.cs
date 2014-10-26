using UnityEngine;
using System.Collections;

public class Checkpoint : MonoBehaviour {


	private void OnTriggerEnter2D (Collider2D other){
		if (other.tag == "Player"){
			other.GetComponent<PlayerController>().SetCheckpoint(transform);
		}

	}
}
