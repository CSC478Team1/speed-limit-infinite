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
	private int wallLayerMask;


	public AIDetection(int layerMaskDetection, float sightDistance, Transform temp, float jumpDistance){
		this.layerMaskDetection = layerMaskDetection;
		this.sightDistance = sightDistance;
		this.jumpDistance = jumpDistance;
		wallLayerMask = (1 << LayerMask.NameToLayer("Wall"));
		playerLayerMask = 1 << LayerMask.NameToLayer("Player");
		boundaryLayerMask = (1 << LayerMask.NameToLayer("Platform")) | (1 << LayerMask.NameToLayer("Default")) | (1 << LayerMask.NameToLayer("IgnorePlayer"))
							| (1 << LayerMask.NameToLayer("PlayerObject")) ;
	}
	public bool EnemyIsNear(Vector3 transform, Vector3 enemy){
		bool enemyDetected = Physics2D.OverlapCircle(transform, 15f, playerLayerMask);
		enemyPosition = enemy;

		RaycastHit2D boundaryDetection = Physics2D.Linecast(transform,enemyPosition,wallLayerMask);
		if (boundaryDetection.collider!= null)
			if (boundaryDetection.collider.gameObject.layer == LayerMask.NameToLayer("Wall"))
				enemyDetected = false;

		if (enemyDetected && !ShouldFire(transform) && enemyPosition.y > transform.y){
			RaycastHit2D detectPlatforms = Physics2D.Raycast(transform, Vector2.up, 8f, boundaryLayerMask);
			if (detectPlatforms.collider != null){
				if (detectPlatforms.transform.position.y > enemyPosition.y){
					shouldJump = true;
				}else
					shouldJump = false;
			} else
				shouldJump = true;
		}

		//float angle = Vector2.Angle(transform, enemy);
		//Vector3 angleVector = Quaternion.AngleAxis(angle, -Vector3.forward) * basicVector;


		return enemyDetected;
	}
	public Vector2 EnemyPosition(){
		return enemyPosition;
	}
	private bool GenerateRaycast(Vector3 transform,  Vector3 angle){
		RaycastHit2D playerDetection = Physics2D.CircleCast(transform, .15f , angle, sightDistance ,playerLayerMask);
		RaycastHit2D boundaryDetection = Physics2D.CircleCast(transform, .15f , angle, sightDistance ,boundaryLayerMask);
		bool safeToJump = true;
		if (boundaryDetection.collider != null && playerDetection.collider != null){
			safeToJump = boundaryDetection.collider.transform.position.y > playerDetection.transform.position.y;
		}
		if (playerDetection.collider != null && playerDetection.collider.tag == "Player" && safeToJump) {
			return true;
		} else{
			return false;
		}
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
	public bool CheckJumpPath(Vector3 transform){
		Vector3 angleNinety = Quaternion.AngleAxis(90f, Vector3.forward) * basicVector;
		return(GenerateRaycast(transform, angleNinety));
	}
}
