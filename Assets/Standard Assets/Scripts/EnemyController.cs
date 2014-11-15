using UnityEngine;
using System.Collections;

public class EnemyController : Controller {

	protected int playerLayerMask;
	protected bool playerDetected = false;
	protected Vector2 directionFacing;
	protected float sightDistance = 15f;
	private bool locked = false;
	protected Vector2 targetPosition;
	protected float jumpDistance = 5f;
	private float deathAnimationTime = 0;
	protected AIDetection aiDetection;
	protected int groundLayerMask;
	
	// Use this for initialization
	protected override void Start () {
		base.Start();	
		playerLayerMask = (1 << LayerMask.NameToLayer("Player")) | (1 << LayerMask.NameToLayer("Default"));
		groundLayerMask = (1 << LayerMask.NameToLayer("Default")) | (1 << LayerMask.NameToLayer("Platform")) | (1 << LayerMask.NameToLayer("IgnorePlayer")) 
						| (1 << LayerMask.NameToLayer("PlayerObject")) ;

		deathAnimationTime = GameResources.GetAnimationClip(GameResources.KeySmallExplosionAnimation).length;

		if (isFacingRight)
			directionFacing = new Vector2(1,0);
		else
			directionFacing = new Vector2(-1,0);

		aiDetection = new AIDetection(playerLayerMask, sightDistance, transform, jumpDistance);
	}

	protected virtual void Update(){
		// move enemy here
		playerDetected = aiDetection.CanSeeEnemyInFront(transform.position, directionFacing);
		if (!playerDetected){
			playerDetected = aiDetection.CanSeeEnemyInBack(transform.position, directionFacing);
			if (playerDetected){
				targetPosition = aiDetection.EnemyPosition();
				MirrorSprite();
				directionFacing = -directionFacing;
			}
		} else
			targetPosition = aiDetection.EnemyPosition();

	}
				
	// Update is called once per frame
	protected override void FixedUpdate () {
		base.FixedUpdate();
		if(isDead && !locked){
			locked = true;
			sightDistance = 0f;
			GameObject explosion = Instantiate(GameResources.GetGameObject(GameResources.KeySmallExplosion), transform.position, Quaternion.identity) as GameObject;
			if (explosion != null){
				//bind position of gameobject to explosion
				explosion.transform.parent = gameObject.transform;
				Animator tempAnimator = explosion.GetComponent<Animator>();
				tempAnimator.SetTrigger("Explode");
				//gameObject.rigidbody2D.isKinematic = true;
				//gameObject.renderer.renderer.collider2D.isTrigger = true;
				StartCoroutine(Pause(deathAnimationTime, true, explosion, this.gameObject));
			} else
				Destroy(gameObject);

		}
	}
	
	private void Destroy(bool destroyCurrent, GameObject [] destroyObjects){
		for (int i = 0; i < destroyObjects.Length; i++){
			if (destroyObjects[i] != null)
				Destroy(destroyObjects[i]);
		}
	}

	protected IEnumerator Pause(float time, bool destroy, params GameObject[] destroyObjects){
		yield return new WaitForSeconds(time);
		if (destroy)
			Destroy(destroy, destroyObjects);
	}
}
