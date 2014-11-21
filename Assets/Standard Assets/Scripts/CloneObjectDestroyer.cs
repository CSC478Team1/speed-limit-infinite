using UnityEngine;
using System.Collections;

public class CloneObjectDestroyer : MonoBehaviour {

	private void OnTriggerEnter2D (Collider2D other){
		if (other.name.Contains(GameResources.ObjectClone))
			Destroy(other.gameObject);
	}
}
