using UnityEngine;
using System.Collections;

/// <summary>
/// Clone script that duplicates an object a set amount of times. Alters name to avoid endless loops.
/// </summary>
public class CloneObject : MonoBehaviour {
	// declared 0 and public to interact with Unity's Inspector. Value is set inside of Unity.
	public int numberToClone = 0;  


	/// <summary>
	/// Game Object has entered collision trigger zone. Game Object will be cloned if it hasn't been already.
	/// </summary>
	/// <param name="other">Other colliding object</param>
	private void OnTriggerEnter2D (Collider2D other){
		try{
			if (numberToClone > 0 && !other.name.Contains(GameResources.ObjectWasCloned)){
				//other object name has a clone name appended to it to prevent further cloning
				other.name += GameResources.ObjectWasCloned;
				for (int i = 0; i < numberToClone; i++){
					//create a game object and offset it a little bit.
					GameObject temp = (GameObject)Instantiate(other.gameObject, new Vector3 (other.transform.position.x + (i *.6f), 
					                                                                         other.transform.position.y, other.transform.position.z), Quaternion.identity);
					//set game objects name to child of cloned object to avoid further cloning.
					temp.name = GameResources.ObjectClone;
				}
			}
		}catch (UnityException e){
			Debug.Log(e.Message);
		}
	}

	
}
