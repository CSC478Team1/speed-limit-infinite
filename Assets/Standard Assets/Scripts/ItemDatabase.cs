using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public static class ItemDatabase {

	public delegate void ItemStatusChanged(Item item, bool isBeingRemoved);
	public static event ItemStatusChanged itemStatusChanged;
	
	private static List<Item> itemsList = new List<Item>();
	private static List<Item> temporaryItemList = new List<Item>();

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
	public static void AddItem(Item item){
		if (!HasItem(item.ItemID)){
			itemsList.Add(item);
			temporaryItemList.Add(item);
			if (itemStatusChanged != null)
				itemStatusChanged(item, false);
		}
	}
	public static Item GetItem(int itemID){
		return (itemsList.Find(item => item.ItemID == itemID));
	}
	public static bool HasItem(string name){
		return (itemsList.Find(item => item.ItemName == name) != null);
	}
	public static bool HasItem(int itemID){
		return (itemsList.Find(item => item.ItemID == itemID) != null);
	}
	public static string GetItemName(int itemID){
		return itemsList.Find(item => item.ItemID == itemID).ItemName;
	}
	public static List<Item> GetAllByType(Item.ItemType itemType){
		return itemsList.FindAll(item => item.ItemObjectType == itemType);
	}
	public static List<Item> GetTemporaryList(){
		return temporaryItemList;
	}
	public static List<Item> GetItemList(){
		return itemsList;
	}
	public static void ClearTemporaryItems(){
		temporaryItemList.Clear();
	}
	public static void ClearItemDatabase(){
		temporaryItemList.Clear();
		itemsList.Clear();
	}

}
