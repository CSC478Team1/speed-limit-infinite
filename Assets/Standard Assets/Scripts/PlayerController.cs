using UnityEngine;
using System.Collections;
/// <summary>
/// Player controller. Inherits from Controller. Used to perform various user interactions in the game.
/// </summary>
public class PlayerController : Controller {

	//save powerup information from scene to scene
	//declare static to retain data between scenes
	private static bool infiniteSpeed = false;
	private static bool canShootLaser = false;
	private static bool canShootDualLaser = false;
	private static bool hasGravityBoots = false;
	private static bool canShootLargeLaser = false;
	private static bool canShootTripleLaser = false;
	
	private float initialSpeed = 8f;
	private int ignoreLayerBitmask;  //ignoring this layer temprorarily for platforms
	private Transform groundCheck; //below player box collider
	private Transform headCheck;  //above player box collider
	private float initialJumpforce = 600f;
	private LayerMask groundLayerMask;  //ignoring just this layer for jumping
	private Transform spawnpoint;
	private int startingHealth = 100;


	/// <summary>
	/// Initialize values for PlayerController
	/// </summary>
	protected override void Start (){
		base.Start();

		speed = initialSpeed;
		jumpForce = initialJumpforce;

		//Get bitmask of Player Layer and perform NOT on it
		//Anything that is not Player Layer will allow jumping
		groundLayerMask = ~((1 << LayerMask.NameToLayer("Player")) | (1 << LayerMask.NameToLayer("Enemy")) | (1 << LayerMask.NameToLayer("Trigger")) |
		                    (1 << LayerMask.NameToLayer("Waypoint")) | (1 << LayerMask.NameToLayer("IgnorePlayer")) | (1 << LayerMask.NameToLayer("IgnoreEnemy")) 
		                    | (1 << LayerMask.NameToLayer("WaypointEdge")));

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

	/// <summary>
	/// Called every Fixed Frame. Perform most of the Physics related calls here.
	/// (Requirement 1.4.1) Platforms - Player can stand on
	/// (Requirement 1.4.3) Platforms - Can't jump through (or can jump through)
	/// (Requirement 2.1.1) Player Action - Move Forward
	/// (Requirement 2.1.2) Player Action - Move Backwards
	/// (Requirement 2.1.4.1) Player Action - Dies From AI Shots
	/// </summary>
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


			float circleRadius = .28f;

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

			//prevent multiple jumps & power jumps
			canJump = Physics2D.OverlapCircle(groundCheck.position, .14f ,groundLayerMask);
			if (canJump && raycastStuckInPlatformCheck.collider != null)
				canJump = false;
				
		}else {
			if (!gameObject.name.Contains(GameResources.ObjectClone))
				GameManager.PlayerHasDied();
			Respawn();
		}
	}
	
	/// <summary>
	/// Update is called once per frame. Player Input is handled here.
	/// (Requirement 2.1.3) Player Action - Jump
	/// </summary>
	protected void Update () {
		//base.Update();
		//maps to player's controls
		if (Input.GetButtonDown("Jump") && canJump){

			//I'm playing around with upside down jump forces
			jumpForce = (headCheck.position.y < groundCheck.position.y) ? initialJumpforce *-1 : initialJumpforce;
				
			//add force to jump only in Y axis
			if (Input.GetButton("Boost")){
				if (!isClone)
					SoundManager.PlaySoundAtCamera(GameResources.GetAudioClip(GameResources.KeyAudioPlayerSmallJump));
				rigidbody2D.AddForce (new Vector2 (0, jumpForce * 1.5f));
			} else {
				if (!isClone)
					SoundManager.PlaySoundAtCamera(GameResources.GetAudioClip(GameResources.KeyAudioPlayerSmallJump));
				rigidbody2D.AddForce (new Vector2 (0, jumpForce));
			}

			anim.SetTrigger("Jumping");
			canJump = false;
			speed = initialSpeed;
		} 

		if (Input.GetButton("Horizontal") && infiniteSpeed){
			speed += .07f;
		} else 
			speed = initialSpeed;

		if (Input.GetButtonDown("Fire1")){
			if (canShootTripleLaser){
				anim.SetTrigger("Shoot Dual");
			}
			else if (canShootLargeLaser){
				anim.SetTrigger("Shoot Single");
			} else if (canShootDualLaser){
				anim.SetTrigger("Shoot Dual");
			} else if (canShootLaser){
				anim.SetTrigger("Shoot Single");
			}
		}
	}

	/// <summary>
	/// Respawns Player to last transform position
	/// (Requirement 2.1.5.2) Player Actions - Resets to last check point
	/// </summary>
	private void Respawn(){
		try{
			//prevent clones from respawning with player
			if(gameObject.name.Contains(GameResources.ObjectClone))
				Destroy(gameObject);
			else{
				gameObject.rigidbody2D.velocity = Vector2.zero;
				if (spawnpoint != null)
					transform.position = spawnpoint.position;

				ResetHealth();
				isDead = false;
			}
		} catch (UnityException e){
			Debug.Log(e.Message);
		}
	}

	/// <summary>
	/// Shoots a laser.
	/// </summary>
	private void ShootLaser(){
		if (canShootTripleLaser)
			FireWeapon(GameResources.GetGameObject(GameResources.KeyBlueLargeTripleLaser), 20f);
		else if (canShootLargeLaser)
			FireWeapon(GameResources.GetGameObject(GameResources.KeyBlueLargeLaser), 9f);
		else if (canShootDualLaser)
			FireWeapon(GameResources.GetGameObject(GameResources.KeyBlueDualLaser), 14f);
		else if (canShootLaser)
			FireWeapon(GameResources.GetGameObject(GameResources.KeyBlueSingleLaser), 14f);
	}

	/// <summary>
	/// Adds a power up.
	/// </summary>
	/// <param name="powerUP">Power Up to add.</param>
	public void AddPowerUp(Item.PowerUpType powerUP){
		SetPowerUp(powerUP, true);
	}

	/// <summary>
	/// Removes a power up.
	/// </summary>
	/// <param name="powerUP">Power Up to remove.</param>
	public void RemovePowerUp(Item.PowerUpType powerUP){
		SetPowerUp(powerUP, false);
	}

	/// <summary>
	/// Can remove or add power up
	/// </summary>
	/// <param name="powerUP">Power Up to perform action on.</param>
	/// <param name="value">If set to <c>true</c> it adds power up otherwise it removes it.</param>
	private void SetPowerUp (Item.PowerUpType powerUP, bool value){
		switch (powerUP){

		case Item.PowerUpType.DualLaser: 
			canShootDualLaser = value;
			break;
		case Item.PowerUpType.GravityBoots:
			hasGravityBoots = value;
			break;
		case Item.PowerUpType.InfiniteSpeed:
			infiniteSpeed = value;
			break;
		case Item.PowerUpType.Laser:
			canShootLaser = value;
			break;
		case Item.PowerUpType.LargeLaser:
			canShootLargeLaser = value;
			break;
		case Item.PowerUpType.TripleLaser:
			canShootTripleLaser = value;
			break;
		}
	}

	/// <summary>
	/// Sets next Respawn location to transform location
	/// </summary>
	/// <param name="checkpointTransform">Checkpoint transform.</param>
	public void SetCheckpoint(Transform checkpointTransform){
		spawnpoint = checkpointTransform;
	}

	/// <summary>
	/// Resets the health.
	/// </summary>
	public void ResetHealth(){
		SetHealth(startingHealth);
	}
	/// <summary>
	/// Resets the health.
	/// </summary>
	/// <param name="max">Maximum health</param>
	/// <param name="current">Current health</param>
	public void ResetHealth(int max, int current){
		startingHealth = max;
		maxHealth = max;
		health = current;
		SetHealth(current);
	}
	/// <summary>
	/// Resets the speed. Only called sometimes after Infinite Speed power up is obtained
	/// </summary>
	public void ResetSpeed(){
		speed = initialSpeed;
	}



	
}
