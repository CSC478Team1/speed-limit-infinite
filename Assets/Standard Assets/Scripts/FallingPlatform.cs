using UnityEngine;
using System.Collections;

public class FallingPlatform : MonoBehaviour {

	private bool isUp = true;
	private Animator anim;
	private GameObject wallSwitch;
	private GameObject fallingPlatform;
	private bool fell = false;
	private bool disabled = false;

	private void Awake(){
		wallSwitch = GameObject.Find("Switch") as GameObject;
		fallingPlatform = GameObject.Find("FPlatform") as GameObject;
		anim = wallSwitch.GetComponent<Animator>();
	}

	private void OnTriggerStay2D (Collider2D other){
		//Player hit switch to drop platform
		if (other.gameObject.tag == "Player" && (gameObject.tag == "Player Object") || (gameObject.tag == "Trigger")){
			if (Input.GetButton("Action")){
				if (isUp){
					fallingPlatform.rigidbody2D.isKinematic = false;
					fallingPlatform.rigidbody2D.gravityScale = 10f;
					fallingPlatform.rigidbody2D.velocity = new Vector2(0f, -2f);
					anim.SetBool("IsUp", !isUp);
					isUp = false;
				} 
			}

		}

		if(other.gameObject.tag == "Enemy" && gameObject.tag == "Platform" && !fell){
			Destroy(other.gameObject); // enemy was smashed change to death sequence
		}
		if (!isUp && fallingPlatform.rigidbody2D.velocity.sqrMagnitude == 0 && !disabled){
			fallingPlatform.tag="Untagged";
			fallingPlatform.layer = LayerMask.NameToLayer("Default");
			gameObject.tag ="Untagged";
			fell = true;
			if (other.tag == "Enemy")
				Destroy(other.gameObject);
			disabled = true;
		}

	}
}