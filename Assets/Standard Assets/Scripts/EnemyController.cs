using UnityEngine;
using System.Collections;

public class EnemyController : Controller {

	protected int playerLayerMask;
	protected bool playerDetected = false;
	protected Vector2 directionFacing;
	protected float sightDistance = 10f;
	private bool locked = false;
	protected float targetDistance = 100f;

	private float deathAnimationTime = 0;
	
	// Use this for initialization
	protected override void Start () {
		base.Start();	
		playerLayerMask = (1 << LayerMask.NameToLayer("Player")) | (1 << LayerMask.NameToLayer("Default"));

		deathAnimationTime = GameResources.GetAnimationClip(GameResources.KeySmallExplosionAnimation).length;

		if (isFacingRight)
			directionFacing = new Vector2(1,0);
		else
			directionFacing = new Vector2(-1,0);
	}

	protected virtual void Update(){
		// move enemy here

		RaycastHit2D playerDetection = Physics2D.Raycast (transform.position, directionFacing, sightDistance, playerLayerMask);
		playerDetected = (playerDetection.collider != null && playerDetection.collider.tag == "Player") ? true: false;
		if (!playerDetected){
			playerDetection = Physics2D.Raycast (transform.position, -directionFacing, sightDistance, playerLayerMask);
			playerDetected =  (playerDetection.collider != null && playerDetection.collider.tag == "Player")? true : false;
			if (playerDetected){
				MirrorSprite();
				directionFacing = -directionFacing;
				targetDistance = playerDetection.distance;
			}
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
				Animator tempAnimator = explosion.GetComponent<Animator>();
				tempAnimator.SetTrigger("Explode");
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
