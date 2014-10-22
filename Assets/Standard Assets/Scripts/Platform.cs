using UnityEngine;
using System.Collections;

public class Platform : MonoBehaviour {

	private int platformLayer;
	private int playerLayer;

	//initialize layers
	private void Awake(){
		platformLayer = LayerMask.NameToLayer("Platform");
		playerLayer = LayerMask.NameToLayer("Player");

	}
	/**
	//testing options between enter and stay	
	private void OnCollisionEnter2D (Collider2D other) {
		if (other.gameObject.tag == "Player")
			Physics2D.IgnoreLayerCollision(playerLayer,platformLayer, false);
	}
	private void OnTriggerEnter2D (Collider2D other){
		if (other.gameObject.tag == "Player")
			Physics2D.IgnoreLayerCollision(playerLayer,platformLayer, false);
	} 
	private void OnTriggerStay2D (Collider2D other){
		if (other.gameObject.tag == "Player")
			Physics2D.IgnoreLayerCollision(playerLayer,platformLayer, false);
	}
	private void OnTriggerExit2D (Collider2D other){
		if (other.gameObject.tag == "Player")
			Physics2D.IgnoreLayerCollision(playerLayer,platformLayer, true);
	}
	**/
}
