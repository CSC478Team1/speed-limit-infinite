using UnityEngine;
using System.Collections;

public class Item {

	public string ItemName { get; set;}
	public int ItemID { get; set;}
	public Texture2D ItemIcon { get; set;}
	public ItemType ItemObjectType{ get; set;}
	public PowerUpType ItemPowerUpType { get; set;}

	public Item(string itemName, int itemID, Texture2D itemIcon, ItemType itemObjectType, PowerUpType itemPowerUpType){
		ItemName = itemName;
		ItemID = itemID;
		ItemIcon = itemIcon;
		ItemObjectType = itemObjectType;
		ItemPowerUpType = itemPowerUpType;
	}
	public enum ItemType {
		PowerUp,
		Consumable,
		Armor,
		LevelItem
	}

	public enum PowerUpType{
		Laser,
		DualLaser,
		InfiniteSpeed,
		GravityBoots,
		LargeLaser,
		TripleLaser,
		None
	}
}
