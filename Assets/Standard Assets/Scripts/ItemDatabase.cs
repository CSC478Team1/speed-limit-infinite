using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class ItemDatabase {
	private static List<Item> itemsList = new List<Item>();
	
	public static void RemoveItem(string name){
		try{
			itemsList.Remove(itemsList.Find(item => item.ItemName == name));
		} catch (UnityException e){
			Debug.Log(e.Message);
		}
	}
	public static void RemoveItem(int itemID){
		try{
			itemsList.Remove(itemsList.Find(item => item.ItemID == itemID));
		}catch (UnityException e){
			Debug.Log(e.Message);
		}
	}
	public static void AddItem(Item item){
		try{
			itemsList.Add(item);
		} catch (UnityException e){
			Debug.Log(e.Message);
		}
	}
	public static bool HasItem(string name){
		try{
			return (itemsList.Find(item => item.ItemName == name) != null);
		}catch(UnityException e){
			Debug.Log(e.Message);
			return false;
		}
	}
	public static bool HasItem(int itemID){
		try{
			return (itemsList.Find(item => item.ItemID == itemID) != null);
		}catch(UnityException e){
			Debug.Log(e.Message);
			return false;
		}
	}
	public static List<Item> GetAllByType(Item.ItemType itemType){
		try{
			return itemsList.FindAll(item => item.ItemObjectType == itemType);
		}catch(UnityException e){
			Debug.Log(e.Message);
			return null;
		}
	}
}
