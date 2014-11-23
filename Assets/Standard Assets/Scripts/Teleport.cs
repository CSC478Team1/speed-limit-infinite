using UnityEngine;
using System.Collections;

public class Teleport: MonoBehaviour {
	public Transform teleportToTransform;

	private void OnTriggerEnter2D(Collider2D other){
		other.transform.position = teleportToTransform.position;
		Destroy(gameObject);
	}

}
