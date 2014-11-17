using UnityEngine;
using System.Collections;

public class ShootingEnemyController : EnemyController {

	protected float fireRate = 2f;
	private float nextTimeToFire;
	private float nextTimeToHandAttack;
	private float handAttackRate = .1f;
	private float moveRate = .1f;
	private float gravity;
	private float spriteHeight;
	private float maxJumpHeight = 8f;
	private bool isAttackingWithHands = false;
	private bool isLocked = false;
	private Vector3 move;
	//private AIBasicPathfinding pathfinder;
	private AIWaypointPathfinding waypointPathfinder;

	protected override void Start(){
		base.Start();
		gravity = -Physics2D.gravity.y;
		spriteHeight = gameObject.renderer.bounds.size.y;
		speed = 2f;
		//pathfinder = new AIBasicPathfinding(this.gameObject, 3f, groundLayerMask, speed, sightDistance);
		waypointPathfinder = new AIWaypointPathfinding(this.gameObject);
		move = transform.position;
	}
	protected override void FixedUpdate(){
		base.FixedUpdate();
		Vector3 moveDirection = Vector3.zero;

		if ((moveRate -= Time.deltaTime) <= 0){
			bool hasMoved = waypointPathfinder.Move(new Vector2 (gameObject.transform.position.x, gameObject.transform.position.y), aiDetection.EnemyPosition(), playerDetected);
			if (hasMoved){
				move = waypointPathfinder.GetNewMoveValue();
				//transform.position = Vector3.MoveTowards(transform.position, move, Time.deltaTime * speed);
				//rigidbody2D.velocity = (move - transform.position) * speed;
				//move.y = Mathf.Abs(transform.position.y - move.y) > .1f ? move.y : transform.position.y;
				//Vector2 direction = (move-transform.position).normalized;
				//rigidbody2D.velocity = direction * speed;
				//rigidbody2D.AddForce(rigidbody2D.velocity);
				//gameObject.transform.position = move;
				//gameObject.transform.position = Vector3.MoveTowards(transform.position, move, speed * Time.deltaTime);
				//anim.SetFloat("HorizontalSpeed", Mathf.Abs(rigidbody2D.velocity.x));
				//transform.position = move;
				moveRate = .1f;

			}
			isLocked = false;
		} 


			//gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, move, speed * Time.deltaTime);

		moveDirection = (move - transform.position) * speed;
		moveDirection = transform.TransformDirection(moveDirection);
		if (waypointPathfinder.ShouldJump()){
			//need to do actual physics here just a temp value
			moveDirection.y = jumpForce * .5f;
		}

		transform.Translate (moveDirection * Time.deltaTime);
	    //transform.position = new Vector3(move.x + speed, move.y);
		anim.SetFloat("HorizontalSpeed", Mathf.Abs(rigidbody2D.velocity.x));
		//transform.position = Vector3.Lerp(transform.position, move, Time.deltaTime  );


	}
	protected override void Update(){
		base.Update();
		isAttackingWithHands = false;

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
		return (Physics2D.OverlapCircle(transform.position, .5f ,groundLayerMask) && rigidbody2D.velocity.y == 0);
	}
	private void OnCollisionStay2D(Collision2D other){
		if (other.gameObject.tag == "Player" && isAttackingWithHands){
			other.rigidbody.AddRelativeForce(new Vector2 (1000 * directionFacing.x, .5f));
			other.gameObject.GetComponent<Controller>().DecreaseHealth(1);
			isAttackingWithHands = false;
		}
	}


	
}
