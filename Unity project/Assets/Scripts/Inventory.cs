using System;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Handles behavior for the player's inventory.
/// </summary>
[RequireComponent(typeof(BoxCollider2D))]
public class Inventory : MonoBehaviour
{
	public static Inventory Instance { get; private set; }
	/// <summary>
	/// The texture that the game is being rendered to.
	/// </summary>
	static private RenderTexture GameRendTex { get { return MainCamera.Instance.targetTexture; } }


	public Transform MyTransform { get; private set; }
	public BoxCollider2D MyCollider { get; private set; }
	public Texture CurrentlySelectedTex
	{
		get
		{
			if (CurrentlySelected.HasValue)
			{
				switch (CurrentlySelected.Value)
				{
					case InventoryObjects.Key: return KeyTex;
					case InventoryObjects.MutilatedRat: return MutilatedRatTex;
					case InventoryObjects.Necklace: return NecklaceTex;
					case InventoryObjects.WheelValve: return WheelValveTex;
					default: Debug.LogError("Unknown inventory object '" + CurrentlySelected.Value.ToString() + "'"); return null;
				}
			}
			return null;
		}
	}


	public enum InventoryObjects
	{
		MutilatedRat,
		Key,
		Necklace,
		WheelValve,
	}

	public Texture2D MutilatedRatTex, KeyTex, NecklaceTex, WheelValveTex;
	public Dictionary<InventoryObjects, bool> HasObjects = new Dictionary<InventoryObjects, bool>()
	{
		{ InventoryObjects.MutilatedRat, false },
		{ InventoryObjects.Key, false },
		{ InventoryObjects.Necklace, false },
		{ InventoryObjects.WheelValve, false },
	};
	public InventoryObjects? CurrentlySelected = null;

	public Texture2D SelectedTex, DeSelectedTex;
	public bool IsSelected = false;

	public Vector2 DisplaySpaceMinLerp = new Vector2(0.1157f, 0.005917f),
				   DisplaySpaceMaxLerp = new Vector2(0.91735537f, 0.64497f);

	public Rect DeSelectedArea = new Rect(-0.5f, -0.5f, 1.0f, 1.0f);
	private Rect selectedArea;


	private void SetSelectedCollider()
	{
		MyCollider.center = selectedArea.center;
		MyCollider.size = selectedArea.size;
	}
	private void SetDeSelectedCollider()
	{
		MyCollider.center = DeSelectedArea.center;
		MyCollider.size = DeSelectedArea.size;
	}

	void Awake()
	{
		if (Instance != null)
		{
			Debug.LogError("More than one 'Inventory' component is in the scene!");
			return;
		}
		Instance = this;

		MyTransform = transform;
		MyCollider = GetComponent<BoxCollider2D>();

		Bounds bs = MyCollider.bounds;
		selectedArea = new Rect(MyCollider.center.x - (0.5f * MyCollider.size.x),
								MyCollider.center.y - (0.5f * MyCollider.size.y),
								MyCollider.size.x, MyCollider.size.y);

		MyCollider.center = DeSelectedArea.center;
		MyCollider.size = DeSelectedArea.size;
	}
	void OnGUI()
	{
		//Calculate positioning info and draw the inventory background.
		Texture2D texToUse = (IsSelected ? SelectedTex : DeSelectedTex);
		if (!IsSelected)
			SetSelectedCollider();
		ScreenPositioningData data = new ScreenPositioningData(MyCollider);
		if (!IsSelected)
			SetDeSelectedCollider();

		Vector2 center = (data.MinPos + data.MaxPos) * 0.5f;
		Vector2 texSize = new Vector2(data.ScreenSizeScale.x * texToUse.width,
									  data.ScreenSizeScale.y * texToUse.height);
		GUI.DrawTexture(new Rect(center.x - (texSize.x * 0.5f),
								 center.y - (texSize.y * 0.5f),
								 texSize.x, texSize.y),
						texToUse, ScaleMode.StretchToFill, true);

		Vector2 objTexSizes = data.GetLerpSize(new Vector2(KeyTex.width, KeyTex.height)),
				halfObjTexSizes = 0.5f * objTexSizes;
		Vector2 displaySpaceMin = DisplaySpaceMinLerp + halfObjTexSizes,
				displaySpaceMax = DisplaySpaceMaxLerp - halfObjTexSizes;
		float displaySpaceMidpointY = (displaySpaceMin.y + displaySpaceMax.y) * 0.5f;

		if (IsSelected)
		{
			if (HasObjects[InventoryObjects.MutilatedRat])
			{
				if (data.GUIButton(displaySpaceMin, new Vector2(KeyTex.width, KeyTex.height), new Vector2(),
								   CellPhone.Instance.ButtonStyle, MutilatedRatTex))
				{
					CurrentlySelected = InventoryObjects.MutilatedRat;
				}
			}
			if (HasObjects[InventoryObjects.Key])
			{
				if (data.GUIButton(new Vector2(displaySpaceMax.x, displaySpaceMin.y),
								   new Vector2(KeyTex.width, KeyTex.height), new Vector2(),
								   CellPhone.Instance.ButtonStyle, KeyTex))
				{
					CurrentlySelected = InventoryObjects.Key;
				}
			}
			if (HasObjects[InventoryObjects.Necklace])
			{
				if (data.GUIButton(new Vector2(displaySpaceMin.x, displaySpaceMidpointY),
								   new Vector2(KeyTex.width, KeyTex.height), new Vector2(),
								   CellPhone.Instance.ButtonStyle, NecklaceTex))
				{
					CurrentlySelected = InventoryObjects.Necklace;
				}
			}
			if (HasObjects[InventoryObjects.WheelValve])
			{
				if (data.GUIButton(new Vector2(displaySpaceMax.x, displaySpaceMidpointY),
								   new Vector2(KeyTex.width, KeyTex.height), new Vector2(),
								   CellPhone.Instance.ButtonStyle, WheelValveTex))
				{
					CurrentlySelected = InventoryObjects.WheelValve;
				}
			}
		}


		if (CurrentlySelected.HasValue)
		{
			Vector2 mousePos = new Vector2(Input.mousePosition.x, Screen.height - Input.mousePosition.y),
					mTexSize = 0.5f * new Vector2(KeyTex.width * data.ScreenSizeScale.x,
						  			 		      KeyTex.height * data.ScreenSizeScale.y);
			GUI.DrawTexture(new Rect(mousePos.x - mTexSize.x,
									 mousePos.y - mTexSize.y,
									 KeyTex.width * data.ScreenSizeScale.x,
									 KeyTex.height * data.ScreenSizeScale.y),
							CurrentlySelectedTex);
		}
	}
	void OnDrawGizmos()
	{
		Transform tr = transform;

		Gizmos.color = new Color(0.5f, 0.5f, 0.5f, 0.5f);
		Gizmos.DrawCube(tr.position + new Vector3(DeSelectedArea.center.x * tr.lossyScale.x, DeSelectedArea.center.y * tr.lossyScale.y, 0.0f),
						new Vector3(DeSelectedArea.size.x * tr.lossyScale.x, DeSelectedArea.size.y * tr.lossyScale.y, 1.0f));
	}

	public void OnClickedOn()
	{
		if (!IsSelected)
		{
			IsSelected = true;
			SetSelectedCollider();
		}
	}
	public void OnClickedOff()
	{
		if (IsSelected)
		{
			IsSelected = false;
			SetDeSelectedCollider();
		}
	}
}