using UnityEngine;
using System.Collections;

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
	// Use this for initialization
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
	
	// Update is called once per frame
	private void Update () {
		if (startGenerator && !isDepleted){
			if ((spawnDelay -= Time.deltaTime) <= 0){
				int index = Random.Range(0, enemySkillLevel);

				Instantiate(enemies[index], transform.position, transform.rotation);

				if (--numberOfRounds < 1)
					isDepleted = true;
				spawnDelay = originalDelay;
			}
		}else if (isDepleted)
			Destroy(gameObject);
	
	}

	private void OnTriggerEnter2D(Collider2D other){
		if (other.tag == "Player")
			startGenerator = true;
	}
}
