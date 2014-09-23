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
	public CPState_Base CurrentState { get; set; }
	/// <summary>
	/// The position of this cell phone when it's not in use.
	/// </summary>
	public Vector3 StartingPosition { get; private set; }
	/// <summary>
	/// Whether this Cell Phone is currently active.
	/// </summary>
	public bool IsUp { get { return CurrentState != null; } }


	void Awake()
	{
		MyCollider = collider2D;
		MySprite = GetComponent<SpriteRenderer>().sprite;
		MyTransform = transform;

		if (Instance != null)
		{
			Debug.LogError("There is more than one CellPhone component: one in the '" + gameObject.name +
						       "' object and one in the '" + Instance.gameObject.name + "' object'");
		}
		Instance = this;

		CurrentState = null;
		StartingPosition = MyTransform.position;
	}
	void OnGUI()
	{
		if (CurrentState != null)
			CurrentState = CurrentState.OnGUI();
	}

	/// <summary>
	/// Should be raised when this phone is clicked on (PlayerInputController handles clicking inputs).
	/// </summary>
	public void OnClickedOn(Vector2 mousePos)
	{
		if (!IsUp)
		{
			MyTransform.position = new Vector3(VisiblePosition.x, VisiblePosition.y, MyTransform.position.z);
			CurrentState = new CPState_MainScreen();
		}
	}
	/// <summary>
	/// Should be raised when the mouse clicks somewhere that ISN'T this phone
	/// (PlayerInputController handles clicking inputs).
	/// </summary>
	public void OnClickedOff(Vector2 mousePos)
	{
		if (IsUp)
		{
			CurrentState.OnInterrupt();
			CurrentState = null;
			MyTransform.position = StartingPosition;
		}
	}
}