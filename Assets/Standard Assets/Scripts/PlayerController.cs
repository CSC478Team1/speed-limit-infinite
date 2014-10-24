using UnityEngine;
using System.Collections;

public class PlayerController : Controller {
	
	private float initialSpeed = 8f;
	private int ignoreLayerBitmask;  //ignoring this layer temprorarily for platforms
	private Transform groundCheck; //below player box collider
	private Transform headCheck;  //above player box collider
	private float initialJumpforce = 600f;
	private LayerMask groundLayerMask;  //ignoring just this layer for jumping


	//save powerup information from scene to scene
	private static bool infiniteSpeed = false;
	private static bool canShootLaser = false;
	private static bool canShootDualLaser = false;
	private static bool hasGravityBoots = false;

	
	// Use this for initialization
	protected override void Start (){
		base.Start();
		speed = initialSpeed;
		jumpForce = initialJumpforce;

		//Get bitmask of Player Layer and perform NOT on it
		//Anything that is not Player Layer will allow jumping
		groundLayerMask = ~(1 << LayerMask.NameToLayer("Player"));


		try{
			//load head, side, and ground transforms and apply bitmask to our ingore layer
			groundCheck = gameObject.transform.Find ("GroundCheck").transform;  
			headCheck = gameObject.transform.Find("HeadCheck").transform;
			ignoreLayerBitmask = 1 << LayerMask.NameToLayer("Platform");
		}catch (UnityException e){
			Debug.Log(e.Message);
		}
	}

	//FixedUpdate can be called variable amount of times per frame
	//Physics is generally suggested here
	protected override void FixedUpdate(){
		base.FixedUpdate();
		if (health > 0){
			//determine if player is moving 
			float moveHorizontal = Input.GetAxis("Horizontal");
			float moveVertical = rigidbody2D.velocity.y;

			//set animator variables to trigger anim states
			anim.SetFloat("VerticalSpeed", moveVertical);  //no longer used in jumping animation
			anim.SetFloat("HorizontalSpeed", Mathf.Abs(moveHorizontal));

			//change player movement data here
			rigidbody2D.velocity = new Vector2 (moveHorizontal * speed, moveVertical);


			// first case player moves right, not facing right
			// second case player is facing right but moves left
			if (moveHorizontal > 0 && !isFacingRight)
				MirrorSprite();
			else if (moveHorizontal < 0 && isFacingRight)
				MirrorSprite();

			//prevent multiple jumps
			//movevertical doesn't work on moving platforms
			//isJumping = (moveVertical != 0) ? true : false;  
			canJump = Physics2D.Linecast(transform.position, groundCheck.position ,groundLayerMask) ? true: false;


			//use raycasts to look below player and above.
			//collider that are triggers are ignored by collisions
			RaycastHit2D raycastFeet = Physics2D.CircleCast (groundCheck.position, .20f,  -Vector2.up, 1f, ignoreLayerBitmask);
			if (raycastFeet.transform !=null)
				raycastFeet.collider.isTrigger = false;

			RaycastHit2D raycastHead = Physics2D.CircleCast (headCheck.position, .20f,  Vector2.up, 1f, ignoreLayerBitmask);
			if (raycastHead.transform !=null)
				raycastHead.collider.isTrigger = true;

			//checks from head to midbody for a platform if true then makes it passable - may not be necessary
			RaycastHit2D raycastStuckInPlatformCheck = Physics2D.CircleCast (headCheck.position, .12f, -Vector2.up, .32f, ignoreLayerBitmask);
			if (raycastStuckInPlatformCheck.transform !=null)
				raycastStuckInPlatformCheck.collider.isTrigger = true;
		
		}else {
			Respawn();
		}
	}
	
	// Update is called once per frame
	// Player Input should always be called within Update
	protected void Update () {
		//base.Update();
		//maps to player's controls
		if (Input.GetButtonDown("Jump") && canJump){

			//I'm playing around with upside down jump forces
			jumpForce = (headCheck.position.y < groundCheck.position.y) ? jumpForce *-1 : initialJumpforce;
				
			//add force to jump only in Y axis
			if (Input.GetButton("Boost"))
				rigidbody2D.AddForce (new Vector2 (0, jumpForce * 1.5f));
			else
				rigidbody2D.AddForce (new Vector2 (0, jumpForce));

			anim.SetTrigger("Jumping");
			canJump = false;
			speed = initialSpeed;
		} else if (Input.GetButton("Horizontal") && infiniteSpeed){
			speed += .13f;
		} else 
			speed = initialSpeed;


	}
	private void Respawn(){
		try{
			Transform spawnpoint = GameObject.FindWithTag("SpawnPoint").transform;
			transform.position = spawnpoint.position;
			health = 100;
		} catch (UnityException e){
			Debug.Log(e.Message);
		}
	}

	public void SetPowerUp(Item.PowerUpType powerUP){
		switch (powerUP){

		case Item.PowerUpType.DualLaser: 
			canShootDualLaser = true;
			break;
		case Item.PowerUpType.GravityBoots:
			hasGravityBoots = true;
			break;
		case Item.PowerUpType.InfiniteSpeed:
			infiniteSpeed = true;
			break;
		case Item.PowerUpType.Laser:
			canShootLaser = true;
			break;
		
		}
	}
	
}
