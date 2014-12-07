using UnityEngine;
using System.Collections;
/// <summary>
/// Ending Credits that scroll a Game Object containing text up to a certain point. Once the position is reached, the Main Menu is called again.
/// </summary>
public class Credits : MonoBehaviour {
	private float speed = .04f;

	private void OnGUI(){
		if (gameObject.transform.position.y < 4.7f){
			gameObject.transform.Translate(Vector3.up * Time.deltaTime *  speed);
		}else
			GameManager.LoadNextLevel("MainMenu");
	}
}
