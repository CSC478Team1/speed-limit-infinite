using UnityEngine;
using System.Collections;

public class LoadScene : MonoBehaviour
{
	public string sceneName;

	void Start(){
		//particle systems lack a sorting layer option in the Inspector
		particleSystem.renderer.sortingLayerName = "Foreground";
	}
	
	void OnCollisionEnter2D (Collision2D other)
	{
		//make sure other objects aren't triggering this
		if (other.gameObject.tag == "Player") 
			GameManager.LoadNextLevel(sceneName);
	}
}