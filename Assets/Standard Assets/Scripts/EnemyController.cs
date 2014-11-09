using UnityEngine;
using System.Collections;

public class EnemyController : Controller {

	protected int playerLayerMask;
	protected bool playerDetected = false;
	private Vector2 directionFacing;
	protected float sightDistance = 10f;
	
	// Use this for initialization
	protected override void Start () {
		base.Start();	
		playerLayerMask = (1 << LayerMask.NameToLayer("Player")) | (1 << LayerMask.NameToLayer("Default"));

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
		if(isDead)
			Destroy(gameObject);
	}

	protected IEnumerator Pause(float time){
		yield return new WaitForSeconds(time);
	}
}
