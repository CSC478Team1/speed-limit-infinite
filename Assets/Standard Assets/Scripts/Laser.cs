using UnityEngine;
using System.Collections;

public class Laser : MonoBehaviour {

	public int damageValue;

	//Left screen so destroy it
	private void OnBecameInvisible() {
		Destroy (gameObject);
	}
	
	private void OnTriggerEnter2D (Collider2D other){
		if ((other.tag == "Enemy" || other.tag == "Player") && gameObject != null )
		{ 
			try{
				if (gameObject.tag.Replace("Projectile", "") != other.tag){
					other.GetComponent<Controller>().DecreaseHealth(damageValue);
					//play sound and do damage!
					Destroy(gameObject);
				} else
					Destroy(gameObject);
			}catch (UnityException e){
				Debug.Log(e.Message);
			}
				
		}
		else if ((other.tag != "Player Object") && gameObject != null)
			Destroy(gameObject);
	}
}
