using UnityEngine;
using System.Collections;

/// <summary>
/// Static Sound Manager class. Used to play audio and retain volume levels.
/// </summary>

public static class SoundManager{

	public static float SoundVolume {get; set;} 
	public static float MusicVolume {get; set;} 

	static SoundManager(){
		SoundVolume = .6f;
		MusicVolume = .4f;
	}

	public static void PlaySound(AudioClip clip, Transform atPosition){
		AudioSource.PlayClipAtPoint(clip, atPosition.position, SoundVolume);
	}
	public static void PlaySoundAtCamera(AudioClip clip){
		PlayAudioAtCamera(clip, SoundVolume);
	}
	public static void PlayMusicAtCamera(AudioClip clip){
		PlayAudioAtCamera(clip, MusicVolume);
	}
	private static void PlayAudioAtCamera(AudioClip clip, float volume){
		AudioSource.PlayClipAtPoint(clip, GameObject.Find("Main Camera").transform.position, volume);
	}
}
