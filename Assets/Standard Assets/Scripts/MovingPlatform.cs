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
				//rigidbody2D.velocity = velocity;
				rigidbody2D.MovePosition(rigidbody2D.position + velocity * Time.deltaTime);
			}
			else{
				velocity = new Vector2 (0, speed);
				rigidbody2D.velocity = velocity;
			}
			          
			timer++;
			if (timer >= maxTimer){
				timer = 0;
				isFinished = true;
			}else
				isFinished = false;
		}
	}

	//trigger events don't happen on the platform
	//use objects to activate platforms
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
