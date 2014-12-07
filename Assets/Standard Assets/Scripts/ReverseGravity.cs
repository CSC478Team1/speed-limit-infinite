using UnityEngine;
using System.Collections;

/// <summary>
/// Reverse gravity script. Changes projects gravity and flips player sprite if they are in the gravity zone
/// </summary>
public class ReverseGravity : MonoBehaviour {

	/// <summary>
	/// Trigger event that checks for player and changes gravity and flips player's sprite in Y direction
	/// </summary>
	/// <param name="other">Other.</param>
	private void OnTriggerEnter2D (Collider2D other){
		try{
			if (other.tag == "Player"){
				GameManager.SetReverseGravity();

				Transform floorLock = gameObject.transform.Find("Floor Lock").transform;
				Transform floorLockLocation = gameObject.transform.Find("Lock Location").transform;

				floorLock.position = floorLockLocation.position;
				FlipSprite(other.gameObject);

			}
		}catch (UnityException e){
			Debug.Log(e.Message);
		}
	}

	/// <summary>
	/// Trigger event when player leaves area it resets gravity and flips player spirte in Y direction again.
	/// </summary>
	/// <param name="other">Other.</param>
	private void OnTriggerExit2D (Collider2D other){
		try{
			if (other.tag == "Player"){
				GameManager.SetOriginalGravity();
				FlipSprite(other.gameObject);
				
			}
		}catch (UnityException e){
			Debug.Log(e.Message);
		}
	}

	/// <summary>
	/// Flips the sprite.
	/// </summary>
	/// <param name="gameObject">Game object to flip</param>
	private void FlipSprite(GameObject gameObject){
		Vector3 scale = gameObject.transform.localScale;
		scale.y *= -1;
		gameObject.transform.localScale = scale;
	}
}
