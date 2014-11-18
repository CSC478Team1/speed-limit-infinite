using UnityEngine;
using System.Collections;

public class AIDetection {
	private int layerMaskDetection;
	private float sightDistance;
	private float jumpDistance;
	private Vector2 enemyPosition;
	private bool onHead = false;
	private bool shouldFire = false;
	private bool shouldJump = false;
	private int playerLayerMask;
	private int boundaryLayerMask;
	private Vector2 basicVector = new Vector2(1,0);


	public AIDetection(int layerMaskDetection, float sightDistance, Transform temp, float jumpDistance){
		this.layerMaskDetection = layerMaskDetection;
		this.sightDistance = sightDistance;
		this.jumpDistance = jumpDistance;
		playerLayerMask = 1 << LayerMask.NameToLayer("Player");
		boundaryLayerMask = (1 << LayerMask.NameToLayer("Platform")) | (1 << LayerMask.NameToLayer("Default")) | (1 << LayerMask.NameToLayer("IgnorePlayer"))
							| (1 << LayerMask.NameToLayer("PlayerObject")) ;
	}
	public bool CanSeeEnemyInFront(Vector3 transform, Vector2 directionFacing){
		return (CanSeeEnemy(transform, directionFacing));
	}
	public bool CanSeeEnemyInBack(Vector3 transform, Vector2 directionFacing){
		return (CanSeeEnemy(transform, -directionFacing));
	}
	public bool EnemyIsNear(Vector3 transform){
		return (Physics2D.OverlapCircle(transform, 10f, playerLayerMask));
	}
	public Vector2 EnemyPosition(){
		return enemyPosition;
	}
	private bool GenerateRaycast(Vector3 transform,  Vector3 angle){
		RaycastHit2D playerDetection = Physics2D.CircleCast(transform, .15f , angle, sightDistance ,playerLayerMask);
		if (playerDetection.collider != null && playerDetection.collider.tag == "Player") {
			enemyPosition = playerDetection.transform.position;
			return true;
		} else{
			return false;
		}
	}
	private bool CanSeeEnemy(Vector3 transform, Vector2 directionFacing){
		bool enemyDetected = false;
		shouldJump = false;


		Vector3 angleUp = Quaternion.AngleAxis(15f, Vector3.forward) * directionFacing;
		Vector3 angleUp2 = Quaternion.AngleAxis(30f, Vector3.forward)* directionFacing;
		Vector3 angleUp3 = Quaternion.AngleAxis(45f, Vector3.forward) * directionFacing;
		Vector3 angleUp4 = Quaternion.AngleAxis(60f, Vector3.forward) * directionFacing;
		Vector3 angleUp5 = Quaternion.AngleAxis(75f, Vector3.forward) * directionFacing;
		Vector3 angleDown = Quaternion.AngleAxis(-15f, Vector3.forward)* directionFacing;
		Vector3 angleDown2 = Quaternion.AngleAxis(-30f, Vector3.forward) * directionFacing;
		Vector3 angleDown3 = Quaternion.AngleAxis(-45f, Vector3.forward) * directionFacing;
		Vector3 angleDown4 = Quaternion.AngleAxis(-60f, Vector3.forward) * directionFacing;
		Vector3 angleDown5 = Quaternion.AngleAxis(-75f, Vector3.forward) * directionFacing;
		Vector3 angleZero = Quaternion.AngleAxis(0f, Vector3.forward) * directionFacing;
		Vector3 angleUpNinety = Quaternion.AngleAxis(90f, Vector3.forward) * directionFacing;
		Vector3 angleDownNinety = Quaternion.AngleAxis(-90f, Vector3.forward) * directionFacing;


		enemyDetected = GenerateRaycast(transform, angleZero);

		if(!enemyDetected){
			enemyDetected = GenerateRaycast(transform, angleUpNinety) || GenerateRaycast(transform, angleDownNinety);
			if (!enemyDetected){
				enemyDetected = GenerateRaycast(transform, angleUp) || GenerateRaycast(transform, angleUp2) || GenerateRaycast(transform, angleUp3) || GenerateRaycast(transform, angleUp4) || GenerateRaycast(transform, angleUp5);
				if (!enemyDetected){
					enemyDetected = GenerateRaycast(transform, angleDown) || GenerateRaycast(transform, angleDown2) || GenerateRaycast(transform, angleDown3) || GenerateRaycast(transform, angleDown4) || GenerateRaycast(transform, angleDown5);
				}
			}
		}
			
		if (enemyDetected && !ShouldFire(transform) && enemyPosition.y > transform.y){
			RaycastHit2D detectPlatforms = Physics2D.Raycast(transform, Vector2.up, 10f, boundaryLayerMask);
			if (detectPlatforms.collider != null){
				if (detectPlatforms.transform.position.y > enemyPosition.y){
					shouldJump = true;
				}
			} else
				shouldJump = true;
		}
		return enemyDetected;
	}
	public bool HandToHandCombat(Vector3 transform, Vector2 directionFacing){
		bool canHit = false;
		onHead = false;
		Vector3 angleNinety = Quaternion.AngleAxis(90f, Vector3.forward) * basicVector;

		RaycastHit2D ray = Physics2D.Raycast(new Vector2(transform.x,transform.y),directionFacing, .35f, playerLayerMask | boundaryLayerMask);
		if (ray.collider != null)
			if (ray.collider.tag == "Player")
				canHit = true;

		if(!canHit){
			bool enemyDetected = GenerateRaycast(transform, angleNinety);
			if (enemyDetected){
				if (enemyPosition.y > transform.y){
					onHead = true;
					canHit = true;
				}
			} 
		}
		return canHit;
	}
	public float JumpDistance(Vector3 transform){
		if (shouldJump){
			return (enemyPosition.y - transform.y);
		}
		return float.MinValue;
	}
	public bool ShouldFire(Vector3 transform){
		bool fire = false;
		if (enemyPosition.y != float.MaxValue)
			if (Mathf.Abs(transform.y - enemyPosition.y) < .22f)
				fire = true;

		return fire;
	}
	public bool OnHead(){
		return onHead;
	}
}
