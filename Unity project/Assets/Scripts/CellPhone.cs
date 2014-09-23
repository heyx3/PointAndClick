using System;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Handles the cell phone behavior.
/// Can be accessed statically because it is essentially a singleton.
/// </summary>
[RequireComponent(typeof(Collider2D))]
public class CellPhone : MonoBehaviour
{
	/// <summary>
	/// A static reference to what should be the only instance of this object in the scene.
	/// </summary>
	static public CellPhone Instance { get; private set; }


	public GUIStyle SmallTextStyle, LargeTextStyle, ButtonStyle;
	public Texture2D CallButtonTex, ContactsButtonTex, InternetButtonTex, MessengerButtonTex,
					 ChatButtonTex, MapsButtonTex, FilesButtonTex, FlashlightButtonTex,
					 WeatherButtonTex, DatingButtonTex, CalendarButtonTex, SettingsButtonTex;
	public Vector2 VisiblePosition;
	public Vector2 SpriteBorderSize, MainScreenButtonSize;


	/// <summary>
	/// Cached reference to this object's collider.
	/// Calculated on Awake().
	/// </summary>
	public Collider2D MyCollider { get; private set; }
	/// <summary>
	/// Cached reference to this object's transform.
	/// Calculated on Awake().
	/// </summary>
	public Transform MyTransform { get; private set; }
	/// <summary>
	/// This phone's sprite.
	/// </summary>
	public Sprite MySprite { get; private set; }

	/// <summary>
	/// The current state of this cell phone (i.e. what screen it's on).
	/// </summary>
	[NonSerialized]public CPState_Base CurrentState { get; set; }
	/// <summary>
	/// The position of this cell phone when it's not in use.
	/// </summary>
	public Vector2 StartingPosition { get; private set; }


	void Awake()
	{
		MyCollider = collider2D;

		if (Instance != null)
		{
			Debug.LogError("There is more than one CellPhone component: one in the '" + gameObject.name +
						       "' object and one in the '" + Instance.gameObject.name + "' object'");
		}

		CurrentState = null;
		MyTransform = transform;
		StartingPosition = new Vector2(MyTransform.position.x, MyTransform.position.y);
	}

	/// <summary>
	/// Should be raised when this phone is clicked on (PlayerInputController handles clicking inputs).
	/// </summary>
	public void OnClicked(Vector2 mousePos)
	{
		if (CurrentState == null)
		{
			MyTransform.position = VisiblePosition;
			CurrentState = new CPState_MainScreen();
		}
	}
}