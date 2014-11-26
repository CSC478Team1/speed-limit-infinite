using UnityEngine;
using System.Collections;

public class TriggerEvent : MonoBehaviour {
	public int requiresItemID = int.MinValue;
	public bool removeItemAfterUse = false;
	public string requiredItemName;
	private AudioClip successClip;
	private AudioClip failureClip;

	private void Start(){
		AudioSource [] sources = gameObject.GetComponentsInChildren<AudioSource>();
		if (sources.Length > 0){
			for (int i = 0; i < sources.Length; i++){
				if (sources[i].name.ToLower() == "success")
					successClip = sources[i].audio.clip;
				else if (sources[i].name.ToLower() == "failure")
					failureClip = sources[i].audio.clip;
			}
		}
	}

	private void OnTriggerEnter2D (Collider2D other){
		if (other.gameObject.tag == "Box")
			Destroy (other.gameObject); // change this for pushable scripting

		if (gameObject.tag == "Player Object")
			if (requiresItemID != int.MinValue && other.tag == "Player")
				if (ItemDatabase.HasItem(requiresItemID)){
					if (removeItemAfterUse){
						if (successClip != null)
							SoundManager.PlaySound(successClip, transform);
						ItemDatabase.RemoveItem(ItemDatabase.GetItem(requiresItemID));
					}
					Destroy(gameObject); // destroy the door for now
				}else{
					if (failureClip != null)
						SoundManager.PlaySound(failureClip, transform);
					GameManager.DisplayMessage("You need " + requiredItemName + " to proceed");
				}
	}
}
