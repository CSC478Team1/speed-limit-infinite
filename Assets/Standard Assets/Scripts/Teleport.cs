using UnityEngine;
using System.Collections;

/// <summary>
/// Teleport script. Move player to another transform position.
/// </summary>
public class Teleport: MonoBehaviour {
	public Transform teleportToTransform;


	private void OnTriggerEnter2D(Collider2D other){
		GameManager.EnableCameraFadeOut();
		other.transform.position = teleportToTransform.position;
		Destroy(gameObject);
	}

}
