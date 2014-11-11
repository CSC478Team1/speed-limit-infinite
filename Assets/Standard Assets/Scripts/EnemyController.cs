using UnityEngine;
using System.Collections;

public class EnemyController : Controller {

	protected int playerLayerMask;
	protected bool playerDetected = false;
	private Vector2 directionFacing;
	protected float sightDistance = 10f;
	private bool locked = false;

	private float deathAnimationTime = 0;
	
	// Use this for initialization
	protected override void Start () {
		base.Start();	
		playerLayerMask = (1 << LayerMask.NameToLayer("Player")) | (1 << LayerMask.NameToLayer("Default"));

		deathAnimationTime = GetAnimationTime(GameResources.GetGameObject(GameResources.KeySmallExplosion), "smallexplosion");

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

	protected float GetAnimationTime(GameObject objectToGet, string animationName){
		float returnValue = 1f;
		Animator tempAnimator = objectToGet.GetComponent<Animator>();
		UnityEditorInternal.AnimatorController tempController = tempAnimator.runtimeAnimatorController as UnityEditorInternal.AnimatorController;
		UnityEditorInternal.StateMachine tempStateMachine = tempController.GetLayer(0).stateMachine;
		for (int i = 0; i < tempStateMachine.stateCount; i++)
		{
			UnityEditorInternal.State state = tempStateMachine.GetState(i);
			if (state.name == animationName){
				AnimationClip clip = state.GetMotion() as AnimationClip;
				returnValue = clip.length;
			}
		}
		return returnValue;
	}
	private void Destroy(bool destroyCurrent, GameObject [] destroyObjects){
		for (int i = 0; i < destroyObjects.Length; i++){
			if (destroyObjects[i] != null)
				Destroy(destroyObjects[i]);
		}
	}

	private IEnumerator Pause(float time, bool destroy, params GameObject[] destroyObjects){
		yield return new WaitForSeconds(time);
		if (destroy)
			Destroy(destroy, destroyObjects);
	}
}
