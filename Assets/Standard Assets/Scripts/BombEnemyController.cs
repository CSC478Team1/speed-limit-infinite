using UnityEngine;
using System.Collections;

public class BombEnemyController : EnemyController {

	private float enemyHeight;
	private float enemyWidth;
	private float attackDistance = 1.5f;
	private int edgeBitmask;
	private float appearTime = 0f;
	private bool isLocked = false;

	// Use this for initialization
	protected override void Start () {
		base.Start();

		enemyHeight = gameObject.renderer.bounds.size.y;
		enemyWidth = gameObject.renderer.bounds.size.x;
		edgeBitmask = (1 << LayerMask.NameToLayer("Default")) | (1 << LayerMask.NameToLayer("Platform"))
						| (1 << LayerMask.NameToLayer("IgnorePlayer")) | (1 << LayerMask.NameToLayer("PlayerObject"));

		speed = 3f;
		appearTime = GameResources.GetAnimationClip(GameResources.KeyBombBotAppearAnimation).length;

	}

	protected override void Update(){
		base.Update();
		
		//should be facing player if it is detected
		if (playerDetected && appearTime <= 0 && !isLocked && !isDead){
			//if player is in attack radius blow up!
			if (Mathf.Abs(transform.position.x - targetPosition.x) < attackDistance && Mathf.Abs(transform.position.y - targetPosition.y) < attackDistance){
				isLocked = true;
				Detonate();
			} else{
				Vector2 nextLocation = new Vector2(transform.position.x + (enemyWidth*directionFacing.x), transform.position.y);
				RaycastHit2D edgeDetection = Physics2D.Raycast(nextLocation, -Vector2.up, enemyHeight, edgeBitmask);
				if (edgeDetection.collider != null){
					Move();
				} else{
					anim.SetFloat("Speed", 0);
					RaycastHit2D canFall = Physics2D.Raycast(nextLocation, -Vector2.up, 15f, edgeBitmask);
					if (canFall.collider != null){
							Move();
					}
				}
			}
		} else if (appearTime > 0)
			appearTime -= Time.deltaTime;
		 else if (isDead || !playerDetected)
			anim.SetFloat("Speed", 0);


	}
	private bool CanMove(){
		return (Physics2D.OverlapCircle(transform.position, .5f, edgeBitmask));
	}
	private void Move(){
		if (CanMove()){
			if (directionFacing.x > 0){
				//transform.position += transform.right*speed*Time.deltaTime;
				rigidbody2D.velocity = new Vector2(Vector2.right.x * speed, 0);
			}
			else{
				rigidbody2D.velocity = new Vector2(-Vector2.right.x * speed, 0);
				//transform.position += -transform.right*speed*Time.deltaTime;
			}
			anim.SetFloat("Speed", 1);
		}
	}
	private void Detonate(){
		GameObject detonationObject = Instantiate(GameResources.GetGameObject(GameResources.KeyLargeExplosion), transform.position, Quaternion.identity) as GameObject;
		detonationObject.transform.parent = gameObject.transform;
		float detonationTime = GameResources.GetAnimationClip(GameResources.KeyLargeExplosionAnimation).length;
		Animator tempAnimator = detonationObject.GetComponent<Animator>();
		tempAnimator.SetTrigger("Explode");
		StartCoroutine(Pause(detonationTime, true, detonationObject, gameObject));
	}

}
