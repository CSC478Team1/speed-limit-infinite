using UnityEngine;
using System.Collections;

public class TriggerEvent : MonoBehaviour {

	private void OnTriggerEnter2D (Collider2D other){
		if (other.gameObject.tag == "Box")
			Destroy (other.gameObject); // change this for pushable scripting
	}
}
