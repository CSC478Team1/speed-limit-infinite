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
						| (1 << LayerMask.NameToLayer("IgnorePlayer"));

		speed = 3f;

		appearTime = GameResources.GetAnimationClip(GameResources.KeyBombBotAppearAnimation).length;
		Debug.Log(appearTime.ToString());

	}

	protected override void Update(){
		base.Update();

		if (playerDetected){
			RaycastHit2D playerDetection = Physics2D.Raycast (transform.position, directionFacing, sightDistance, playerLayerMask);
			if (playerDetection.collider != null && appearTime <=0)
				targetDistance = playerDetection.distance;
		}
		//should be facing player if it is detected
		if (playerDetected && appearTime <= 0 && !isLocked && !isDead){
			if (targetDistance <= attackDistance){
				isLocked = true;
				Detonate();
			} else{
				Vector2 nextLocation = new Vector2((transform.position.x + (enemyWidth))*directionFacing.x, transform.position.y);
				RaycastHit2D edgeDetection = Physics2D.Raycast(nextLocation, -Vector2.up, enemyHeight, edgeBitmask);
				if (edgeDetection.collider != null){
					if (directionFacing.x > 0){
						transform.position += transform.right*speed*Time.deltaTime;
					}
					else{
						transform.position += -transform.right*speed*Time.deltaTime;
					}
					anim.SetFloat("Speed", 1);
				} else
					anim.SetFloat("Speed", 0);
			}
		} else if (appearTime > 0)
			appearTime -= Time.deltaTime;
		 else if (isDead || !playerDetected)
			anim.SetFloat("Speed", 0);


	}

	private void Detonate(){
		GameObject detonationObject = Instantiate(GameResources.GetGameObject(GameResources.KeyLargeExplosion), transform.position, Quaternion.identity) as GameObject;
		float detonationTime = GameResources.GetAnimationClip(GameResources.KeyLargeExplosionAnimation).length;
		Animator tempAnimator = detonationObject.GetComponent<Animator>();
		tempAnimator.SetTrigger("Explode");
		StartCoroutine(Pause(detonationTime, true, detonationObject, gameObject));
	}

}
