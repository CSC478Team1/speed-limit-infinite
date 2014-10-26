using UnityEngine;
using System.Collections;

public class CollectibleItem : MonoBehaviour {

	//only using this for enemies
	public GameObject fakeEnemy;
	//Leaving this public for now for the Inspector
	public string itemName;
	public int itemID;
	public Texture2D textureIcon;
	public Item.ItemType itemType;
	public Item.PowerUpType powerUpType;
	private Item item;
	public bool isHiddenEnemy;

	//initialize item to be collected
	private void Awake(){
		if(!isHiddenEnemy)
			item = new Item(itemName, itemID, textureIcon, itemType, powerUpType);
	}
	private void OnTriggerEnter2D (Collider2D other){
		//tag can be enemy on fake collectible items, but switch it to Player Objects
		if (other.tag == "Player" && !isHiddenEnemy){
			try{
				if (powerUpType != Item.PowerUpType.None)
					GameObject.Find("Player1").GetComponent<PlayerController>().SetPowerUp(powerUpType);

				ItemDatabase.AddItem(item);

				//play sound or something and disappear
				Destroy(gameObject);
			} catch (UnityException e){
				Debug.Log(e.Message);
			}
		} else if (isHiddenEnemy){
			try{
				if (fakeEnemy != null)
					fakeEnemy = (GameObject)Instantiate(fakeEnemy, this.transform.position, Quaternion.identity);
				Destroy(gameObject);
			}catch (UnityException e){
				Debug.Log(e.Message);
			}
		}
	}
	
}
