using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

/// <summary>
/// Item database that stores Items.
/// </summary>
public static class ItemDatabase {
	//Events that indicate the ItemDatabase has changed.
	public delegate void ItemStatusChanged(Item item, bool isBeingRemoved);
	public static event ItemStatusChanged itemStatusChanged;
	
	private static List<Item> itemsList = new List<Item>();
	private static List<Item> temporaryItemList = new List<Item>();

	/// <summary>
	/// Removes an item.
	/// </summary>
	/// <param name="item">Item to remove.</param>
	public static void RemoveItem(Item item){
		try{
			itemsList.Remove(item);
			temporaryItemList.Remove(item);
			if (itemStatusChanged != null)
				itemStatusChanged(item, true);
		}catch (Exception e){
			Debug.Log(e.Message);
		}
	}

	/// <summary>
	/// Adds an item. Item must have unique ID to be added.
	/// </summary>
	/// <param name="item">Item to add.</param> 
	public static void AddItem(Item item){
		if (!HasItem(item.ItemID)){
			itemsList.Add(item);
			temporaryItemList.Add(item);
			if (itemStatusChanged != null)
				itemStatusChanged(item, false);
		}
	}

	/// <summary>
	/// Gets an item by a specified ID.
	/// </summary>
	/// <returns>The item.</returns>
	/// <param name="itemID">Item ID.</param>
	public static Item GetItem(int itemID){
		return (itemsList.Find(item => item.ItemID == itemID));
	}

	/// <summary>
	/// Determines if ItemDatabase contains item by the specified name.
	/// </summary>
	/// <returns><c>true</c> if ItemDatabase has item by the specified name; otherwise, <c>false</c>.</returns>
	/// <param name="name">Name of item.</param>
	public static bool HasItem(string name){
		return (itemsList.Find(item => item.ItemName == name) != null);
	}

	/// <summary>
	/// Determines if ItemDatabase contains item by the specified ItemID
	/// </summary>
	/// <returns><c>true</c> if ItemDatabase has the item  by the specified itemID; otherwise, <c>false</c>.</returns>
	/// <param name="itemID">Item ID.</param>
	public static bool HasItem(int itemID){
		return (itemsList.Find(item => item.ItemID == itemID) != null);
	}

	/// <summary>
	/// Gets the name of the item by ItemID.
	/// </summary>
	/// <returns>The item name.</returns>
	/// <param name="itemID">Item ID.</param>
	public static string GetItemName(int itemID){
		return itemsList.Find(item => item.ItemID == itemID).ItemName;
	}

	/// <summary>
	/// Gets a list of a specified ItemType
	/// </summary>
	/// <returns>List of contained items that match ItemType</returns>
	/// <param name="itemType">Item type.</param>
	public static List<Item> GetAllByType(Item.ItemType itemType){
		return itemsList.FindAll(item => item.ItemObjectType == itemType);
	}

	/// <summary>
	/// Gets the temporary list.
	/// </summary>
	/// <returns>The temporary list.</returns>
	public static List<Item> GetTemporaryList(){
		return temporaryItemList;
	}

	/// <summary>
	/// Gets the main item list.
	/// </summary>
	/// <returns>The main item list.</returns>
	public static List<Item> GetItemList(){
		return itemsList;
	}

	/// <summary>
	/// Clears the temporary items.
	/// </summary>
	public static void ClearTemporaryItems(){
		temporaryItemList.Clear();
	}

	/// <summary>
	/// Clears the item database and temporary items.
	/// </summary>
	public static void ClearItemDatabase(){
		temporaryItemList.Clear();
		itemsList.Clear();
	}

}
