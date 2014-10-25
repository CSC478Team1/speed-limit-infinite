using UnityEngine;
using System.Collections;

public class EnemyController : Controller {
	
	// Use this for initialization
	protected override void Start () {
		base.Start();	
	}
	
	// Update is called once per frame
	protected override void FixedUpdate () {
		if(isDead)
			Destroy(gameObject);
	}
}
