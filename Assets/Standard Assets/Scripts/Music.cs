using UnityEngine;
using System.Collections;

public class Music : MonoBehaviour {

	private void OnEnable(){
		InGameMenu.volumeHasChanged += VolumeHasChanged;
	}
	private void OnDisable(){
		InGameMenu.volumeHasChanged -= VolumeHasChanged;
	}
	
	private void Start () {
		gameObject.audio.volume = SoundManager.MusicVolume;
	
	}

	private void VolumeHasChanged(bool isMusic, float volume){
		if (isMusic){
			AudioSource source = gameObject.GetComponent<AudioSource>();
			if (source != null){
				source.volume = volume;
			}
		}
	}
}
