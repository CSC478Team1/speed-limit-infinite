using UnityEngine;
using System.Collections;

/// <summary>
/// Falling platform. Script is attached to a platform and a switch. If the platform lands on an enemy when falling, it destroys the enemy.
/// </summary>
public class FallingPlatform : MonoBehaviour {

	private bool isUp = true;
	private Animator anim;
	private GameObject wallSwitch;
	private GameObject fallingPlatform;
	private bool fell = false;
	private bool disabled = false;


	/// <summary>
	/// Initialize the value for the switch and the platform
	/// </summary>
	private void Awake(){
		wallSwitch = GameObject.Find("Switch") as GameObject;
		fallingPlatform = GameObject.Find("FPlatform") as GameObject;
		anim = wallSwitch.GetComponent<Animator>();
	}

	/// <summary>
	/// Determine if player hit switch and if so change platform to a non-kinematic object. If the falling platform lands on an enemy, destroy the enemy.
	/// (Requirement 1.5.1.2.1) Triggers - Button activates some sort of action
	/// </summary>
	/// <param name="other">Other.</param>
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

		if(other.gameObject.tag == "Enemy" && gameObject.tag == "Platform" && !fell && !fallingPlatform.rigidbody2D.isKinematic){
			Destroy(other.gameObject); // enemy was smashed change to death sequence
		}
		if (!isUp && fallingPlatform.rigidbody2D.velocity.sqrMagnitude == 0 && !disabled && !fallingPlatform.rigidbody2D.isKinematic){
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