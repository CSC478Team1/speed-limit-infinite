using UnityEngine;
using System.Collections;

public class MovingPlatform : MonoBehaviour {

	public float speed = 2f;
	private bool isFinished = false;
	public bool UseOnExit = false;
	public bool requiresTrigger = false;
	public string triggerTag = "";
	private int timer = 0;
	public int maxTimer = 150;
	//private float platformWidth;
	public bool isHorizontal = true;  // don't use horizontal platforms until can fix player slipping
	private int playerLayer;
	private Transform playerCheck;
	private Transform player;

	private void Awake(){
		//playerLayer =  1 << LayerMask.NameToLayer("Player");
		//platformWidth = .96f;
		//player = GameObject.Find ("Player1").transform;
	}

	private void Update () {
		if (!requiresTrigger){
			if (isFinished)
				speed *= -1;

			Vector2 velocity;
			//playerCheck = gameObject.transform.Find ("PlayerCheck").transform;
			if (isHorizontal){
				velocity = new Vector2 (speed, 0);
				rigidbody2D.velocity = velocity;
			}
			else{
				velocity = new Vector2 (0, speed);
				rigidbody2D.velocity = velocity;
			}
			//RaycastHit2D checkForPlayer =  Physics2D.CircleCast (gameObject.transform.position, .15f,  Vector2.up, .15f, playerLayer);
			//if (checkForPlayer.transform !=null)
				//player.transform.parent = transform.parent;
			//else
				//player.transform.parent = null;
	            
			timer++;
			if (timer >= maxTimer){
				timer = 0;
				isFinished = true;
			}else
				isFinished = false;
		}
	}
	private void OnTriggerExit2D (Collider2D other){
		try{
			if (requiresTrigger && other.tag == triggerTag && UseOnExit){
				requiresTrigger = false;	
			}
		}catch (UnityException e){
			Debug.Log(e.Message);
		}
	}

	private void OnTriggerEnter2D (Collider2D other){
		try{
			if (requiresTrigger && other.tag == triggerTag && !UseOnExit){
				requiresTrigger = false;
			}
		}catch (UnityException e){
			Debug.Log(e.Message);
		}
	}

}
