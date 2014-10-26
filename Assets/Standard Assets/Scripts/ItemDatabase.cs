using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public static class ItemDatabase {
	private static List<Item> itemsList = new List<Item>();
	
	public static void RemoveItem(string name){
		try{
			itemsList.Remove(itemsList.Find(item => item.ItemName == name));
		} catch (Exception e){
			Debug.Log(e.Message);
		}
	}
	public static void RemoveItem(int itemID){
		try{
			itemsList.Remove(itemsList.Find(item => item.ItemID == itemID));
		}catch (Exception e){
			Debug.Log(e.Message);
		}
	}
	public static void AddItem(Item item){
			itemsList.Add(item);
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
}
