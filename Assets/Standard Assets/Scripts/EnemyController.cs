using UnityEngine;
using System.Collections;

public class EnemyController : Controller {

	protected int playerLayerMask;
	protected bool playerDetected = false;
	protected Vector2 directionFacing;
	protected float sightDistance = 8f;
	private float timeToWaitForDetection = .05f;
	private bool locked = false;
	protected Vector2 targetPosition;
	protected float jumpDistance = 5f;
	private float deathAnimationTime = 0;
	protected AIDetection aiDetection;
	protected int groundLayerMask;
	protected GameObject player;
	
	// Use this for initialization
	protected override void Start () {
		base.Start();	
		playerLayerMask = (1 << LayerMask.NameToLayer("Player")) | (1 << LayerMask.NameToLayer("Default"));
		groundLayerMask = (1 << LayerMask.NameToLayer("Default")) | (1 << LayerMask.NameToLayer("Platform")) | (1 << LayerMask.NameToLayer("IgnorePlayer")) 
						| (1 << LayerMask.NameToLayer("PlayerObject")) ;

		player = GameObject.Find("Player1");
		deathAnimationTime = GameResources.GetAnimationClip(GameResources.KeySmallExplosionAnimation).length;

		if (isFacingRight)
			directionFacing = new Vector2(1,0);
		else
			directionFacing = new Vector2(-1,0);

		aiDetection = new AIDetection(playerLayerMask, sightDistance, transform, jumpDistance);
	}

	protected virtual void Update(){
		// move enemy here
		if ((timeToWaitForDetection -= Time.deltaTime)<=0){
			playerDetected = aiDetection.EnemyIsNear(transform.position,player.transform.position);
			float distance = player.transform.position.x - transform.position.x;
			float xDirection = distance >= 0 ? 1f : -1f;
			if (xDirection != directionFacing.x && Mathf.Abs(distance) > .3f ){
					MirrorSprite();
					directionFacing = -directionFacing;
				}
			targetPosition = aiDetection.EnemyPosition();
			timeToWaitForDetection = .05f;
		}
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
