using UnityEngine;
using System.Collections;
/// <summary>
/// Super class for all characters. Checks health, enables deaths, and fires projectiles if applicable.
/// </summary>
public class Controller : MonoBehaviour {
	//delegate to HUD to update health
	public delegate void HealthStatus(int currentHealth, int maximumHealth);
	public static event HealthStatus updateHealth;

	protected int health;
	protected int maxHealth = 100;
	protected float deathHeight = -15f;  //Y value to determine if Character fell off map
	protected bool isFacingRight = true; 
	protected bool canJump = true;
	protected Animator anim;
	protected float speed = 3f;
	protected float jumpForce;
	protected bool isDead = false;
	protected bool isClone = false;  //use this to avoid audio amplification bug when same audio is overlapped


	/// <summary>
	/// Initialize values for animator based on selected Animator Controller and set health.
	/// </summary>
	protected virtual void Start () {
		anim = GetComponent<Animator>();
		health = maxHealth;

		if (gameObject.name.Contains(GameResources.ObjectClone))
			isClone = true;
	}

	/// <summary>
	/// Check Game Object's Y position if position is below set deathHeight then declare object dead and Destroy object it is an enemy.
	/// (Requirement 2.1.4.2) Player Actions - Dies from falling
	/// (Requirement 3.3.2) AI Actions - Dies When falls below level
	/// </summary>
	protected virtual void FixedUpdate(){
		if (transform.position.y <= deathHeight) {
			//player died send back to spawn point 
			if (gameObject.tag == "Player"){
				isDead = true;
			}
			else if (gameObject.tag == "Enemy"){
				Destroy(gameObject);  //delete object to keep it from endlessly falling
			}
		}
	}

	/// <summary>
	/// Decreases the health. If health is less than or equal to 0, character is dead.
	/// (Requirement 3.2.2) AI Actions - Bullet can kill player
	/// </summary>
	/// <param name="damage">Value to be removed from health.</param>
	public void DecreaseHealth(int damage){
		health -= damage;
		isDead = (health <= 0) ? true: false;
		if (gameObject.tag == "Player")
			HealthHasChanged();
	}

	/// <summary>
	/// Increases the health of character.
	/// </summary>
	/// <param name="restoreValue">Value to add to health.</param>
	public void IncreaseHealth(int restoreValue){
		health = ((health + restoreValue) > maxHealth) ? maxHealth : health + restoreValue;
		if (gameObject.tag == "Player")
			HealthHasChanged();
	}
	/// <summary>
	/// Sets the health. If the object is the Player then call HeathHasChanged()
	/// </summary>
	/// <param name="healthValue">Health value.</param>
	protected void SetHealth(int healthValue){
		health = healthValue;
		if (gameObject.tag == "Player")
			HealthHasChanged();
	}
	/// <summary>
	/// Mirrors the sprite on X axis. Since sprites only have images in one direction the sprite needs to be adjusted.
	/// </summary>
	protected void MirrorSprite(){
		isFacingRight = !isFacingRight;
		Vector3 scale = transform.localScale;
		scale.x *= -1;
		transform.localScale = scale;
	}

	/// <summary>
	/// Fires characters weapon.
	/// (Requirement 2.2.1) Shoot in looking direction
	/// (Requirement 2.2.2.1) Bullet moves
	/// (Requirement 3.2.1) AI Shoots in looking direction
	/// </summary>
	/// <param name="projectileObject">Projectile object.</param>
	/// <param name="projectileVelocity">Projectile velocity.</param>
	protected void FireWeapon(GameObject projectileObject, float projectileVelocity){
		AudioSource source = projectileObject.GetComponent<AudioSource>();
		if (source != null && !isClone){
			//only play non-clone audio to prevent amplification bugs.
			SoundManager.PlaySound(source.audio.clip, projectileObject.transform);
		}

		if (isFacingRight){
			Vector2 displacement = new Vector2 (transform.position.x + .5f, transform.position.y);
			projectileObject = Instantiate(projectileObject, displacement, Quaternion.Euler(new Vector3(0,0,0)))as GameObject;
			projectileObject.tag = gameObject.tag + "Projectile";
			projectileObject.rigidbody2D.velocity = new Vector2 (projectileVelocity, 0);
		} else {
			Vector2 displacement = new Vector2 (transform.position.x - .3f, transform.position.y);
			projectileObject = Instantiate(projectileObject, displacement, Quaternion.Euler(new Vector3(0,0,180f))) as GameObject;
			projectileObject.tag = gameObject.tag + "Projectile";
			projectileObject.rigidbody2D.velocity = new Vector2 (-projectileVelocity, 0);
		}


	}

	/// <summary>
	/// Healths the has changed, update listeners.
	/// </summary>
	private void HealthHasChanged(){
		if (!gameObject.name.Contains(GameResources.ObjectClone) && updateHealth != null)
			updateHealth(health, maxHealth);
	}

}
