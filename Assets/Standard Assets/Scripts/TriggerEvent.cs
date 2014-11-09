﻿using UnityEngine;
using System.Collections;

public class TriggerEvent : MonoBehaviour {
	public int requiresItemID;
	public bool removeItemAfterUse = false;

	private void OnTriggerEnter2D (Collider2D other){
		if (other.gameObject.tag == "Box")
			Destroy (other.gameObject); // change this for pushable scripting

		if (gameObject.tag == "Player Object")
			if (requiresItemID != null && other.tag == "Player")
				if (ItemDatabase.HasItem(requiresItemID)){
					if (removeItemAfterUse)
						ItemDatabase.RemoveItem(ItemDatabase.GetItem(requiresItemID));
					Destroy(gameObject); // destroy the door for now
				}else{
					//display message to user asking to obtain requiresItemID
				}
	}
}
