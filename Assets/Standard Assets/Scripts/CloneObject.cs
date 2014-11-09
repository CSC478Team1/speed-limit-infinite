using UnityEngine;
using System.Collections;

public class CloneObject : MonoBehaviour {
	public int numberToClone;

	private void OnTriggerEnter2D (Collider2D other){
		//clone any object that enters collider
		//
		try{
			if (numberToClone != null && !other.name.Contains(GameResources.ObjectWasCloned))
			    if (numberToClone > 0){
					other.name += GameResources.ObjectWasCloned;
					for (int i = 0; i < numberToClone; i++){
						GameObject temp = (GameObject)Instantiate(other.gameObject, other.transform.position, Quaternion.identity);
						temp.name = GameResources.ObjectClone;
					}
			}
		}catch (UnityException e){
			Debug.Log(e.Message);
		}
	}

	
}
