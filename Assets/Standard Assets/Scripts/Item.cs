using UnityEngine;
using System.Collections;
/// <summary>
/// Item class.
/// </summary>
public class Item {

	public string ItemName { get; set;}
	public int ItemID { get; set;}
	public Texture2D ItemIcon { get; set;}
	public ItemType ItemObjectType{ get; set;}
	public PowerUpType ItemPowerUpType { get; set;}

	/// <summary>
	/// Initializes a new instance of the <see cref="Item"/> class.
	/// </summary>
	/// <param name="itemName">Item name.</param>
	/// <param name="itemID">Item ID.</param>
	/// <param name="itemIcon">Item icon.</param>
	/// <param name="itemObjectType">Item object type.</param>
	/// <param name="itemPowerUpType">Item power up type.</param>
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
