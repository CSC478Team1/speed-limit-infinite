using UnityEngine;
using System.Collections;

public class Music : MonoBehaviour {
	
	void Start () {
		gameObject.audio.volume = SoundManager.MusicVolume;
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
