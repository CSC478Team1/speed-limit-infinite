using UnityEngine;
using System.Collections;

/// <summary>
/// Script that causes transitions between levels or scenes.
/// </summary>

public class LoadScene : MonoBehaviour
{
	public string sceneName;

	/// <summary>
	/// Initialize the sortingLayer to Foreground since the Inspector is lacking this option.
	/// </summary>
	void Start(){
		//particle systems lack a sorting layer option in the Inspector
		particleSystem.renderer.sortingLayerName = "Foreground";
	}


	/// <summary>
	/// Check collision zone if player enters zone, then load next scene.
	/// (Requirement 1.5.1.3.1) Triggers - Player steps onto marker and transitions to next level
	/// (Requirement 5.3) Game Play - Player is able to transition between levels after reaching goal
	/// </summary>
	/// <param name="other">Other collision object</param>
	void OnCollisionEnter2D (Collision2D other)
	{
		//make sure other objects aren't triggering this
		if (other.gameObject.tag == "Player") 
			GameManager.LoadNextLevel(sceneName);
	}
}