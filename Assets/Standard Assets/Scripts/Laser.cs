using UnityEngine;
using System.Collections;

/// <summary>
/// Laser script. Causes damage to foes and adds forces to boxes.
/// </summary>
public class Laser : MonoBehaviour {

	public int damageValue;


	/// <summary>
	/// If laser has left the screen then delete the game object.
	/// (Requirement 2.2.2.4) Shoot - Bullet disappears after impact or off screen
	/// </summary>
	private void OnBecameInvisible() {
		Destroy (gameObject);
	}

	/// <summary>
	/// Checks the various colliding objects the laser shot can encounter. Enemies should decrease health, boxes should add force.
	/// (Requirement 1.3.4) Blocks - Blocks interact with bullet
	/// (Requirement 2.2.2.2) Player Actions - Bullet impacts
	/// (Requirement 2.2.2.3.1) Shoot - Bullet damages - Hurts AI
	/// (Requirement 2.2.2.3.2) Shoot - Bullet damages - Moves blocks
	/// (Requirement 2.2.2.4) Shoot - Bullet disappears after impact or off screen
	/// </summary>
	/// <param name="other">Other collidng object</param>
	private void OnTriggerEnter2D (Collider2D other){
		//dual laser shots can trigger events close to one another
		//check if gameObject is null if second laser hits before item is destroyed
		if ((other.tag == "Enemy" || other.tag == "Player") && gameObject != null )
		{ 
			try{
				if (gameObject.tag.Replace("Projectile", "") != other.tag){
					//play sound and do damage!
					other.GetComponent<Controller>().DecreaseHealth(damageValue);
				}

				Destroy(gameObject);
			}catch (UnityException e){
				Debug.Log(e.Message);
			}
				
		}
		else if ((other.tag != "Player Object") && gameObject != null){
			if (other.tag == "Box"){
				other.rigidbody2D.AddForce(new Vector2(gameObject.rigidbody2D.velocity.x * 3 + gameObject.rigidbody2D.mass, 0));
			}
			Destroy(gameObject);
		}
	}
}
