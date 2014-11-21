using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AIBasicPathfinding  {

	private Vector2 position;
	private Vector2 enemyPosition = Vector2.zero;
	private float distanceToKeep;
	private int groundCollisionLayerMask;
	private GameObject gameObject;
	private float gravity;
	private float spriteHeight;
	private float spriteWidth;
	private List<Paths> move = new List<Paths>();
	private float distanceXToPlayer;
	private float distanceYToPlayer;
	private Paths path;
	private float speed;
	private float sightDistance;
	private int pathCollisionLayerMask;


	private struct Paths{
		public Vector2 position { get; set;}
		public int cost { get; set;}
	}

	public AIBasicPathfinding(GameObject gameObject, float distanceToKeep, int groundCollisionLayerMask, float speed, float sightDistance){
		this.gameObject = gameObject;
		this.distanceToKeep = distanceToKeep;
		this.groundCollisionLayerMask = groundCollisionLayerMask;
		pathCollisionLayerMask = groundCollisionLayerMask | (1 << LayerMask.NameToLayer("Enemy"));
		this.speed = speed;
		this.sightDistance = sightDistance;
		gravity = -Physics2D.gravity.y;
		spriteHeight = gameObject.renderer.bounds.size.y;
		spriteWidth = gameObject.renderer.bounds.size.x;

	}

	public Vector2 Move(Vector2 position, Vector2 enemyPosition){
		this.position = position;
		this.enemyPosition = enemyPosition;
		if (enemyPosition != Vector2.zero && (Mathf.Abs(enemyPosition.x - position.x) < sightDistance)){
			distanceXToPlayer = position.x - enemyPosition.x;
			distanceYToPlayer = position.y - enemyPosition.y;
			if (distanceXToPlayer < 0 && (distanceYToPlayer < .25f && distanceYToPlayer > -.25f)){
				MoveRight();
			}
			if (distanceXToPlayer > 0 && (distanceYToPlayer < .25f && distanceYToPlayer > -.25f)){
				MoveLeft();
			}
			if (distanceXToPlayer < 0 && (distanceYToPlayer < -.5f)){
				MoveUp(true);
			}
			if (distanceXToPlayer > 0 && (distanceYToPlayer < -.5f)){
				MoveUp(false);
			}
			if (distanceXToPlayer < 0 && (distanceYToPlayer > .2f)){
				MoveDown(true);
			}
			if (distanceXToPlayer > 0 && (distanceYToPlayer > .2f)){
				MoveDown(false);
			}
		}else{
			//idle wander
			if (MoveLeft()){
				// move left
			}else if (MoveRight()){
				//move right
			}
		}
		return (MoveObject());
	}
	private Vector2 MoveObject(){
		if (move.Count > 0){
			move.Sort((Paths x, Paths y)=>x.cost.CompareTo(y.cost));
			float moveX = (move[0].position.x - position.x) ;

			float moveY = Mathf.Abs(move[0].position.y - position.y);
			float directionX = (move[0].position.x > position.x) ? 1f : -1f;
			float directionY = (move[0].position.y > position.y) ? 1f : -1f;  // >= causes some weird jumping - it's funny actually
			float jumpForce = 0f;
			if (directionY == 1f){
				jumpForce = Mathf.Sqrt(2 *(gravity * (moveY + spriteHeight /2)));
				if (float.IsInfinity(jumpForce) || float.IsNaN(jumpForce))
					jumpForce = 0f;
			}
			//Debug.Log((directionX * speed).ToString());
			move.Clear();
			//gameObject.transform.position += new Vector3(directionX, directionY, 0) * speed * Time.deltaTime;
			return new Vector2(speed * directionX , jumpForce);
		}
		return new Vector2(0,0);
	}

	private bool MoveXAxis(bool moveRight){
		bool canMove = false;
		if (distanceToKeep < Mathf.Abs(enemyPosition.x - position.x) && CanMove()){
			int direction = moveRight ? 1 : -1;
			Vector2 nextLocation = new Vector2(position.x + (spriteWidth * direction), position.y);
			RaycastHit2D pathDetection = Physics2D.Raycast(nextLocation, new Vector2(Vector2.right.x * direction, 0), spriteWidth, pathCollisionLayerMask);
			if (pathDetection.collider == null){
				RaycastHit2D edgeDetection = Physics2D.Raycast(nextLocation, -Vector2.up, spriteHeight, groundCollisionLayerMask);
				if (edgeDetection.collider != null){
					path.position = nextLocation;
					path.cost = 1;
					move.Add(path);
					canMove = true;
				}
			}
		}
		return canMove;
	}
	private bool MoveLeft(){
		return(MoveXAxis(false));
	}
	private bool MoveRight(){
		return (MoveXAxis(true));
	}
	private bool MoveDown(bool moveRight){
		bool canMove = MoveXAxis(moveRight);
		if (!canMove && CanMove()){
			int direction = moveRight ? 1 : -1;
			Vector2 nextLocation = new Vector2(position.x + (spriteWidth * direction), position.y);
			RaycastHit2D pathDetection = Physics2D.Raycast(nextLocation, new Vector2(Vector2.right.x * direction, 0), spriteWidth, pathCollisionLayerMask);
			if (pathDetection.collider == null){
				int current = 0, end = 10;
				while (++current < end){
					RaycastHit2D canFall = Physics2D.Raycast(nextLocation, -Vector2.up, 10f, groundCollisionLayerMask);
					if (canFall.collider != null){
						path.position = nextLocation;
						path.cost = 20 - current;
						move.Add(path);
						canMove = true;
					}
					nextLocation.x += (spriteWidth  * direction);
				}
			}
		}
		
		return canMove;
	}
	private bool MoveUp(bool moveRight){
		bool canMove = MoveXAxis(moveRight);
		if (!canMove && CanMove()){
			int direction = moveRight ? 1: -1;
			int current = 0, end = 3, loop =1;;
			while (++current < end){
				Vector2 nextLocation = new Vector2(position.x + (spriteWidth * direction * current), position.y);
				RaycastHit2D canJump = Physics2D.Raycast(nextLocation, Vector2.up, 10f, groundCollisionLayerMask);
				RaycastHit2D canStand = Physics2D.Raycast(nextLocation, -Vector2.up, .9f, groundCollisionLayerMask);
				//RaycastHit2D canJumpOnPlatformLeft = Physics2D.Raycast(new Vector2(nextLocation.x - spriteWidth , nextLocation.y), -Vector2.up, enemyPosition.y - position.y + spriteHeight, groundCollisionLayerMask);
				RaycastHit2D canJumpOnPlatformRight = Physics2D.Raycast(new Vector2(nextLocation.x + spriteWidth , nextLocation.y), -Vector2.up, enemyPosition.y - position.y + spriteHeight, groundCollisionLayerMask);
				if (canJump.collider == null){
					path.cost = current;
				}
				if (canStand.collider == null)
						path.cost = 1000;

				//if (canJumpOnPlatformLeft.collider != null)
			//			path.cost = 1;
			//	else if (canJumpOnPlatformRight.collider !=null)
			//			path.cost = 1 ;
				path.position = nextLocation;
				move.Add(path);
				canMove = true;

					nextLocation.x += spriteWidth * direction * current;

				if (current +1 == end && loop > 0){
					loop --;
					direction *= -1;
					current = 0;
				}

				}

			}

			//move.Sort((Paths x, Paths y)=>x.cost.CompareTo(y.cost));
			//gameObject.transform.Translate( new Vector3 ( (move[0].position.x - position.x) * speed, (move[0].position.y - position.x) * speed, gameObject.transform.position.z));

		
		return canMove;
	}
	private bool CanMove(){
		return (Physics2D.OverlapCircle(position, .5f, groundCollisionLayerMask));
	}

}
