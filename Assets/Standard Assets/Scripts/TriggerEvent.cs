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
	private bool audioPlaying = false;
	private float audioTime = 0f;

	/// <summary>
	/// Initialize variables. Load sound resources if they exist.
	/// </summary>
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
	/// Update is called once per frame. Prevent multiple audio instances by putting a time limit on audio.
	/// </summary>
	private void Update(){
		if (audioPlaying){
			if ((audioTime -= Time.deltaTime) <= 0)
				audioPlaying = false;
		}
	}

	/// <summary>
	/// Trigger event zone entered. Door keys are evaluated.
	/// (Requirement 1.5.1.1.1) Triggers - Key unlocks door
	/// (Requirement 1.5.1.1.2) Triggers - Key doesn't unlock doors it shouldn't
	/// (Requirement 1.5.1.1.3) Triggers - (Key) Removed from inventory after door is opened.
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
					if (failureClip != null && !audioPlaying){
						audioPlaying = true;
						audioTime = failureClip.length;
						SoundManager.PlaySound(failureClip, transform);
					}
					GameManager.DisplayMessage("You need " + requiredItemName + " to proceed");
				}
	}
}
