using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// AI waypoint pathfinding using raycasts to detect waypoints. Only checks next path and does not check all paths.
/// </summary>

public class AIWaypointPathfinding {

	private int groundCollisionLayerMask;
	private int waypointMask;
	private Vector2 position;
	private Vector2 enemyPosition;
	private float spriteHeight;
	private float spriteWidth;
	private float waypointRadiusoffset = .85f; // made it a little bigger than actual radius so raycast doesn't terminate in outer edges
	private Vector3 oldMoveValue;
	private Vector3 newMoveValue;
	private bool shouldJump = false;

	/// <summary>
	/// Initializes a new instance of the <see cref="AIWaypointPathfinding"/> class.
	/// </summary>
	/// <param name="gameObject">Game object reference of current game object</param>
	public AIWaypointPathfinding(GameObject gameObject){
		waypointMask = 1 << LayerMask.NameToLayer("Waypoint") | (1 << LayerMask.NameToLayer("WaypointEdge"));
		spriteHeight = gameObject.renderer.bounds.size.y;
		spriteWidth = gameObject.renderer.bounds.size.x;
		groundCollisionLayerMask = (1 << LayerMask.NameToLayer("Default")) | (1<< LayerMask.NameToLayer("Platform")) | (1 << LayerMask.NameToLayer("Player"));
	}

	/// <summary>
	/// Determine if the current position is the ideal location or if the character should move
	/// </summary>
	/// <returns><c>true</c>, if a move is required <c>false</c> if current position is ideal location</returns>
	/// <param name="position">Current Vector2 location of this character</param>
	/// <param name="enemyPosition">Vector2 location of the Player</param>
	/// <param name="playerDetected">If set to <c>true</c> player detected.</param>
	public bool Move(Vector2 position, Vector2 enemyPosition, bool playerDetected){
		Vector3 move = Vector3.zero;
		this.position = position;
		this.enemyPosition = enemyPosition;
		if (playerDetected){
			float distanceXToPlayer = position.x - enemyPosition.x;
			float distanceYToPlayer = position.y - enemyPosition.y;

			if (distanceXToPlayer < 0 && (distanceYToPlayer < .15f && distanceYToPlayer > -.15f)){
				move = MoveXAxis(position, true);
			}
			else if (distanceXToPlayer > 0 && (distanceYToPlayer < .15f && distanceYToPlayer > -.15f)){
				move = MoveXAxis(position, false);
			}
			else if (distanceXToPlayer < 0 && (distanceYToPlayer < -.15f || distanceYToPlayer > .15f)){
				move = MoveYAxis(position, true);
			}
			else if (distanceXToPlayer > 0 && (distanceYToPlayer < -.15f || distanceYToPlayer > .15f)){
				move = MoveYAxis(position, false);
			}
		} else { // patrol!
		}
		if (move != Vector3.zero){
			newMoveValue = move;
		}
		return (move != Vector3.zero);
	}

	/// <summary>
	/// Checks if character can move only in X axis to reach player
	/// </summary>
	/// <returns>Preferred Vector3 location to move to</returns>
	/// <param name="start">Origin Vector3 location of character</param>
	/// <param name="lookRight">If set to <c>true</c> look right.</param>
	private Vector3 MoveXAxis(Vector3 start, bool lookRight){
		Vector3 move = newMoveValue;
		if (CanMove()){
			float direction = lookRight ? 1f : -1f;
			Vector2 nextLocation = new Vector2(start.x + ((waypointRadiusoffset)*direction), start.y);
			RaycastHit2D waypointDetection = Physics2D.Raycast(nextLocation, new Vector2(direction, 0), 10f, waypointMask);
			if (waypointDetection.collider != null){
				RaycastHit2D pathDetection = Physics2D.Raycast(nextLocation, new Vector2(Vector2.right.x * direction, 0), waypointDetection.distance, groundCollisionLayerMask);
				if (pathDetection.collider == null){
					move = waypointDetection.collider.transform.gameObject.transform.position;
					CheckForJump(position, move);
				}
			}
 
		}
		return move;
	}

	/// <summary>
	/// Checks if the character can move in Y Axis to reach the player
	/// </summary>
	/// <returns>Preferred Vector3 location to move to</returns>
	/// <param name="start">Origin Vector3 location of character</param>
	/// <param name="lookRight">If set to <c>true</c> look right.</param>
	private Vector3 MoveYAxis (Vector3 start, bool lookRight){
		Vector3 move = MoveXAxis(start, lookRight);
		//move = DeterminePath(move, MoveXAxis(!lookRight);
		Vector3 temp = move;
		if (CanMove()){
			List<Vector3> nextNodes = new List<Vector3>();

			float direction = enemyPosition.y > start.y ? 1f : -1f;
			Vector2 nextLocation = new Vector2 (start.x , start.y + ((waypointRadiusoffset) * direction));
			Vector3 nextLocationX = nextLocation;
			bool waypointLoop = true;
			int counter = -4;
			while (waypointLoop){
				RaycastHit2D waypointDetection = Physics2D.Raycast(nextLocation, new Vector2(0f, direction), 10f, waypointMask);
				if (waypointDetection.collider != null){
					RaycastHit2D pathDetection = Physics2D.Raycast(nextLocation, new Vector2(0f, direction), waypointDetection.distance, groundCollisionLayerMask);
					if (pathDetection.collider == null){
						nextNodes.Add(waypointDetection.collider.transform.gameObject.transform.position);
					}
				}

				nextNodes.Add(MoveXAxis(nextLocationX, lookRight));
				nextLocation.x += direction * (spriteWidth) * counter;
				nextLocationX.y += direction * (spriteHeight) * counter;
				//Debug.Log(nextLocationX.ToString());
				if (++counter > 4)
					waypointLoop = false;
			}
			if (nextNodes.Count > 0){
				if (nextNodes.Count == 1){
					move = nextNodes[0];
				} else{
					foreach (Vector3 path in nextNodes){
						move = DeterminePath(move, path);
					}
				}
			}
		}

		if (temp != move){
			//calculate which path is best for reaching the player
			move = DeterminePath(temp, move);
		}
		return move;
	}
	/// <summary>
	/// Checks for ceiling. Adds cost to path if ceiling is hit.
	/// </summary>
	/// <returns>Obstacle cost if it exists. Returns 0 if no obstacle</returns>
	/// <param name="path">The Vector3 location of the path to test</param>
	//check to see if the player is reachable for this waypoint if not add a high cost to move them along
	private float CheckForCeiling(Vector3 path){
		Vector2 direction = enemyPosition.y > position.y ? Vector2.up : -Vector2.up;
		float obstacleCost = 100f;
		RaycastHit2D raycastUnderPlatform = Physics2D.Raycast(path, direction, Mathf.Abs(enemyPosition.y - path.y), groundCollisionLayerMask);
		if (raycastUnderPlatform.collider != null){
			if (raycastUnderPlatform.distance < Mathf.Abs(enemyPosition.y - path.y))
				return obstacleCost;    
        }
		return 0f;
	}
	/// <summary>
	/// Determines the path to take based on total cost.
	/// </summary>
	/// <returns>The suggested path to reach the player.</returns>
	/// <param name="pathOne">First path to consider</param>
	/// <param name="pathTwo">Second path to consider</param>
 	private Vector3 DeterminePath(Vector3 pathOne, Vector3 pathTwo){
		Vector3 move;
		float pathOnePoints = 0;
		float pathTwoPoints = 0;
		float pathOneYPos = Mathf.Abs(Mathf.Abs(pathOne.y) - Mathf.Abs(enemyPosition.y));
		float pathTwoYPos = Mathf.Abs(Mathf.Abs(pathTwo.y) - Mathf.Abs(enemyPosition.y));
		float pathOneXPos = Mathf.Abs(Mathf.Abs(pathOne.x) - Mathf.Abs(enemyPosition.x));
		float pathTwoXPos = Mathf.Abs(Mathf.Abs(pathTwo.x) - Mathf.Abs(enemyPosition.x));

		float characterCostOneY = Mathf.Abs(Mathf.Abs(pathOne.y - Mathf.Abs(position.y)));
		float characterCostOneX = Mathf.Abs(Mathf.Abs(pathOne.x - Mathf.Abs(position.x)));
		float characterCostTwoY = Mathf.Abs(Mathf.Abs(pathTwo.y - Mathf.Abs(position.y)));
		float characterCostTwoX = Mathf.Abs(Mathf.Abs(pathTwo.x - Mathf.Abs(position.x)));

		pathOnePoints = pathOneYPos + pathOneXPos + characterCostOneX + characterCostOneY +  CheckForCeiling(pathOne);
		pathTwoPoints = pathTwoYPos + pathTwoXPos + characterCostTwoX + characterCostTwoY +  CheckForCeiling(pathTwo);

		move = pathOnePoints < pathTwoPoints ? pathOne : pathTwo;
        return move;
	}

	/// <summary>
	/// Checks path to determine if a jump is required to reach the player
	/// </summary>
	/// <param name="pointA">Starting location</param>
	/// <param name="pointB">Player location</param>
	private void CheckForJump(Vector3 pointA, Vector3 pointB){
		float direction = pointA.x < pointB.x ? 1f : -1f;
		Vector2 nextLocation = new Vector2(pointA.x + (spriteWidth*direction), pointA.y);
		RaycastHit2D raycastCheckForGap = Physics2D.Raycast(nextLocation, -Vector2.up, .6f, groundCollisionLayerMask);
		shouldJump = raycastCheckForGap.collider == null;
	}

	public bool ShouldJump(){
		return shouldJump;
	}
	public Vector3 GetNewMoveValue(){
		return newMoveValue;
	}

	/// <summary>
	/// Determines whether this character can move or if it is falling or currently jumping.
	/// </summary>
	/// <returns><c>true</c> if this character can move; otherwise, <c>false</c>.</returns>
	private bool CanMove(){
		return (Physics2D.OverlapCircle(position, .5f, groundCollisionLayerMask ));
	}
	/**
	 * testing total waypoint detection with the intention of bulding a path to reach player
	public void Test(Transform transform, Vector2 directionFacing){
		RaycastHit2D [] hits;
		Vector3 path = transform.position;
		hits = Physics2D.RaycastAll(transform.position, directionFacing, 12f);
		foreach (RaycastHit2D hit in hits){
			if (hit.collider != null){
				path = DeterminePath(hit.collider.transform.position, path);
			}
		}
		Debug.Log("my path " +  path.ToString());
	}
	**/
}
