using UnityEngine;
using System.Collections;

public class FallingPlatform : MonoBehaviour {

	private void OnTriggerEnter2D (Collider2D other){
		//Player hit switch to drop platform
		if (other.gameObject.tag == "Player")
			rigidbody2D.gravityScale = 10;
		else if(other.gameObject.tag == "Enemy")
			Destroy(other.gameObject); // enemy was smashed change to death sequence
	}
}
