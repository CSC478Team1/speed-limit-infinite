using UnityEngine;
using System.Collections;

public class Credits : MonoBehaviour {
	private float speed = .04f;

	private void OnGUI(){
		if (gameObject.transform.position.y < 4.1f){
			gameObject.transform.Translate(Vector3.up * Time.deltaTime *  speed);
		}else
			GameManager.LoadMainMenu();
	}
}
