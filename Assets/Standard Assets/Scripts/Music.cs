using UnityEngine;
using System.Collections;
/// <summary>
/// Music class. Plays music at specified volume.
/// </summary>
public class Music : MonoBehaviour {
	/// <summary>
	/// Attaches to the event that monitors music volume levels
	/// </summary>
	private void OnEnable(){
		InGameMenu.volumeHasChanged += VolumeHasChanged;
	}

	/// <summary>
	/// Detaches from the eent that monitors music volume levels
	/// </summary>
	private void OnDisable(){
		InGameMenu.volumeHasChanged -= VolumeHasChanged;
	}

	/// <summary>
	/// Initializes values
	/// </summary>
	private void Start () {
		gameObject.audio.volume = SoundManager.MusicVolume;
	
	}

	/// <summary>
	/// Volumes has changed event. Set current Audio Source to volume
	/// </summary>
	/// <param name="isMusic">If set to <c>true</c> the audio is music.</param>
	/// <param name="volume">Volume level</param>
	private void VolumeHasChanged(bool isMusic, float volume){
		if (isMusic){
			AudioSource source = gameObject.GetComponent<AudioSource>();
			if (source != null){
				source.volume = volume;
			}
		}
	}
}
