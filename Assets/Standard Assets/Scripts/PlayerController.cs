using UnityEngine;
using System.Collections;

public class PlayerController : Controller {
	
	private float initialSpeed = 8f;
	private int ignoreLayerBitmask;  //ignoring this layer temprorarily for platforms
	private Transform groundCheck; //below player box collider
	private Transform headCheck;  //above player box collider
	private float initialJumpforce = 600f;
	private LayerMask groundLayerMask;  //ignoring just this layer for jumping
	private Transform spawnpoint;

	//save powerup information from scene to scene
	//declare static to retain data between scenes
	private static bool infiniteSpeed = false;
	private static bool canShootLaser = false;
	private static bool canShootDualLaser = false;
	private static bool hasGravityBoots = false;
	private static bool canShootLargeLaser = false;

	
	// Use this for initialization
	protected override void Start (){
		base.Start();

		speed = initialSpeed;
		jumpForce = initialJumpforce;

		//Get bitmask of Player Layer and perform NOT on it
		//Anything that is not Player Layer will allow jumping
		groundLayerMask = ~(1 << LayerMask.NameToLayer("Player"));

		try{
			//load head and ground transforms and apply bitmask to our ignore layer
			groundCheck = gameObject.transform.Find ("GroundCheck").transform;  
			headCheck = gameObject.transform.Find("HeadCheck").transform;
			ignoreLayerBitmask = 1 << LayerMask.NameToLayer("Platform");


			//load spawnpoint if it exists
			GameObject tempGameObject = GameObject.FindWithTag("SpawnPoint");
			if (tempGameObject != null)
				spawnpoint = tempGameObject.transform;

		}catch (UnityException e){
			Debug.Log(e.Message);
		}
	}

	//FixedUpdate can be called variable amount of times per frame
	//Physics is generally suggested here
	protected override void FixedUpdate(){
		base.FixedUpdate();
		if (!isDead){
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
			float circleRadius = .28f;
			//canJump = Physics2D.Linecast(transform.position, groundCheck.position ,groundLayerMask) ? true : false;
			canJump = Physics2D.OverlapCircle(groundCheck.position, .20f ,groundLayerMask) ? true : false;

			RaycastHit2D raycastFeet = Physics2D.CircleCast (groundCheck.position, circleRadius,  -Vector2.up, 1f, ignoreLayerBitmask);
			if (raycastFeet.transform !=null)
				raycastFeet.collider.isTrigger = false;

			RaycastHit2D raycastHead = Physics2D.CircleCast (headCheck.position, circleRadius,  Vector2.up, 1f, ignoreLayerBitmask);
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
			speed += .07f;
		} else if (Input.GetButtonDown("Fire1")){

			if (canShootLargeLaser){
				anim.SetTrigger("Shoot Single");
				FireWeapon(GameResources.GetGameObject(GameResources.KeyBlueLargeLaser), 9f);
			} else if (canShootDualLaser){
				anim.SetTrigger("Shoot Dual");
				FireWeapon(GameResources.GetGameObject(GameResources.KeyBlueDualLaser), 14f);
			} else if (canShootLaser){
				anim.SetTrigger("Shoot Single");
				FireWeapon(GameResources.GetGameObject(GameResources.KeyBlueSingleLaser), 14f);
			}
		} else
			speed = initialSpeed;


	}
	private void Respawn(){
		try{
			//prevent clones from respawning with player
			if(gameObject.name.Contains(GameResources.ObjectClone))
				Destroy(gameObject);
			else{
				if (spawnpoint != null)
					transform.position = spawnpoint.position;

				SetHealth(100);
				isDead = false;
			}
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
		case Item.PowerUpType.LargeLaser:
			canShootLargeLaser = true;
			break;
		
		}
	}
	public void SetCheckpoint(Transform checkpointTransform){
		spawnpoint = checkpointTransform;
	}
	
}
