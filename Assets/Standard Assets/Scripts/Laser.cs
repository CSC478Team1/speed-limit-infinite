using UnityEngine;
using System.Collections;

public class Laser : MonoBehaviour {

	public int damageValue;

	
	//Left screen so destroy it
	private void OnBecameInvisible() {
		Destroy (gameObject);
	}
	
	private void OnTriggerEnter2D (Collider2D other){
		//dual laser shots can trigger events close to one another
		//check if gameObject is null if second laser hits before item is destroyed
		if ((other.tag == "Enemy" || other.tag == "Player") && gameObject != null )
		{ 
			try{
				if (gameObject.tag.Replace("Projectile", "") != other.tag){
					//play sound and do damage!
					//leaving this 
					other.GetComponent<Controller>().DecreaseHealth(damageValue);
				}

				Destroy(gameObject);
			}catch (UnityException e){
				Debug.Log(e.Message);
			}
				
		}
		else if ((other.tag != "Player Object") && gameObject != null)
			Destroy(gameObject);
	}
}
