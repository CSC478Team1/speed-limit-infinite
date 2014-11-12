using UnityEngine;
using System.Collections;

public class Explosion : MonoBehaviour {

	public int damage = 0;

	private void OnTriggerEnter2D (Collider2D other){
		//can call static Game Manager class
		//this is faster and gauranteed to be the player
		if (other.tag == "Player")
			other.GetComponent<Controller>().DecreaseHealth(damage);
	}
}
