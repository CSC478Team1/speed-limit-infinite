using UnityEngine;
using System.Collections;

public class CloneObject : MonoBehaviour {
	public int numberToClone = 0;

	private void OnTriggerEnter2D (Collider2D other){
		//clone any object that enters collider
		//
		try{
			if (numberToClone > 0 && !other.name.Contains(GameResources.ObjectWasCloned)){
				other.name += GameResources.ObjectWasCloned;
				for (int i = 0; i < numberToClone; i++){
						
				GameObject temp = (GameObject)Instantiate(other.gameObject, new Vector3 (other.transform.position.x + (i *.6f), 
					                                                                         other.transform.position.y, other.transform.position.z), Quaternion.identity);
					temp.name = GameResources.ObjectClone;
				}
			}
		}catch (UnityException e){
			Debug.Log(e.Message);
		}
	}

	
}
