using UnityEngine;
using System.Collections;

public class IgnoreJumpCollision : MonoBehaviour {
	
	void OnCollisionEnter2D (Collision2D coll) {
		Physics2D.IgnoreLayerCollision (8, 9, rigidbody2D.velocity.y > 0);
	}
	void OnCollisionStay2D (Collision2D coll) {
		Physics2D.IgnoreLayerCollision (8, 9, rigidbody2D.velocity.y > 0);
}
