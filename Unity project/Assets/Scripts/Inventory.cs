using System;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Handles behavior for the player's inventory.
/// </summary>
[RequireComponent(typeof(Collider2D))]
public class Inventory : MonoBehaviour
{
	public static Inventory Instance { get; private set; }
	/// <summary>
	/// The texture that the game is being rendered to.
	/// </summary>
	static private RenderTexture GameRendTex { get { return MainCamera.Instance.targetTexture; } }


	public Transform MyTransform { get; private set; }
	public Collider2D MyCollider { get; private set; }


	/// <summary>
	/// An object that is in an inventory.
	/// </summary>
	[Serializable]
	public class InventoryObject
	{
		public bool DoesPlayerHaveObject = false;
		public string ObjectName = "";
		public Texture2D ObjectThumbnail = null;
	}
	public InventoryObject[] InventoryItems = new InventoryObject[0];

	public Texture2D SelectedTex, DeSelectedTex;
	public bool IsSelected = false;

	public Vector2 SpacingBetweenObjectsLerp = new Vector2(0.05f, 0.1f);
	public Vector2 DisplaySpaceMinLerp = new Vector2(0.1157f, 0.005917f),
				   DisplaySpaceMaxLerp = new Vector2(0.91735537f, 0.64497f);


	void Awake()
	{
		if (Instance != null)
		{
			Debug.LogError("More than one 'Inventory' component is in the scene!");
			return;
		}
		Instance = this;

		MyTransform = transform;
		MyCollider = collider2D;
	}
	void OnGUI()
	{
		//Calculate positioning info and draw the inventory background.
		Texture2D texToUse = (IsSelected ? SelectedTex : DeSelectedTex);
		ScreenPositioningData data = new ScreenPositioningData(MyCollider);

		Vector2 center = (data.MinPos + data.MaxPos) * 0.5f;
		Vector2 texSize = new Vector2(data.ScreenSizeScale.x * texToUse.width,
									  data.ScreenSizeScale.y * texToUse.height);
		GUI.DrawTexture(new Rect(center.x - (texSize.x * 0.5f),
								 center.y - (texSize.y * 0.5f),
								 texSize.x, texSize.y),
						texToUse, ScaleMode.StretchToFill, true);


		//Draw the inventory items, filling the inventory area from left to right.

		Vector2 displaySpaceMin = new Vector2(Mathf.Lerp(data.MinPos.x, data.MaxPos.x,
														 DisplaySpaceMinLerp.x),
											  Mathf.Lerp(data.MaxPos.y, data.MinPos.y,
														 DisplaySpaceMinLerp.y)),
				displaySpaceMax = new Vector2(Mathf.Lerp(data.MinPos.x, data.MaxPos.x,
														 DisplaySpaceMaxLerp.x),
											  Mathf.Lerp(data.MaxPos.y, data.MinPos.y,
														 DisplaySpaceMaxLerp.y)),
				displaySpaceSize = new Vector2(displaySpaceMax.x - displaySpaceMin.x,
											   displaySpaceMin.y - displaySpaceMax.y),
				objectSpacing = new Vector2(SpacingBetweenObjectsLerp.x * displaySpaceSize.x,
											SpacingBetweenObjectsLerp.y * displaySpaceSize.y);
		Vector2 counter = DisplaySpaceMinLerp;
		float nextYMin = DisplaySpaceMinLerp.y;

		for (int i = 0; i < InventoryItems.Length; ++i)
		{
			InventoryObject obj = InventoryItems[i];
			if (obj.DoesPlayerHaveObject)
			{
				Vector2 texLerpSize = data.GetLerpSize(new Vector2(obj.ObjectThumbnail.width,
																   obj.ObjectThumbnail.height));
				data.GUITexture(counter.x, counter.y, obj.ObjectThumbnail);

				counter.x += texLerpSize.x + SpacingBetweenObjectsLerp.x;
				nextYMin = Mathf.Max(nextYMin, counter.y + texLerpSize.y + SpacingBetweenObjectsLerp.y);

				if (counter.x > DisplaySpaceMaxLerp.x)
				{
					counter.x = DisplaySpaceMinLerp.x;
					counter.y = nextYMin;
				}
			}
		}
	}
	void OnDrawGizmos()
	{
		Collider2D colldr = collider2D;
		if (colldr is BoxCollider2D)
		{
			Bounds bnds = colldr.bounds;
			Vector2 boxCenter = (colldr as BoxCollider2D).center;

			Gizmos.color = Color.green;
			Gizmos.DrawCube(bnds.center - new Vector3(boxCenter.x, boxCenter.y, 0.0f), bnds.size);
		}
	}

	public void OnClickedOn()
	{
		IsSelected = true;
	}
	public void OnClickedOff()
	{
		IsSelected = false;
	}
}