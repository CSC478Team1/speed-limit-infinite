using UnityEngine;
using System.Collections;
/// <summary>
/// Simple checkpoint script. Sets player's spawn point to current position if player collides with collider.
/// </summary>
public class Checkpoint : MonoBehaviour {

	private bool triggered = false;

	/// <summary>
	/// Called when object has entered collider zone. Checks if object has Player's tag to perform any action
	/// </summary>
	/// <param name="other">Other colliding object</param>
	private void OnTriggerEnter2D (Collider2D other){
		if (other.tag == "Player" && !triggered){
			other.GetComponent<PlayerController>().SetCheckpoint(transform);
			GameManager.DisplayMessage("Checkpoint Reached");
			triggered = true;
		}

	}
}
