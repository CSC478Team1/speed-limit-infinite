using UnityEngine;
using System.Collections;

public class FinalBoss : MonoBehaviour {
	public GameObject dropItem;
	private EnemyController controller;

	// Use this for initialization
	void Start () {
		controller = gameObject.GetComponent<EnemyController>();
	}
	
	// Update is called once per frame
	void Update () {
		if (controller.GetCurrentHealth() < 300){
			if (gameObject != null)
				dropItem.transform.position = gameObject.transform.position;
		}
	
	}
}
