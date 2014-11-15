using UnityEngine;
using System.Collections;

public class ShootingEnemyController : EnemyController {

	protected float fireRate = 2f;
	private float nextTimeToFire;
	private float nextTimeToHandAttack;
	private float handAttackRate = .1f;
	private float gravity;
	private float spriteHeight;
	private float maxJumpHeight = 8f;
	private bool isAttackingWithHands = false;

	protected override void Start(){
		base.Start();
		gravity = -Physics2D.gravity.y;
		spriteHeight = gameObject.renderer.bounds.size.y;
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
				if (distanceToJump > 0 && distanceToJump != float.MinValue &&  CanJump() && !aiDetection.OnHead()){
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
		return (Physics2D.OverlapCircle(transform.position, .41f ,groundLayerMask));
	}
	private void OnCollisionStay2D(Collision2D other){
		if (other.gameObject.tag == "Player" && isAttackingWithHands){
			other.rigidbody.AddRelativeForce(new Vector2 (1000 * directionFacing.x, .5f));
			other.gameObject.GetComponent<Controller>().DecreaseHealth(1);
			isAttackingWithHands = false;
		}
	}

	
}
