using UnityEngine;
using System.Collections;

/// <summary>
/// Script for various items displayed throughout the game. Fake enemies also use this script to initialize.
/// </summary>
public class CollectibleItem : MonoBehaviour {

	//only GameObject using this for enemies
	public GameObject fakeEnemy;
	//public values are initialized and set in Unity's Inspector
	public string itemName;
	public int itemID;
	public Texture2D textureIcon;
	public Item.ItemType itemType;
	public Item.PowerUpType powerUpType;
	public bool isHiddenEnemy;
	public int value = 1; // use this for health value or item counts
	//end public Inspector values

	private Item item;
	
	/// <summary>
	/// Initialize values of the item as an Item if object isn't a fake enemy
	/// </summary>
	private void Awake(){
		if(!isHiddenEnemy)
			item = new Item(itemName, itemID, textureIcon, itemType, powerUpType);
	}

	/// <summary>
	/// Object has entered trigger collision zone. Check if colliding object is player and if current object isn't an enemy then give item to player.
	/// If current object is an object initialize the enemy whenever anything enters its collision trigger zone.
	/// </summary>
	/// <param name="other">Other colliding object</param>
	private void OnTriggerEnter2D (Collider2D other){
		//tag can be enemy on fake collectible items, but switch it to Player Objects
		if (other.tag == "Player" && !isHiddenEnemy){
			try{
				if (powerUpType != Item.PowerUpType.None)
					GameManager.AddPowerUpToPlayer(powerUpType);

				if (itemType != Item.ItemType.Consumable && itemType != Item.ItemType.Armor){
					GameManager.DisplayMessage("You've just obtained: " + itemName);
					ItemDatabase.AddItem(item);
				}
				else if (itemType == Item.ItemType.Consumable){
					GameManager.DisplayMessage(itemName);
					GameManager.AddHealthToPlayer(value);
				} else if (itemType == Item.ItemType.Armor){
					GameManager.DisplayMessage(itemName);
					GameManager.SetNewPlayerHealth(value,value);
				}

				//Destroy object once it has been given to the player
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
