using UnityEngine;
using System.Collections;

public class ShootingEnemyController : EnemyController {

	protected float fireRate = 2f;
	private float nextTimeToFire;
	private float nextTimeToHandAttack;
	private float handAttackRate = .1f;
	private float moveRate = .05f;
	private float gravity;
	private float spriteHeight;
	private float spriteWidth;
	private float maxJumpHeight = 55f;
	private bool isAttackingWithHands = false;
	//private AIBasicPathfinding pathfinder;
	private AIWaypointPathfinding waypointPathfinder;
	private Vector3 waypoint;
	private Vector3 velocity;
	private Vector3 startPoint;
	private float lastSquareMagnitude;
	private bool isMoving = false;

	protected override void Start(){
		base.Start();
		gravity = -Physics2D.gravity.y;
		spriteHeight = gameObject.renderer.bounds.size.y;
		spriteWidth = gameObject.renderer.bounds.size.x;
		speed = 3f;
		//pathfinder = new AIBasicPathfinding(this.gameObject, 3f, groundLayerMask, speed, sightDistance);
		waypointPathfinder = new AIWaypointPathfinding(this.gameObject);
		waypoint = startPoint = transform.position;
		lastSquareMagnitude = Mathf.Infinity;
		jumpForce = 0;
	}
	protected override void FixedUpdate(){
		base.FixedUpdate();


        
    }
    protected override void Update(){
		base.Update();
		isAttackingWithHands = false;
		float squareMagnitude =  (waypoint - transform.position).sqrMagnitude;

		if ((moveRate -= Time.deltaTime) <= 0){
			bool hasMoved = waypointPathfinder.Move(new Vector2 (gameObject.transform.position.x, gameObject.transform.position.y), aiDetection.EnemyPosition(), playerDetected);
			if (hasMoved){
				startPoint = waypoint;
				waypoint = waypointPathfinder.GetNewMoveValue();

				anim.SetFloat("HorizontalSpeed", 1f);

				//velocity = (waypoint - transform.position).normalized * speed; 

				if (waypointPathfinder.ShouldJump() || Mathf.Abs(waypoint.y - transform.position.y) > spriteHeight){
					if (Mathf.Abs(waypoint.y - transform.position.y) > spriteHeight){
						float distanceToJump = (waypoint.y - transform.position.y);
						jumpForce = Mathf.Sqrt(2 * gravity * (distanceToJump + spriteHeight /2));
						if (float.IsInfinity(jumpForce) || float.IsNaN(jumpForce)){
							jumpForce = 0;
                        }

					} else{
						float distanceToJump = (waypoint.x - transform.position.x);
						jumpForce = Mathf.Sqrt(2 * gravity * (distanceToJump + spriteWidth  /2));
						if (float.IsInfinity(jumpForce) || float.IsNaN(jumpForce)){
							jumpForce = 0;
                        }         
					}

				}else
				//velocity.y = 0f;
				isMoving = true;
				moveRate = .05f;
				
			} 
        } 
		if (isMoving){

			if (jumpForce != 0){
				//Debug.Log("jump! " + jumpForce.ToString());
				if(!float.IsNaN(jumpForce))
					rigidbody2D.AddForce(new Vector2(0f, jumpForce));
				jumpForce = 0;
			}else
		
			if (CanJump())
				transform.position = Vector3.MoveTowards(transform.position, waypoint, speed * Time.deltaTime);
		}

		if (squareMagnitude > lastSquareMagnitude || transform.position == waypoint){
			isMoving = false;
			anim.SetFloat("HorizontalSpeed", 0f);
		}
		lastSquareMagnitude = squareMagnitude;
        
        if ((nextTimeToHandAttack -= Time.deltaTime) <= 0 && playerDetected){
			bool canHit = aiDetection.HandToHandCombat(transform.position, directionFacing);
			if (canHit && CanJump()&& aiDetection.OnHead()){
				PunchAbove();
			} else if (canHit && CanJump() && !aiDetection.OnHead()){
				Kick();
			}
		} else if (nextTimeToHandAttack < float.MinValue + 100)
			nextTimeToHandAttack = 0;

		if ((nextTimeToFire -= Time.deltaTime) <= 0){
			if(playerDetected && aiDetection.ShouldFire(transform.position))
				FireTimedWeapon();
			else if (playerDetected){
				float distanceToJump = aiDetection.JumpDistance(transform.position);
				if (distanceToJump > 0 &&  CanJump() && !aiDetection.OnHead()){
					//make enemy jump here animation
					if (distanceToJump < maxJumpHeight){
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
			} else if (nextTimeToFire < float.MinValue + 100)
				nextTimeToFire = 0;
		}

	}
	private void Kick(){
		anim.SetTrigger("Kick");
		nextTimeToHandAttack = handAttackRate;
		isAttackingWithHands = true;
	}
	private void PunchAbove(){
		float distanceToJump = aiDetection.JumpDistance(transform.position);
		if (distanceToJump < maxJumpHeight){
			jumpForce = Mathf.Sqrt(2 * gravity * (distanceToJump + spriteHeight /2));
			if (!float.IsInfinity(jumpForce) && !float.IsNaN(jumpForce)){
				gameObject.rigidbody2D.velocity = new Vector2(0, jumpForce);
				anim.SetTrigger("Punch");
			}
			isAttackingWithHands = true;
			nextTimeToHandAttack = handAttackRate;
		}
	}
	private void FireTimedWeapon(){
		anim.SetTrigger("Fire Weapon");
		nextTimeToFire = fireRate;
	}
	private void CreateLaser(){
		FireWeapon(GameResources.GetGameObject(GameResources.KeyGreenSmallLaser), 10);

	}
	private bool CanJump(){
		return (Physics2D.OverlapCircle(new Vector2(transform.position.x, transform.position.y - (spriteHeight/2)), .3f ,groundLayerMask) && rigidbody2D.velocity.y == 0);
	}
	private void OnTriggerEnter2D(Collider2D other){
		if (other.gameObject.tag == "WaypointEdge"){
			transform.position = Vector3.Lerp(transform.position,other.transform.position, speed * 14*  Time.deltaTime);
		}
	}
	private void OnTriggerStay2D(Collider2D other){
		if (other.gameObject.tag == "Waypoint"){
			anim.SetFloat("HorizontalSpeed", 0);   
        }
    }
    private void OnCollisionStay2D(Collision2D other){
		if (other.gameObject.tag == "Player" && isAttackingWithHands){
			other.rigidbody.AddRelativeForce(new Vector2 (1000 * directionFacing.x, .5f));
			other.gameObject.GetComponent<Controller>().DecreaseHealth(1);
			isAttackingWithHands = false;
		}
	}


	
}
