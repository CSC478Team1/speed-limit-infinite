using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {


	private float deathHeight = -15f;  //Y value to determine player fell off map
	private bool isFacingRight = true; 
	private bool isJumping = false;
	private float initialSpeed = 12f;
	private float speed;  
	private float jumpForce = 1000f; // static for now
	private Animator anim;

	// Use this for initialization
	private void Start () {
		anim = GetComponent<Animator>();
		speed = initialSpeed;
	}

	//FixedUpdate can be called variable amount of times per frame
	//Physics is generally suggested here
	private void FixedUpdate(){
		if (transform.position.y <= deathHeight) {
			//player died send back to spawn point 
			Transform spawnpoint = GameObject.FindWithTag("SpawnPoint").transform;
			transform.position = spawnpoint.position;
		} else {

			//determine if player is moving 
			float moveHorizontal = Input.GetAxis("Horizontal");
			float moveVertical = rigidbody2D.velocity.y;

			//set animator variables to trigger anim states
			anim.SetFloat("VerticalSpeed", moveVertical);
			anim.SetFloat("HorizontalSpeed", Mathf.Abs(moveHorizontal));

			//change player movement data here
			rigidbody2D.velocity = new Vector2 (moveHorizontal * speed, moveVertical);


			// first case player moves right, not facing right
			// second case player is facing right but moves left
			if (moveHorizontal > 0 && !isFacingRight)
				FlipSprite();
			else if (moveHorizontal < 0 && isFacingRight)
				FlipSprite();

			//prevent multiple jumps
			isJumping = (moveVertical != 0) ? true : false;
		}
	}
	
	// Update is called once per frame
	// Player Input should always be called within Update
	private void Update () {

		//maps to player's controls
		if (Input.GetButtonDown("Jump") && !isJumping){
			//add force to jump only in Y axis
			rigidbody2D.AddForce (new Vector2 (0, jumpForce));
			speed = initialSpeed;
		}
		else if (Input.GetButton("Horizontal")){
			speed += .08f;
		}
		else
			speed = initialSpeed;
	}

	private void FlipSprite(){
		isFacingRight = !isFacingRight;
		Vector3 scale = transform.localScale;
		scale.x *= -1;
		transform.localScale = scale;
	}
}
