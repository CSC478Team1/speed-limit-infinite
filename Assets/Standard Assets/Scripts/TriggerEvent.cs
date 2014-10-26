using UnityEngine;
using System.Collections;

public class TriggerEvent : MonoBehaviour {
	public int requiresItemID;

	private void OnTriggerEnter2D (Collider2D other){
		if (other.gameObject.tag == "Box")
			Destroy (other.gameObject); // change this for pushable scripting

		if (gameObject.tag == "Player Object")
			if (requiresItemID != null)
				if (ItemDatabase.HasItem(requiresItemID))
					Destroy(gameObject); // destroy the door for now
				else{
					//display message to user asking to obtain requiresItemID
				}
	}
}
