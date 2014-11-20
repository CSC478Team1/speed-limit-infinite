using UnityEngine;
using System.Collections;

public class ReverseGravity : MonoBehaviour {

	Vector2 currentGravity;

	private void Awake(){
		currentGravity = Physics2D.gravity;
	}

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

	private void FlipSprite(GameObject gameObject){
		Vector3 scale = gameObject.transform.localScale;
		scale.y *= -1;
		gameObject.transform.localScale = scale;
	}
}
