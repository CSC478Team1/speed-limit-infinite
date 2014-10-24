using UnityEngine;
using System.Collections;

public class Controller : MonoBehaviour {
	protected int health = 100;
	protected float deathHeight = -15f;  //Y value to determine if Character fell off map
	protected bool isFacingRight = true; 
	protected bool canJump = true;
	protected Animator anim;
	protected float speed;
	protected float jumpForce;

	// Use this for initialization
	protected virtual void Start () {
		anim = GetComponent<Animator>();
	
	}

	protected virtual void FixedUpdate(){
		if (transform.position.y <= deathHeight) {
			//player died send back to spawn point 
			if (gameObject.tag == "Player"){
				health = 0; //fix this later if we implement lives
			}
			else if (gameObject.tag == "Enemy"){
				Destroy(gameObject);  // add death call later
			}
		}
	}
	//Decrement health return if character is alive or not
	protected bool Damaged(int damage){
		health -= damage;
		return (health <= 0) ? true: false;
	}
	protected void MirrorSprite(){
		isFacingRight = !isFacingRight;
		Vector3 scale = transform.localScale;
		scale.x *= -1;
		transform.localScale = scale;
	}
	public void DoSomething(){
	}
}
