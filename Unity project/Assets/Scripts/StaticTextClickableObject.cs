using System;
using UnityEngine;


/// <summary>
/// A ClickableObject that just displays a message if clicked on.
/// </summary>
public class StaticTextClickableObject : ClickableObject
{
	public string message;

	public override void OnClicked(Vector2 mouse, Inventory.InventoryObjects? currentlySelected)
	{
		BeenClicked = true;

		float x = transform.position.x;
		float y = transform.position.y;

		DialogController.Instance.objectRect = new Rect((Screen.width * 0.33333f) + (x * 2.0f),
														(Screen.height * 0.25f) - y,
														Screen.width * 0.25f,
														Screen.height * 0.25f);

		if (currentlySelected.HasValue)
		{
			DialogController.Instance.SendMessage("StaticMessage", "Can't use that object on this one.");
		}
		else
		{
			DialogController.Instance.SendMessage("StaticMessage", message);
		}
	}

}