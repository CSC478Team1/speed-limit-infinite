using UnityEngine;
using System.Collections;

/// <summary>
/// Creature generator. Generates random enemies based on skill and time delay.
/// </summary>

public class CreatureGenerator : MonoBehaviour {

	// generate enemies based on skill level 1-4
	public int enemySkillLevel = 2;
	//delay in between spawnings
	public float spawnDelay = 2f;
	private float originalDelay;
	//number of loops to complete before finished
	public int numberOfRounds = 4;
	private bool startGenerator = false;
	private bool isDepleted = false;
	private GameObject[] enemies;
	
	/// <summary>
	/// Initializes variables. Load enemies array based on chosen skill level.
	/// </summary>
	private void Start () {
		originalDelay = spawnDelay;
		if (enemySkillLevel > 4)
			enemySkillLevel = 4;

		enemies = new GameObject[enemySkillLevel];
		if (enemySkillLevel > 0){
			enemies[0] = GameResources.GetGameObject(GameResources.KeyEnemyDarkRobot);
			if (enemySkillLevel > 1){
				enemies[1] = GameResources.GetGameObject(GameResources.KeyEnemySilverRobot);
			}
			if (enemySkillLevel > 2){
				enemies[2] = GameResources.GetGameObject(GameResources.KeyEnemyRustyRobot);
			}
			if (enemySkillLevel > 3){
				enemies[3] = GameResources.GetGameObject(GameResources.KeyEnemyBomb);
			}
		}
	
	}
	
	/// <summary>
	/// Update is called once per frame. startGenerator must be true before generating code can start. Generating enemies is handled here. Once the number of rounds is less than 1 the Game Object is destroyed.
	/// </summary>
	private void Update () {
		if (startGenerator && !isDepleted){
			if ((spawnDelay -= Time.deltaTime) <= 0){
				int index = Random.Range(0, enemySkillLevel);
				float randomX = Random.Range(-2f, 2f);
				Instantiate(enemies[index], new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z), transform.rotation);

				if (--numberOfRounds < 1)
					isDepleted = true;
				spawnDelay = originalDelay;
			}
		}else if (isDepleted)
			Destroy(gameObject);
	
	}

	/// <summary>
	/// Trigger Event that looks for the Player tag. Sets startGenerator to true, enabling the generator in Update()
	/// </summary>
	/// <param name="other">Other colliding object</param>
	private void OnTriggerEnter2D(Collider2D other){
		if (other.tag == "Player")
			startGenerator = true;
	}
}
