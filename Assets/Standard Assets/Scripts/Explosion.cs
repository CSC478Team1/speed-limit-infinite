using UnityEngine;
using System.Collections;
/// <summary>
/// Explosion checks to see if player is in blast radius. If the player is in the blast radius, it removes some health.
/// </summary>
public class Explosion : MonoBehaviour {

	public int damage = 0;

	private void OnTriggerEnter2D (Collider2D other){
		//can call static Game Manager class
		//this is faster and gauranteed to be the player
		if (other.tag == "Player")
			other.GetComponent<Controller>().DecreaseHealth(damage);
	}
}
