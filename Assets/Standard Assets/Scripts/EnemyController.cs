using UnityEngine;
using System.Collections;
/// <summary>
/// Enemy controller class inherits from Controller class. AI Detection and death sequences mainly happen here.
/// </summary>
public class EnemyController : Controller {
	//declare these public to set individual values inside Unity's Inspector
	public int enemyHealth = 100;
	public float sightDistance = 8f;
	public float jumpDistance = 5f;
	//end public Inspector variables

	protected int playerLayerMask;
	protected bool playerDetected = false;
	protected Vector2 directionFacing;
	private float timeToWaitForDetection = .05f;
	private bool locked = false;
	protected Vector2 targetPosition;
	private float deathAnimationTime = 0;
	protected AIDetection aiDetection;
	protected int groundLayerMask;
	protected GameObject player;
	
	/// <summary>
	/// Initialize values for animator based on selected Animator Controller and set health. Player and ground bitmasks are initialized here.
	/// </summary>
	protected override void Start () {
		base.Start();	
		playerLayerMask = (1 << LayerMask.NameToLayer("Player")) | (1 << LayerMask.NameToLayer("Default"));
		groundLayerMask = (1 << LayerMask.NameToLayer("Default")) | (1 << LayerMask.NameToLayer("Platform")) | (1 << LayerMask.NameToLayer("IgnorePlayer")) 
						| (1 << LayerMask.NameToLayer("PlayerObject")) ;

		player = GameManager.GetPlayerObject();
		deathAnimationTime = GameResources.GetAnimationClip(GameResources.KeySmallExplosionAnimation).length;

		if (isFacingRight)
			directionFacing = new Vector2(1,0);
		else
			directionFacing = new Vector2(-1,0);

		aiDetection = new AIDetection((1 << LayerMask.NameToLayer("Player")), sightDistance, jumpDistance);
		SetHealth(enemyHealth);
	}

	/// <summary>
	/// Update is called once per frame. Check if player is near and if needed Mirror the sprite to face player.
	/// </summary>
	protected virtual void Update(){
		// move enemy here
		if (!isDead){
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
	}
				
	/// <summary>
	/// FixedUpdate is called every fixed frame. Load death prefab if object is dead.
	/// (Requirement 3.3.3) AI Actions - Dies - When shot by player
	/// </summary>
	protected override void FixedUpdate () {
		base.FixedUpdate();
		if(isDead && !locked){
			locked = true;
			sightDistance = 0f;
			gameObject.layer = LayerMask.NameToLayer("IgnorePlayer");
			GameObject explosion = Instantiate(GameResources.GetGameObject(GameResources.KeySmallExplosion), transform.position, Quaternion.identity) as GameObject;
			if (explosion != null){
				//bind position of gameobject to explosion
				explosion.transform.parent = gameObject.transform;
				Animator tempAnimator = explosion.GetComponent<Animator>();
				AudioSource source = explosion.GetComponent<AudioSource>();
				if (source != null)
					SoundManager.PlaySound(source.audio.clip, explosion.transform);
				tempAnimator.SetTrigger("Explode");
				//gameObject.rigidbody2D.isKinematic = true;
				//gameObject.renderer.renderer.collider2D.isTrigger = true;
				StartCoroutine(Pause(deathAnimationTime, true, explosion, this.gameObject));
			} else
				Destroy(gameObject);

		}
	}
	public int GetCurrentHealth(){
		return health;
	}
	/// <summary>
	/// If destroyCurrent is true, current game object is destroyed along with any other instantiated game objects.
	/// </summary>
	/// <param name="destroyCurrent">If set to <c>true</c> destroy current game object.</param>
	/// <param name="destroyObjects">Game object array to destroy</param>
	private void Destroy(bool destroyCurrent, GameObject [] destroyObjects){
		for (int i = 0; i < destroyObjects.Length; i++){
			if (destroyObjects[i] != null)
				Destroy(destroyObjects[i]);
		}
	}

	/// <summary>
	/// Pause the destroying of Game Object long enough to play animation sequence and then destroy everything.
	/// </summary>
	/// <param name="time">Time needed to play animation</param>
	/// <param name="destroy">If set to <c>true</c> destroy current game object</param>
	/// <param name="destroyObjects">Variable length argument of game objects to destroy. Current game object should be last object destroyed if it is to be destroyed.</param>
	protected IEnumerator Pause(float time, bool destroy, params GameObject[] destroyObjects){
		yield return new WaitForSeconds(time);
		if (destroy)
			Destroy(destroy, destroyObjects);
	}
}
