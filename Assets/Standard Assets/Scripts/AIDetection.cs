using UnityEngine;
using System.Collections;

///<summary> 
/// AIDetection is used for general player detection and determining if the player is within its sight distance to strike
/// in various ways.
/// </summary>
public class AIDetection {
	private float sightDistance;
	private float jumpDistance;
	private Vector2 enemyPosition;
	private bool onHead = false;
	private bool shouldJump = false;
	private int playerLayerMask;
	private int boundaryLayerMask;
	private Vector2 basicVector = new Vector2(1,0);
	private int wallLayerMask;

	/// <summary>
	/// Initializes a new instance of the <see cref="AIDetection"/> class.
	/// </summary>
	/// <param name="playerLayerMask">Player layer bitmask. Should only be that of the Player layer</param>
	/// <param name="sightDistance">Distance this character uses to detect player.</param>
	/// <param name="jumpDistance">Distance this character is allowed to jump to reach the player</param>
	public AIDetection(int playerLayerMask, float sightDistance,  float jumpDistance){
		this.sightDistance = sightDistance;
		this.jumpDistance = jumpDistance;
		this.playerLayerMask = playerLayerMask;
		wallLayerMask = (1 << LayerMask.NameToLayer("Wall"));

		boundaryLayerMask = (1 << LayerMask.NameToLayer("Platform")) | (1 << LayerMask.NameToLayer("Default")) | (1 << LayerMask.NameToLayer("IgnorePlayer"))
							| (1 << LayerMask.NameToLayer("PlayerObject")) ;
	}

	/// <summary>
	/// Detects if the player is near this character
	/// </summary>
	/// <returns><c>true</c>, if player is in range, <c>false</c> if player is out of range</returns>
	/// <param name="transform">Current character transform</param>
	/// <param name="enemy">Player Vector3 transform.position</param>
	public bool EnemyIsNear(Vector3 transform, Vector3 enemy){
		bool enemyDetected = Physics2D.OverlapCircle(transform, sightDistance, playerLayerMask);
		enemyPosition = enemy;

		RaycastHit2D boundaryDetection = Physics2D.Linecast(transform,enemyPosition,wallLayerMask);
		if (boundaryDetection.collider!= null)
			if (boundaryDetection.collider.gameObject.layer == LayerMask.NameToLayer("Wall"))
				enemyDetected = false;

		if (enemyDetected && !ShouldFire(transform) && enemyPosition.y > transform.y){
			RaycastHit2D detectPlatforms = Physics2D.Raycast(transform, Vector2.up, jumpDistance, boundaryLayerMask);
			if (detectPlatforms.collider != null){
				if (detectPlatforms.transform.position.y > enemyPosition.y){
					shouldJump = true;
				}else
					shouldJump = false;
			} else
				shouldJump = true;
		}
		return enemyDetected;
	}

	/// <summary>
	/// Gets stored player position
	/// </summary>
	/// <returns>Position of stored Vector2 location of the player</returns>
	public Vector2 EnemyPosition(){
		return enemyPosition;
	}

	/// <summary>
	/// Raycasts from location at an angle using CircleCasts
	/// </summary>
	/// <returns><c>true</c>, if player was hit with raycast with no obstacles, <c>false</c> if boundary is in the way or player not hit with raycast</returns>
	/// <param name="transform">This character's transform.</param>
	/// <param name="angle">Vector3 angle to cast raycast.</param>
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

	/// <summary>
	/// Determine if this character can use special moves on the player if they're really close
	/// </summary>
	/// <returns><c>true</c>, If player is within distance of being struck with hands or feet, <c>false</c> if player is out of range of melee combat.</returns>
	/// <param name="transform">Transform.position of the current character.</param>
	/// <param name="directionFacing">Current direction character is facing.</param>
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

	/// <summary>
	/// Distance needed to Jump
	/// </summary>
	/// <returns>Returns the distance needed to reach the player in the Y direction. Returns float.MinValue if it shouldn't jump.</returns>
	/// <param name="transform">Transform.position of the current character</param>
	public float JumpDistance(Vector3 transform){
		if (shouldJump){
			return (enemyPosition.y - transform.y);
		}
		return float.MinValue;
	}

	/// <summary>
	/// Determines if the player is currently in shooting range
	/// </summary>
	/// <returns><c>true</c>, if this character has a shot, <c>false</c> otherwise.</returns>
	/// <param name="transform">Transform.position of the current character</param>
	public bool ShouldFire(Vector3 transform){
		bool fire = false;
		if (enemyPosition.y != float.MaxValue)
			if (Mathf.Abs(transform.y - enemyPosition.y) < .22f)
				fire = true;

		return fire;
	}
	/// <summary>
	/// Returns the bool value if the player is on this characters head or near it. A jump punch is preferred in this range
	/// </summary>
	public bool OnHead(){
		return onHead;
	}
	/// <summary>
	/// Checks the jump path for obstacles.
	/// </summary>
	/// <returns><c>true</c>, if the jump path is free of obstacles, <c>false</c> if path has obstacles.</returns>
	/// <param name="transform">Transform.position of the current character.</param>
	public bool CheckJumpPath(Vector3 transform){
		Vector3 angleNinety = Quaternion.AngleAxis(90f, Vector3.forward) * basicVector;
		return(GenerateRaycast(transform, angleNinety));
	}
}
