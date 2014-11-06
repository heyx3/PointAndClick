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
	/// <summary>
	/// The texture that the game is being rendered to.
	/// </summary>
	static private RenderTexture GameRendTex { get { return MainCamera.Instance.targetTexture; } }


	#region Screen data structs

	[Serializable]
	public class MainScreenData
	{
		public Texture2D CallButtonTex, ContactsButtonTex, InternetButtonTex, MessengerButtonTex,
						 ChatButtonTex, MapsButtonTex, FilesButtonTex, FlashlightButtonTex,
						 WeatherButtonTex, DatingButtonTex, CameraButtonTex, SettingsButtonTex;
		public Texture2D MessengerButtonTexNotification;
		public Vector2 ScreenBorder = new Vector2(15.43f, -33.93f);
		public Vector2 ButtonSize;
	}
	[Serializable]
	public class OfflineScreenData
	{
		public Texture2D Background;
	}
	[Serializable]
	public class MessengerScreenData
	{
		public Texture2D Background, NewSendMessage, NoSendMessage;
		public float MessageXBorderLerp = 0.01f;
		public float FirstMessageYLerp = 0.2f,
					 MessageSeparationLerp = 0.1f;
		public Vector2 MessageBoxTopLeftLerp = new Vector2(0.547945f, 1.0f - 0.798077f),
					   MessageBoxBottomRightLerp = new Vector2(0.712329f, 1.0f - 0.951923f);
		public float TextHeightOffset = -45449.0f;
		public Vector2 MessageButtonCenterLerp = new Vector2(0.863014f, 0.125f);
		public float WaitTimeBeforeReply = 2.0f,
					 PlayerTypeInterval = 0.1f;

		[Serializable]
		public class Message
		{
			public Texture2D Image;
			public string MessageText = "";
			public bool FromPlayer = true;

			/// <summary>
			/// How to transition to the next message after this one.
			/// </summary>
			public enum NextMessageKinds
			{
				/// <summary>
				/// The next message will not be sent to/by the player
				/// until it is manually triggered by some story action.
				/// </summary>
				WaitForTrigger,
				/// <summary>
				/// The next message is ready to be sent/received
				/// by the player as soon as this one is visible.
				/// </summary>
				Continue,
			}
			/// <summary>
			/// How to transition to the next message after this one.
			/// </summary>
			public NextMessageKinds NextMessageType = NextMessageKinds.WaitForTrigger;
		}
		public Message[] Messages = new Message[0];
	}
	[Serializable]
	public class CallsScreenData
	{
		public Texture2D Background;
	}
	[Serializable]
	public class DatingScreenData
	{
		public Texture2D Background;
	}
	[Serializable]
	public class ContactsScreenData
	{
		public Texture2D Background;
	}
	[Serializable]
	public class CameraScreenData
	{
		public Texture2D Background, PhotoButton;
		public Vector2 PhotoButtonPosLerp;

		public Texture2D DisplayTex;
		public float DisplayTexYOffsetLerp = 0.5f;
	}

	#endregion

	public MainScreenData MainScreen;
	public OfflineScreenData OfflineScreen;
	public MessengerScreenData MessengerScreen;
	public CallsScreenData CallsScreen;
	public DatingScreenData DatingScreen;
	public ContactsScreenData ContactsScreen;
	public CameraScreenData CameraScreen;

	public Texture2D CellPhoneTex;
	public GUIStyle SmallTextStyle, LargeTextStyle, ButtonStyle;

	public float DeSelectedHeight = -158.375f,
				 SelectedHeight = -26.7f;
	public Vector2 BackgroundSpriteBorderSize,
				   BackgroundSpriteOffset;

	
	/// <summary>
	/// Whether there is a new message for the player to see.
	/// </summary>
	public bool TriggerNewMessage
	{
		get { return triggerNewMsg; }
		set
		{
			if (value)
			{
				SoundAssets.Instance.PlaySound(SoundAssets.Instance.TextNotification);
			}

			triggerNewMsg = value;
		}
	}
	private bool triggerNewMsg = false;

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
	/// The current state of this cell phone (i.e. what screen it's on).
	/// </summary>
	public CPState_Base CurrentState { get; set; }
	/// <summary>
	/// Whether this Cell Phone is currently active.
	/// </summary>
	public bool IsUp { get { return CurrentState != null; } }


	private Vector2 scrollViewPos = Vector2.zero;

	void Awake()
	{
		MyCollider = collider2D;
		MyTransform = transform;

		if (Instance != null)
		{
			Debug.LogError("There is more than one CellPhone component: one in the '" + gameObject.name +
						       "' object and one in the '" + Instance.gameObject.name + "' object'");
		}
		Instance = this;

		CurrentState = null;
	}
	void Update()
	{
		MyTransform.localPosition = new Vector3(MyTransform.localPosition.x,
												(IsUp ? SelectedHeight : DeSelectedHeight),
												MyTransform.localPosition.z);
	}
	void OnGUI()
	{
		ScreenPositioningData data = new ScreenPositioningData(MyCollider);
		Vector2 center = (data.MinPos + data.MaxPos) * 0.5f;

		Vector2 cellSize = new Vector2(data.ScreenSizeScale.x * CellPhoneTex.width,
									   data.ScreenSizeScale.y * CellPhoneTex.height);
		GUI.DrawTexture(new Rect(center.x - (cellSize.x * 0.5f),
								 center.y - (cellSize.y * 0.5f),
								 cellSize.x, cellSize.y),
						CellPhoneTex, ScaleMode.StretchToFill, true);

		if (CurrentState != null)
		{
			CurrentState = CurrentState.OnGUI(data);
			if (CurrentState == null)
			{
				MyTransform.localPosition = new Vector3(MyTransform.localPosition.x,
														SelectedHeight, MyTransform.localPosition.z);
			}
		}
	}
	void OnDrawGizmos()
	{
		if (MyCollider is BoxCollider2D)
		{
			Bounds bnds = MyCollider.bounds;
			Vector2 boxCenter = (MyCollider as BoxCollider2D).center;

			Gizmos.color = Color.grey;
			Gizmos.DrawCube(bnds.center - new Vector3(boxCenter.x, boxCenter.y, 0.0f), bnds.size);
		}
	}

	/// <summary>
	/// Should be raised when this phone is clicked on (PlayerInputController handles clicking inputs).
	/// </summary>
	public void OnClickedOn(Vector2 mousePos)
	{
		if (!IsUp)
		{
			MyTransform.localPosition = new Vector3(MyTransform.localPosition.x,
													SelectedHeight, MyTransform.localPosition.z);
			CurrentState = new CPState_MainScreen();
		}
	}
	/// <summary>
	/// Should be raised when the mouse clicks somewhere that ISN'T this phone
	/// (PlayerInputController handles clicking inputs).
	/// Returns whether the phone should actually NOT be clicked off.
	/// </summary>
	public bool OnClickedOff(Vector2 mousePos)
	{
		if (IsUp)
		{
			bool cancel = CurrentState.OnInterrupt();
			if (!cancel)
			{
				CurrentState = null;
				MyTransform.localPosition = new Vector3(MyTransform.localPosition.x,
														SelectedHeight, MyTransform.localPosition.z);
			}
			return cancel;
		}

		return false;
	}
}