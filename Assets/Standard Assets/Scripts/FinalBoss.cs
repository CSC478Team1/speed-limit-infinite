using UnityEngine;
using System.Collections;

/// <summary>
/// Boss fight. When boss is low on health drop item.
/// </summary>
public class FinalBoss : MonoBehaviour {
	//Unity Inspector variable to drop
	public GameObject dropItem;
	private EnemyController controller;

	/// <summary>
	/// Initialize controller
	/// </summary>
	void Start () {
		controller = gameObject.GetComponent<EnemyController>();
	}
	
	/// <summary>
	/// Update is called once per frame. Check enemies health and if it is low enough, put dropItem at characters current location.
	/// </summary>
	void Update () {
		if (controller.GetCurrentHealth() < 300){
			if (gameObject != null)
				dropItem.transform.position = gameObject.transform.position;
		}
	
	}
}
