using UnityEngine;
using System.Collections;

public class Credits : MonoBehaviour {
	private float speed = .04f;

	private void OnGUI(){
		if (gameObject.transform.position.y < 4.7f){
			gameObject.transform.Translate(Vector3.up * Time.deltaTime *  speed);
		}else
			GameManager.LoadMainMenu();
	}
}
