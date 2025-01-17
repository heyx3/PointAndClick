﻿using System;
using UnityEngine;

public class InventoryClickableObject : ClickableObject
{
	public Inventory.InventoryObjects inventoryType;
	public string message;

	public void Update(){
	}

	public override void OnClicked(Vector2 mouse, Inventory.InventoryObjects? currentlySelected){
		Inventory.Instance.HasObjects.Remove(inventoryType);
		Inventory.Instance.HasObjects.Add (inventoryType, true);
		Destroy(this);

		DialogController.Instance.SendMessage("StaticMessage", message);
	}
}