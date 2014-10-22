using UnityEngine;
using System.Collections;

public class PlayerController : Controller {
	
	private float initialSpeed = 8f;
	private int ignoreLayerBitmask;
	private int playerMask;
	private Transform groundCheck; //below player box collider
	private Transform headCheck;  //above player box collider

	// Use this for initialization
	protected override void Start () {
		base.Start();
		speed = initialSpeed;
		jumpForce = 600f;
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
			anim.SetFloat("VerticalSpeed", moveVertical);
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
			isJumping = (moveVertical != 0) ? true : false;

			//use raycasts to look below player and above.
			//collider that are triggers are ignored by collisions
			RaycastHit2D raycastFeet = Physics2D.CircleCast (groundCheck.position, .16f,  -Vector2.up, 1f, ignoreLayerBitmask);
			if (raycastFeet.transform !=null)
				raycastFeet.collider.isTrigger = false;

			RaycastHit2D raycastHead = Physics2D.CircleCast (headCheck.position, .16f,  Vector2.up, 1f, ignoreLayerBitmask);
			if (raycastHead.transform !=null)
				raycastHead.collider.isTrigger = true;

			//checks from head to midbody for a platform if true then makes it passable - may not be necessary
			RaycastHit2D raycastStuckInPlatformCheck = Physics2D.CircleCast (headCheck.position, .16f, -Vector2.up, .32f, ignoreLayerBitmask);
			if (raycastStuckInPlatformCheck.transform !=null)
				raycastStuckInPlatformCheck.collider.isTrigger = true;
		
		}else {
			Respawn();
		}
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
	
	// Update is called once per frame
	// Player Input should always be called within Update
	protected void Update () {
		//base.Update();
		//maps to player's controls
		if (Input.GetButtonDown("Jump") && !isJumping)
			//add force to jump only in Y axis
			if (Input.GetKey(KeyCode.LeftShift))
				rigidbody2D.AddForce (new Vector2 (0, jumpForce * 1.5f));
			else
				rigidbody2D.AddForce (new Vector2 (0, jumpForce));

			//speed = initialSpeed;
		//}
		//else if (Input.GetButton("Horizontal")){
			//speed += .08f;
		//}
		//else
			speed = initialSpeed;
	}
	
}
