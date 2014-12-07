using UnityEngine;
using System.Collections;

/// <summary>
/// Destroys cloned objects if they enter a trigger zone.
/// </summary>
public class CloneObjectDestroyer : MonoBehaviour {

	private void OnTriggerEnter2D (Collider2D other){
		if (other.name.Contains(GameResources.ObjectClone))
			Destroy(other.gameObject);
	}
}
