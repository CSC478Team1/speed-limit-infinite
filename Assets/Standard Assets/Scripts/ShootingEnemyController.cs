using UnityEngine;
using System.Collections;

/// <summary>
/// Shooting enemy controller. Inherits from Enemy Controller. Used to move enemy characters and fire projectiles at the player.
/// </summary>
public class ShootingEnemyController : EnemyController {
	//declare these public to set individual values inside Unity's Inspector
	public float fireRate = 2f;
	//end public Inspector variables


	private float nextTimeToFire;
	private float nextTimeToHandAttack;
	private float handAttackRate = .1f;
	private float moveRate = .055f;
	private float jumpRate = .125f;
	private float gravity;
	private float spriteHeight;
	private float spriteWidth;
	private bool isAttackingWithHands = false;
	private AIWaypointPathfinding waypointPathfinder;
	private Vector3 waypoint;
	private Vector3 velocity;
	private int playerMask;

	/// <summary>
	/// Initialize values for ShootingEnemyController
	/// </summary>
	protected override void Start(){
		base.Start();
		gravity = -Physics2D.gravity.y;
		spriteHeight = gameObject.renderer.bounds.size.y;
		spriteWidth = gameObject.renderer.bounds.size.x;
		speed = 1.3f;
		waypointPathfinder = new AIWaypointPathfinding(this.gameObject);
		waypoint = transform.position;
		playerMask = (1 << LayerMask.NameToLayer("Player"));
		jumpForce = 0;
	}

	/// <summary>
	/// Fixed Updated is called every Fixed Frame. Used to add velocity behind jumps and horizontal movement.
	/// (Requirement 3.1.1) AI Actions - Moves forward
	/// (Requirement 3.1.2) AI Actions - Moves backwards
	/// (Requirement 3.1.3) AI Actions - Jumps
	/// </summary>
	protected override void FixedUpdate(){
		base.FixedUpdate();
		if (!isDead){
			if (CanJump() && (jumpRate -= Time.fixedDeltaTime) <=0){

				if (jumpForce != float.MaxValue){
					if(!float.IsNaN(jumpForce)){
						rigidbody2D.velocity = new Vector2(0, jumpForce);
					}

					jumpForce = float.MaxValue;
	            }
				jumpRate = .125f;
	        }
			rigidbody2D.velocity = new Vector2((waypoint.x - transform.position.x) * speed, rigidbody2D.velocity.y);

	        anim.SetFloat("HorizontalSpeed", Mathf.Abs(rigidbody2D.velocity.x));
			}
        
    }

	/// <summary>
	/// Update is called once per frame. AI pathfinding and AIDetection attacking is handled here.
	/// </summary>
    protected override void Update(){
		base.Update();
		if (!isDead){
			isAttackingWithHands = false;
				if ((moveRate -= Time.deltaTime) <= 0){
					bool hasMoved = waypointPathfinder.Move(new Vector2 (gameObject.transform.position.x, gameObject.transform.position.y), aiDetection.EnemyPosition(), playerDetected);
					if (hasMoved){
						waypoint = waypointPathfinder.GetNewMoveValue();

						if (waypointPathfinder.ShouldJump() || Mathf.Abs(waypoint.y - transform.position.y) > spriteHeight*3){
							if (Mathf.Abs(waypoint.y - transform.position.y) > spriteHeight){
								float distanceToJump = (waypoint.y - transform.position.y);
								if (distanceToJump > 0 && distanceToJump < jumpDistance)
									jumpForce = Mathf.Sqrt(2 * (gravity * (distanceToJump + spriteHeight /2)));


							} else{
								float distanceToJump = Mathf.Abs(waypoint.x - transform.position.x);
								jumpForce = Mathf.Sqrt(2 * (gravity * (distanceToJump + spriteWidth/2)));     
							}

						}
						moveRate = .125f;
						
					} 
		        } 

		        
		        if ((nextTimeToHandAttack -= Time.deltaTime) <= 0 && playerDetected){
					bool canHit = aiDetection.HandToHandCombat(transform.position, directionFacing);

					if (canHit && CanJump()&& aiDetection.OnHead()){
						PunchAbove();
					} else if (canHit && CanJump()){
						Kick();
					}
					nextTimeToHandAttack = handAttackRate;
				} 

				if ((nextTimeToFire -= Time.deltaTime) <= 0){
					if(playerDetected && aiDetection.ShouldFire(transform.position))
						FireTimedWeapon();
					else if (playerDetected){
						float distanceToJump = aiDetection.JumpDistance(transform.position);
						if (distanceToJump > 0 &&  CanJump() && !aiDetection.OnHead()){
							if (distanceToJump < jumpDistance){
								jumpForce = Mathf.Sqrt(2 *(gravity * (distanceToJump + spriteHeight /2)));
								if (!float.IsInfinity(jumpForce) &&  !float.IsNaN(jumpForce)){
									anim.SetTrigger("Jump");
									gameObject.rigidbody2D.velocity = new Vector2(0, jumpForce); 
								}
							}
						}
						if (distanceToJump > 0 && CanJump()&&  aiDetection.OnHead()){
							PunchAbove();
						} 
					} 
				}
		}
	}
	/// <summary>
	/// Kicks at the Player if they are near by
	/// </summary>
	private void Kick(){
		anim.SetTrigger("Kick");
		isAttackingWithHands = true;
	}
	/// <summary>
	/// Punchs at the player if they are above their head
	/// </summary>
	private void PunchAbove(){
		float distanceToJump = aiDetection.JumpDistance(transform.position);
		if (distanceToJump < jumpDistance){
			jumpForce = Mathf.Sqrt(2 * gravity * (distanceToJump + spriteHeight /2));
			if (!float.IsInfinity(jumpForce) && !float.IsNaN(jumpForce)){
				gameObject.rigidbody2D.velocity = new Vector2(0, jumpForce);
				anim.SetTrigger("Punch");
			}
			isAttackingWithHands = true;
		}
	}
	/// <summary>
	/// Fires the weapon and resets cooldown timer.
	/// </summary>
	private void FireTimedWeapon(){
		anim.SetTrigger("Fire Weapon");
		nextTimeToFire = fireRate;
	}
	/// <summary>
	/// Calls FireWeapon in Controller
	/// </summary>
	private void CreateLaser(){
		FireWeapon(GameResources.GetGameObject(GameResources.KeyGreenSmallLaser), 10);
	}
	/// <summary>
	/// Determines whether this character can jump.
	/// </summary>
	/// <returns><c>true</c> if this instance can jump; otherwise, <c>false</c>.</returns>
	private bool CanJump(){
		//return (Physics2D.OverlapCircle(new Vector2(transform.position.x, transform.position.y - (spriteHeight/2)), .2f ,groundLayerMask) && rigidbody2D.velocity.y == 0);
		return(rigidbody2D.velocity.y == 0);
	}

	/// <summary>
	/// Determine if the player is near enough for melee combat
	/// </summary>
	/// <returns><c>true</c>, if player is near, <c>false</c> otherwise.</returns>
	private bool PlayerIsNear(){
		return (isAttackingWithHands = Physics2D.OverlapCircle(transform.position, spriteWidth * 3, playerMask));
	}

	/// <summary>
	/// Try to relocate player to waypoint transform if they hit the trigger zone. Used as a helper if character gets bad waypoint data.
	/// </summary>
	/// <param name="other">Other collider</param>
	private void OnTriggerEnter2D(Collider2D other){
		if (other.gameObject.tag == "WaypointEdge"){
			transform.position = Vector3.Lerp(transform.position,other.transform.position, speed * 14*  Time.deltaTime);
		}
	}

	/// <summary>
	/// Harm the player and use a force to remove them from characters immediate area.
	/// </summary>
	/// <param name="other">Other.</param>
    private void OnCollisionStay2D(Collision2D other){
		if (other.gameObject.tag == "Player" && isAttackingWithHands){
			other.rigidbody.AddRelativeForce(new Vector2 (1000 * directionFacing.x, .5f));
			other.gameObject.GetComponent<Controller>().DecreaseHealth(1);
			isAttackingWithHands = false;
		}
	}

}
