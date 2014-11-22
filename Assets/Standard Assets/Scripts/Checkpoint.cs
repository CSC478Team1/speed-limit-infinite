using UnityEngine;
using System.Collections;

public class Checkpoint : MonoBehaviour {

	private bool triggered = false;

	private void OnTriggerEnter2D (Collider2D other){
		if (other.tag == "Player" && !triggered){
			other.GetComponent<PlayerController>().SetCheckpoint(transform);
			GameManager.DisplayMessage("Checkpoint Reached");
			triggered = true;
		}

	}
}
