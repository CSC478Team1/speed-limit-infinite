﻿using UnityEngine;
using System.Collections;

/// <summary>
/// Bomb enemy controller.Character cannot shoot and contains own AI Navigation 
/// </summary>

public class BombEnemyController : EnemyController {

	//hack to prevent audio from stacking itself up and increasing volume way too loudly
	private static float audioDelayTimer = .3f;
	private static bool audioLocked = false;

	private float enemyHeight;
	private float enemyWidth;
	private float attackDistance = 1.5f;
	private int edgeBitmask;
	private float appearTime = 0f;
	private bool isLocked = false;



	/// <summary>
	/// Initializes instance and sets speed, appear animation speeds, boundary bitmask, and enemy sprite sizes
	/// </summary>
	protected override void Start () {
		base.Start();

		enemyHeight = gameObject.renderer.bounds.size.y;
		enemyWidth = gameObject.renderer.bounds.size.x;
		edgeBitmask = (1 << LayerMask.NameToLayer("Default")) | (1 << LayerMask.NameToLayer("Platform"))
						| (1 << LayerMask.NameToLayer("IgnorePlayer")) | (1 << LayerMask.NameToLayer("PlayerObject"));

		speed = 3f;
		appearTime = GameResources.GetAnimationClip(GameResources.KeyBombBotAppearAnimation).length;

	}
	/// <summary>
	/// Called once per frame. Game behaviour is suggested here.
	/// </summary>
	protected override void Update(){
		base.Update();
		
		//should be facing player if it is detected
		if (playerDetected && appearTime <= 0 && !isLocked && !isDead){
			//if player is in attack radius blow up!
			if (Mathf.Abs(transform.position.x - targetPosition.x) < attackDistance && Mathf.Abs(transform.position.y - targetPosition.y) < attackDistance){
				isLocked = true;
				rigidbody2D.isKinematic = true;
				rigidbody2D.collider2D.enabled = false;
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

		if (audioLocked){
			if ((audioDelayTimer -= Time.deltaTime) <= 0){
				audioDelayTimer = .3f;
				audioLocked = false;
			}
		}


	}
	/// <summary>
	/// Determines if character can move
	/// </summary>
	/// <returns><c>true</c> if this character can move; otherwise, <c>false</c>.</returns>
	private bool CanMove(){
		return (Physics2D.OverlapCircle(transform.position, .5f, edgeBitmask));
	}

	/// <summary>
	/// Move this character.
	/// (Requirement 3.1.1) AI Actions - Moves forward
	/// (Requirement 3.1.2) AI Actions - Moves backwards
	/// </summary>
	private void Move(){
		if (CanMove()){
			if (directionFacing.x > 0){
				rigidbody2D.velocity = new Vector2(Vector2.right.x * speed, 0);
			}
			else{
				rigidbody2D.velocity = new Vector2(-Vector2.right.x * speed, 0);
			}
			anim.SetFloat("Speed", 1);
		}
	}

	/// <summary>
	/// Initializes the detonation sequence
	/// </summary>
	private void Detonate(){
		
		GameObject detonationObject = Instantiate(GameResources.GetGameObject(GameResources.KeyLargeExplosion), transform.position, Quaternion.identity) as GameObject;
		detonationObject.transform.parent = gameObject.transform;
		AudioSource source = detonationObject.GetComponent<AudioSource>();
		if (source != null){
			if (!audioLocked){
				audioLocked = true;
				SoundManager.PlaySound(source.audio.clip, detonationObject.transform);
			}
		}
		float detonationTime = GameResources.GetAnimationClip(GameResources.KeyLargeExplosionAnimation).length;
		Animator tempAnimator = detonationObject.GetComponent<Animator>();
		tempAnimator.SetTrigger("Explode");
		StartCoroutine(Pause(detonationTime, true, detonationObject, gameObject));
	}

}
