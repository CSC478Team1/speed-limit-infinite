using UnityEngine;
using System.Collections;

public class CameraScript : MonoBehaviour {

	private Transform player;

	// Use this for initialization
	private void Start () {
		player = GameObject.Find("Player1").transform;
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 position = new Vector3(player.position.x, player.position.y, transform.position.z);
		gameObject.transform.position = position;

		if (Input.GetButtonDown("Camera Zoom Out"))
			if (gameObject.camera.fieldOfView < 100)
				gameObject.camera.fieldOfView += 5;

		if (Input.GetButtonDown("Camera Zoom In"))
			if (gameObject.camera.fieldOfView > 20)
				gameObject.camera.fieldOfView -=5;
	}
}
