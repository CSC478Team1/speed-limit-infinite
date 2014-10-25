using UnityEngine;
using System.Collections;

public class CloneObject : MonoBehaviour {
	public int numberToClone;
	public GameObject objectToClone;

	private void OnTriggerEnter2D (Collider2D other){
		//clone colliding object only if it isn't the player
		//To clone the player use prefab of the player 1 for cloning
		//will fix this when we remove the camera as a child of player 1
		try{
			if (numberToClone != null && !other.name.Contains("CLONEDOBJECT"))
			    if (numberToClone > 0){
					other.name += GameResources.ObjectWasCloned;
					for (int i = 0; i < numberToClone; i++){
						GameObject temp;
						if (other.gameObject.tag != "Player")
							temp = (GameObject)Instantiate(other.gameObject, other.transform.position, Quaternion.identity);
						else
							temp = (GameObject)Instantiate(objectToClone, other.transform.position, Quaternion.identity);
						temp.name = GameResources.ObjectClone;
					}
			}
		}catch (UnityException e){
			Debug.Log(e.Message);
		}
	}

	
}
