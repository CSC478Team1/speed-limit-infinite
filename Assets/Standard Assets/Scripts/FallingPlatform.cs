using UnityEngine;
using System.Collections;

public class FallingPlatform : MonoBehaviour {

	private bool isUp = true;
	private Animator anim;
	private Transform startingLocation;
	private GameObject wallSwitch;
	private GameObject fallingPlatform;

	private void Awake(){
		wallSwitch = GameObject.Find("Switch") as GameObject;
		fallingPlatform = GameObject.Find("FPlatform") as GameObject;
		anim = wallSwitch.GetComponent<Animator>();
		startingLocation = fallingPlatform.transform;
	}

	private void Start(){

	}
	private void OnTriggerStay2D (Collider2D other){
		//Player hit switch to drop platform
		if (other.gameObject.tag == "Player" && gameObject.tag == "Player Object"){
			if (Input.GetButton("Action")){
				if (isUp){
					fallingPlatform.rigidbody2D.isKinematic = false;
					fallingPlatform.rigidbody2D.gravityScale = 10f;
					anim.SetBool("IsUp", !isUp);
				} /**else{

					fallingPlatform.rigidbody2D.gravityScale = 0f;
					fallingPlatform.transform.position = transform.position;
					Debug.Log(startingLocation.position.y.ToString());
					fallingPlatform.rigidbody2D.isKinematic = true;
				}
				isUp = !isUp;
				**/

			}

		}
		else if(other.gameObject.tag == "Enemy" && gameObject.tag == "Platform")
			Destroy(other.gameObject); // enemy was smashed change to death sequence
	}
}