using UnityEngine;
using System.Collections;

/// <summary>
/// General Trigger event class. Can be used for doors or activating various objects. 
/// </summary>
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

	/// <summary>
	/// Trigger event zone entered. Door keys are evaluated.
	/// (Requirement 1.5.1.1) Triggers - key unlocks door
	/// (Requirement 1.5.1.2) Triggers - Key doesn't unlock doors it shouldn't
	/// </summary>
	/// <param name="other">Other.</param>
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
