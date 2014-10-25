using UnityEngine;
using System.Collections;

public class Controller : MonoBehaviour {
	protected int health = 100;
	protected float deathHeight = -15f;  //Y value to determine if Character fell off map
	protected bool isFacingRight = true; 
	protected bool canJump = true;
	protected Animator anim;
	protected float speed;
	protected float jumpForce;
	protected bool isDead = false;

	// Use this for initialization
	protected virtual void Start () {
		anim = GetComponent<Animator>();
	
	}

	protected virtual void FixedUpdate(){
		if (transform.position.y <= deathHeight) {
			//player died send back to spawn point 
			if (gameObject.tag == "Player"){
				isDead = true;
				//fix this later if we implement lives
			}
			else if (gameObject.tag == "Enemy"){
				Destroy(gameObject);  // add death call later
			}
		}

		if (isDead){
		}
	}
	//Decrement health return if character is alive or not
	public void DecreaseHealth(int damage){
		health -= damage;
		isDead = (health <= 0) ? true: false;
	}
	protected void MirrorSprite(){
		isFacingRight = !isFacingRight;
		Vector3 scale = transform.localScale;
		scale.x *= -1;
		transform.localScale = scale;
	}
	protected void FireWeapon(GameObject projectileObject, float projectileVelocity){

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
}
