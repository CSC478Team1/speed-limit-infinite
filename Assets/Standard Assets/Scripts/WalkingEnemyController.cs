using UnityEngine;
using System.Collections;

public class WalkingEnemyController : EnemyController {

	// Use this for initialization
	protected override void Start () {
		base.Start();
		health = 50;
		speed = 3f;
	}


}
